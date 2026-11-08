"""Перевірка збереження даних: prepare; перезапуск застосунку; verify."""
import json
import re
import sys
from pathlib import Path
from http.cookiejar import CookieJar
from urllib.request import build_opener, HTTPCookieProcessor, Request
from urllib.parse import urlencode
base = 'http://localhost:5055'
client = build_opener(HTTPCookieProcessor(CookieJar()))
state = Path(__file__).resolve().parents[1] / 'obj' / 'persistence-state.json'
def req(path, data=None):
    with client.open(Request(base+path, data=None if data is None else urlencode(data).encode()), timeout=20) as r:
        return r.read().decode(), r.url
def token(path):
    text,_ = req(path)
    return re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"',text).group(1)
def snapshots():
    return {route: sorted(re.findall(r'/'+route+r'/Details/(\d+)',req('/'+route)[0])) for route in ['Products','Customers','Supermarkets','Orders','OrderDetails']}
if sys.argv[1] == 'prepare':
    form = {'Name':'RESTART_CHECK_LAB5','Price':'12.34','__RequestVerificationToken':token('/Products/Create')}
    text,_ = req('/Products/Create',form)
    row = next(row for row in re.findall(r'<tr>(.*?)</tr>',text,re.S) if 'RESTART_CHECK_LAB5' in row)
    ident = int(re.search(r'/Products/Details/(\d+)',row).group(1))
    state.write_text(json.dumps({'id':ident,'snapshot':snapshots()}))
    print('PASS: created persistent product',ident,'and saved all list snapshots')
else:
    saved = json.loads(state.read_text())
    assert snapshots() == saved['snapshot'], 'Seed duplicated or persisted records changed'
    text,_ = req('/Products/Details/'+str(saved['id']))
    assert 'RESTART_CHECK_LAB5' in text and '12.34' in text
    path = '/Products/Delete/'+str(saved['id'])
    _,url = req(path,{'Id':saved['id'],'__RequestVerificationToken':token(path)})
    assert url.endswith('/Products')
    state.unlink()
    print('PASS: persisted product survives restart; all lists unchanged; no seed duplicates; test record removed')