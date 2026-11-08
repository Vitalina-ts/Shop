"""Інтеграційні HTTP-перевірки. Запустіть застосунок перед цим скриптом.
Потрібен Python 3; сторонні пакети не потрібні. Видаляє лише створені ним записи.
"""
import html
import re
import sys
import uuid
from http.cookiejar import CookieJar
from urllib.request import build_opener, HTTPCookieProcessor, Request
from urllib.parse import urlencode
from urllib.error import HTTPError
from html.parser import HTMLParser

BASE = sys.argv[1] if len(sys.argv) > 1 else 'http://localhost:5055'
opener = build_opener(HTTPCookieProcessor(CookieJar()))
passed = 0
created = []

class Page(HTMLParser):
    def __init__(self, text):
        super().__init__(convert_charrefs=True)
        self.inputs = {}; self.rows = []; self.row = None; self.cell = None
        self.feed(text)
    def handle_starttag(self, tag, attrs):
        a = dict(attrs)
        if tag == 'input' and 'name' in a and a.get('type') != 'checkbox':
            self.inputs[a['name']] = a.get('value', '')
        if tag == 'tr': self.row = []
        if tag == 'td': self.cell = ''
    def handle_data(self, data):
        if self.cell is not None: self.cell += data
    def handle_endtag(self, tag):
        if tag == 'td' and self.row is not None:
            self.row.append(self.cell.strip()); self.cell = None
        if tag == 'tr' and self.row:
            self.rows.append(self.row); self.row = None

def check(condition, message):
    global passed
    if not condition: raise AssertionError(message)
    passed += 1
    print('PASS:', message)

def request(path, data=None):
    req = Request(BASE + path, data=None if data is None else urlencode(data).encode())
    try:
        with opener.open(req, timeout=20) as r: return r.status, r.read().decode(), r.url
    except HTTPError as e: return e.code, e.read().decode(), e.url

def get(path):
    status, text, url = request(path)
    check(status == 200, 'GET ' + path)
    return text

def post(path, data, token_path=None):
    form = Page(get(token_path or path)).inputs
    form.update({k: str(v) for k, v in data.items()})
    return request(path, form)

def create(entity, data, marker):
    status, text, url = post('/' + entity + '/Create', data)
    check(status == 200 and url.endswith('/' + entity), 'Create ' + entity)
    rows = Page(text).rows
    row = next(r for r in rows if marker in ' '.join(r))
    ident = int(row[0]); created.append((entity, ident))
    return ident

def details_ids(order_id):
    text = get('/Orders/Details/' + str(order_id))
    return [int(i) for i in re.findall(r'/OrderDetails/Edit/(\d+)', text)]

try:
    for path in ['/', '/Products', '/Customers', '/Supermarkets', '/Orders', '/OrderDetails', '/css/site.css', '/lib/bootstrap/dist/css/bootstrap.min.css']:
        get(path)
    for entity in ['Products', 'Customers', 'Supermarkets', 'Orders', 'OrderDetails']:
        for action in ['Details', 'Edit', 'Delete']:
            check(request(f'/{entity}/{action}/2147483647')[0] == 404, f'404 {entity}/{action}')
    # Сортування звіряється за порядком рядків у відповіді SQL-запиту.
    for column, ascending, descending in [(2, 'name', 'name_desc'), (3, 'address', 'address_desc')]:
        a = Page(get('/Customers?sort=' + ascending)).rows
        b = Page(get('/Customers?sort=' + descending)).rows
        check([r[column] for r in a] == list(reversed([r[column] for r in b])), 'Customer sorting ' + ascending)
    rows = Page(get('/Customers?search=ann&sort=name_desc')).rows
    check(len(rows) >= 2 and all('ann' in (r[1]+r[2]).lower() for r in rows), 'Substring filtering with sorting')
    check(len(Page(get('/Customers?search=NO_MATCH_987654')).rows) == 0, 'Empty customer search')
    for page, expected in [(1,5),(2,5),(3,2),(-4,5),(999999,2)]:
        check(len(Page(get('/Supermarkets?page=' + str(page))).rows) == expected, 'Pagination ' + str(page))
    mark = 'SMOKE_' + uuid.uuid4().hex[:8]
    products = [create('Products', {'Name': mark + str(i), 'Price': price}, mark + str(i)) for i, price in enumerate([25,40,60])]
    customer = create('Customers', {'FirstName': mark, 'LastName':'Tester', 'Address':'Test', 'Discount':5}, mark)
    market = create('Supermarkets', {'Name':'000_' + mark, 'Address':'Test'}, mark)
    for entity, ident, data in [
        ('Products',products[0],{'Name':mark+'0edit','Price':25}),
        ('Customers',customer,{'FirstName':mark,'LastName':'TesterEdit','Address':'Test2','Discount':10}),
        ('Supermarkets',market,{'Name':'000_'+mark,'Address':'Test2'})]:
        check(post(f'/{entity}/Edit/{ident}',dict(data,Id=ident))[2].endswith('/'+entity), 'Edit '+entity)
        for action in ['Details','Delete']: get(f'/{entity}/{action}/{ident}')
    check(request('/Products/Create', {'Name':'NoToken','Price':'1'})[0] == 400, 'Antiforgery rejects missing token')
    for data in [{'Name':'','Price':1},{'Name':'x'*51,'Price':1},{'Name':'invalid','Price':-1}]:
        check(post('/Products/Create',data)[2].endswith('/Products/Create'), 'Invalid product rejected')
    check(post('/Customers/Create',{'FirstName':mark,'LastName':'Invalid','Discount':101})[2].endswith('/Customers/Create'), 'Invalid discount rejected')
    order_data = {'CustomerId':customer,'SupermarketId':market,'OrderDate':'2026-11-08'}
    for i, (pid, qty) in enumerate(zip(products,[2,3,1])):
        order_data.update({f'Items[{i}].ProductId':pid,f'Items[{i}].Selected':'true',f'Items[{i}].Quantity':qty})
    # Не копіюємо весь каталог з GET: перевіряємо мінімальний валідний POST binding.
    def order_post(path, data):
        token = Page(get(path)).inputs['__RequestVerificationToken']
        return request(path, dict(data, __RequestVerificationToken=token))
    before = len(Page(get('/Orders')).rows)
    invalids = []
    invalids.append(dict(order_data, CustomerId=2147483647))
    invalids.append(dict(order_data, SupermarketId=2147483647))
    invalids.append(dict(order_data, **{'Items[0].ProductId':2147483647}))
    invalids.append(dict(order_data, **{'Items[0].Quantity':0}))
    invalids.append(dict(order_data, **{'Items[0].Quantity':'abc'}))
    invalids.append(dict(order_data, **{'Items[1].ProductId':products[0]}))
    invalids.append({'CustomerId':customer,'SupermarketId':market,'OrderDate':'2026-11-08'})
    invalids.append(dict(order_data, OrderDate='invalid'))
    for data in invalids:
        status, text, url = order_post('/Orders/Create',data)
        check(status == 200 and url.endswith('/Orders/Create'), 'Invalid order rejected')
    check(len(Page(get('/Orders')).rows) == before, 'Invalid orders cause no partial inserts')
    forged = dict(order_data, **{'Items[0].Price':0,'Items[1].Price':0,'Items[2].Price':0})
    status,text,url = order_post('/Orders/Create',forged)
    check(status == 200 and '/Orders/Details/' in url, 'Create multi-product order')
    order = int(url.rsplit('/',1)[1]); created.append(('Orders',order))
    check('230.00' in text, 'Authoritative prices: 25*2+40*3+60=230.00')
    get('/Orders/Edit/' + str(order)); get('/Orders/Delete/' + str(order))
    for entity,ident in [('Products',products[0]),('Customers',customer),('Supermarkets',market)]:
        status,text,url = post(f'/{entity}/Delete/{ident}',{'Id':ident})
        check(status == 200 and '/Delete/' in url, 'Referenced deletion blocked '+entity)
        get(f'/{entity}/Details/{ident}')
    edit = dict(order_data,Id=order,**{'Items[0].Quantity':3,'Items[2].Selected':'false'})
    status,text,url = order_post('/Orders/Edit/'+str(order), edit)
    check('/Orders/Details/' in url and '195.00' in text, 'Edit order quantities and remove item')
    invalid_edit = dict(edit, **{'Items[0].Quantity':0})
    check('/Orders/Edit/' in order_post('/Orders/Edit/'+str(order), invalid_edit)[2], 'Invalid edit rejected')
    check('195.00' in get('/Orders/Details/'+str(order)), 'Invalid edit preserves existing data')
    status,text,url = post('/OrderDetails/Create',{'OrderId':order,'ProductId':products[2],'Quantity':2})
    check('/Orders/Details/' in url and '315.00' in text, 'Add order detail')
    ids = details_ids(order)
    for ident in ids: get('/OrderDetails/Details/'+str(ident)); get('/OrderDetails/Delete/'+str(ident))
    detail = ids[-1]
    check('/OrderDetails/Create' in post('/OrderDetails/Create',{'OrderId':order,'ProductId':products[0],'Quantity':1})[2], 'Duplicate detail rejected')
    check('/OrderDetails/Create' in post('/OrderDetails/Create',{'OrderId':2147483647,'ProductId':products[0],'Quantity':1})[2], 'Invalid OrderId rejected')
    status,text,url = post('/OrderDetails/Edit/'+str(detail),{'Id':detail,'OrderId':order,'ProductId':products[2],'Quantity':3})
    check('/Orders/Details/' in url and '375.00' in text, 'Edit order detail')
    status,text,url = post('/OrderDetails/Delete/'+str(detail),{'Id':detail})
    check('/Orders/Details/' in url and '195.00' in text, 'Delete order detail')
    # Редагування знову додає вилучений товар.
    check('255.00' in order_post('/Orders/Edit/'+str(order),dict(edit,**{'Items[2].Selected':'true','Items[2].Quantity':1}))[1], 'Edit order adds product')
    ids = details_ids(order)
    # Зміна товару позиції, додатна кількість і порожнє замовлення.
    removed = ids[-1]
    post('/OrderDetails/Delete/'+str(removed),{'Id':removed})
    target = ids[0]
    status,text,url = post('/OrderDetails/Edit/'+str(target),{'Id':target,'OrderId':order,'ProductId':products[2],'Quantity':2})
    check('/Orders/Details/' in url and '240.00' in text, 'Change product of order detail')
    check('/OrderDetails/Edit/' in post('/OrderDetails/Edit/'+str(target),{'Id':target,'OrderId':order,'ProductId':products[2],'Quantity':0})[2], 'Reject nonpositive detail quantity')
    for ident in details_ids(order): post('/OrderDetails/Delete/'+str(ident),{'Id':ident})
    text = get('/Orders/Details/'+str(order))
    check('0.00' in text and len(Page(text).rows) == 0, 'Empty order renders zero total')
    # Повертаємо позиції для окремої перевірки каскадного видалення.
    order_post('/Orders/Edit/'+str(order),dict(order_data,Id=order))
    ids = details_ids(order)
    check(post('/Orders/Delete/'+str(order),{'Id':order})[2].endswith('/Orders'), 'Delete order')
    created.remove(('Orders',order))
    for ident in ids: check(request('/OrderDetails/Details/'+str(ident))[0] == 404, 'Cascade deletes detail '+str(ident))
    for entity,ident in [('Products',products[0]),('Customers',customer),('Supermarkets',market)]: get(f'/{entity}/Details/{ident}')
    print('ALL SCENARIOS PASSED')
finally:
    for entity, ident in reversed(created):
        status,text,url = post(f'/{entity}/Delete/{ident}', {'Id':ident})
        check(status == 200 and '/Delete/' not in url, 'Cleanup '+entity+' '+str(ident))
    print('Successful checks:', passed)