# Перелік створених файлів

Шляхи відносно кореня Git lab5. Службовий каталог .git та згенеровані bin/obj не включено. Ресурси стандартного MVC-шаблону також створено під час цієї роботи.

| Файл | Призначення |
|---|---|
| `.config/dotnet-tools.json` | Маніфест локального dotnet-ef 8.0.22 |
| `.gitignore` | Виключення збірок, тимчасових файлів і локальних секретів |
| `ShoppingSystem/appsettings.Development.json` | Параметри застосунку та журналювання |
| `ShoppingSystem/appsettings.json` | Параметри застосунку та журналювання |
| `ShoppingSystem/Controllers/CustomersController.cs` | CRUD відповідної сутності, валідація й асинхронне збереження |
| `ShoppingSystem/Controllers/HomeController.cs` | Головна сторінка та обробка помилок стандартного MVC |
| `ShoppingSystem/Controllers/OrderDetailsController.cs` | CRUD відповідної сутності, валідація й асинхронне збереження |
| `ShoppingSystem/Controllers/OrdersController.cs` | CRUD відповідної сутності, валідація й асинхронне збереження |
| `ShoppingSystem/Controllers/ProductsController.cs` | CRUD відповідної сутності, валідація й асинхронне збереження |
| `ShoppingSystem/Controllers/SupermarketsController.cs` | CRUD відповідної сутності, валідація й асинхронне збереження |
| `ShoppingSystem/Data/SampleData.cs` | Ідемпотентні демонстраційні дані |
| `ShoppingSystem/Data/ShoppingContext.cs` | Контекст EF Core, зв’язки та обмеження SQL |
| `ShoppingSystem/FILES.md` | Цей пофайловий перелік |
| `ShoppingSystem/Migrations/20261108151450_InitialShoppingSchema.cs` | Створення таблиць, індексів, зовнішніх ключів та SQL-обмежень |
| `ShoppingSystem/Migrations/20261108151450_InitialShoppingSchema.Designer.cs` | Метадані початкової міграції EF Core |
| `ShoppingSystem/Migrations/ShoppingContextModelSnapshot.cs` | Знімок поточної моделі EF Core |
| `ShoppingSystem/Models/Customer.cs` | Сутність, ключі, навігації та атрибути перевірки |
| `ShoppingSystem/Models/ErrorViewModel.cs` | Модель повідомлення про помилку |
| `ShoppingSystem/Models/Order.cs` | Сутність, ключі, навігації та атрибути перевірки |
| `ShoppingSystem/Models/OrderDetails.cs` | Сутність, ключі, навігації та атрибути перевірки |
| `ShoppingSystem/Models/Product.cs` | Сутність, ключі, навігації та атрибути перевірки |
| `ShoppingSystem/Models/Supermarket.cs` | Сутність, ключі, навігації та атрибути перевірки |
| `ShoppingSystem/Program.cs` | Реєстрація MVC і SQL Server, middleware, міграції та seed |
| `ShoppingSystem/Properties/launchSettings.json` | Профіль запуску на порту 5055 |
| `ShoppingSystem/README.md` | Повний звіт, аудит 12 вимог, результати перевірок і ручна інструкція |
| `ShoppingSystem/ShoppingSystem.csproj` | Цільова платформа .NET 8 і пакети EF Core |
| `ShoppingSystem/Verification/DatabaseChecks.ps1` | Перевірки SQL-схеми, зв’язків і початкових даних |
| `ShoppingSystem/Verification/restart.py` | Збереження даних і відсутність повторного seed після перезапуску |
| `ShoppingSystem/Verification/smoke.py` | 163 перевірки HTTP, CRUD, обмежень та підсумків |
| `ShoppingSystem/ViewModels/ListViewModels.cs` | Стан списків покупців і супермаркетів |
| `ShoppingSystem/ViewModels/OrderCreateViewModel.cs` | Форма замовлення та колекція вибраних товарів |
| `ShoppingSystem/Views/_ViewImports.cshtml` | Простори імен і MVC Tag Helpers |
| `ShoppingSystem/Views/_ViewStart.cshtml` | Вибір спільного макета |
| `ShoppingSystem/Views/Customers/_Fields.cshtml` | Повторно використані поля створення та редагування |
| `ShoppingSystem/Views/Customers/Create.cshtml` | Форма створення |
| `ShoppingSystem/Views/Customers/Delete.cshtml` | Підтвердження видалення через POST |
| `ShoppingSystem/Views/Customers/Details.cshtml` | Сторінка відомостей |
| `ShoppingSystem/Views/Customers/Edit.cshtml` | Форма редагування |
| `ShoppingSystem/Views/Customers/Index.cshtml` | Таблиця записів або головна сторінка |
| `ShoppingSystem/Views/Home/Index.cshtml` | Таблиця записів або головна сторінка |
| `ShoppingSystem/Views/Home/Privacy.cshtml` | Довідкова сторінка навчального застосунку |
| `ShoppingSystem/Views/OrderDetails/_Fields.cshtml` | Повторно використані поля створення та редагування |
| `ShoppingSystem/Views/OrderDetails/_Summary.cshtml` | Відомості та серверний розрахунок сум |
| `ShoppingSystem/Views/OrderDetails/Create.cshtml` | Форма створення |
| `ShoppingSystem/Views/OrderDetails/Delete.cshtml` | Підтвердження видалення через POST |
| `ShoppingSystem/Views/OrderDetails/Details.cshtml` | Сторінка відомостей |
| `ShoppingSystem/Views/OrderDetails/Edit.cshtml` | Форма редагування |
| `ShoppingSystem/Views/OrderDetails/Index.cshtml` | Таблиця записів або головна сторінка |
| `ShoppingSystem/Views/Orders/_Form.cshtml` | Колекційна форма замовлення: покупець, магазин, дата й товари |
| `ShoppingSystem/Views/Orders/_Summary.cshtml` | Відомості та серверний розрахунок сум |
| `ShoppingSystem/Views/Orders/Create.cshtml` | Форма створення |
| `ShoppingSystem/Views/Orders/Delete.cshtml` | Підтвердження видалення через POST |
| `ShoppingSystem/Views/Orders/Details.cshtml` | Сторінка відомостей |
| `ShoppingSystem/Views/Orders/Edit.cshtml` | Форма редагування |
| `ShoppingSystem/Views/Orders/Index.cshtml` | Таблиця записів або головна сторінка |
| `ShoppingSystem/Views/Products/_Fields.cshtml` | Повторно використані поля створення та редагування |
| `ShoppingSystem/Views/Products/Create.cshtml` | Форма створення |
| `ShoppingSystem/Views/Products/Delete.cshtml` | Підтвердження видалення через POST |
| `ShoppingSystem/Views/Products/Details.cshtml` | Сторінка відомостей |
| `ShoppingSystem/Views/Products/Edit.cshtml` | Форма редагування |
| `ShoppingSystem/Views/Products/Index.cshtml` | Таблиця записів або головна сторінка |
| `ShoppingSystem/Views/Shared/_Layout.cshtml` | Bootstrap-макет і навігація |
| `ShoppingSystem/Views/Shared/_Layout.cshtml.css` | Стандартні ізольовані стилі MVC-шаблону |
| `ShoppingSystem/Views/Shared/_ValidationScriptsPartial.cshtml` | Підключення клієнтської валідації |
| `ShoppingSystem/Views/Shared/Error.cshtml` | Українське повідомлення про помилку |
| `ShoppingSystem/Views/Supermarkets/_Fields.cshtml` | Повторно використані поля створення та редагування |
| `ShoppingSystem/Views/Supermarkets/Create.cshtml` | Форма створення |
| `ShoppingSystem/Views/Supermarkets/Delete.cshtml` | Підтвердження видалення через POST |
| `ShoppingSystem/Views/Supermarkets/Details.cshtml` | Сторінка відомостей |
| `ShoppingSystem/Views/Supermarkets/Edit.cshtml` | Форма редагування |
| `ShoppingSystem/Views/Supermarkets/Index.cshtml` | Таблиця записів або головна сторінка |
| `ShoppingSystem/wwwroot/css/site.css` | Стилі застосунку |
| `ShoppingSystem/wwwroot/favicon.ico` | Стандартна піктограма MVC-шаблону |
| `ShoppingSystem/wwwroot/js/site.js` | Стандартний файл JavaScript MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap.css` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap.css.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap.min.css` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap.min.css.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap.rtl.css` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap.rtl.css.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap.rtl.min.css` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap.rtl.min.css.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-grid.css` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-grid.css.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-grid.min.css` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-grid.min.css.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-grid.rtl.css` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-grid.rtl.css.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-grid.rtl.min.css` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-grid.rtl.min.css.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-reboot.css` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-reboot.css.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-reboot.min.css` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-reboot.min.css.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-reboot.rtl.css` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-reboot.rtl.css.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-reboot.rtl.min.css` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-reboot.rtl.min.css.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-utilities.css` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-utilities.css.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-utilities.min.css` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-utilities.min.css.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-utilities.rtl.css` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-utilities.rtl.css.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-utilities.rtl.min.css` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/css/bootstrap-utilities.rtl.min.css.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/js/bootstrap.bundle.js` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/js/bootstrap.bundle.js.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/js/bootstrap.bundle.min.js` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/js/bootstrap.bundle.min.js.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/js/bootstrap.esm.js` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/js/bootstrap.esm.js.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/js/bootstrap.esm.min.js` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/js/bootstrap.esm.min.js.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/js/bootstrap.js` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/js/bootstrap.js.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/js/bootstrap.min.js` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/dist/js/bootstrap.min.js.map` | Локальний ресурс Bootstrap 5 зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/bootstrap/LICENSE` | Ліцензія бібліотеки зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/jquery/dist/jquery.js` | Локальний jQuery зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/jquery/dist/jquery.min.js` | Локальний jQuery зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/jquery/dist/jquery.min.map` | Локальний jQuery зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/jquery/LICENSE.txt` | Ліцензія бібліотеки зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/jquery-validation/dist/additional-methods.js` | Стандартна клієнтська валідація jQuery |
| `ShoppingSystem/wwwroot/lib/jquery-validation/dist/additional-methods.min.js` | Стандартна клієнтська валідація jQuery |
| `ShoppingSystem/wwwroot/lib/jquery-validation/dist/jquery.validate.js` | Стандартна клієнтська валідація jQuery |
| `ShoppingSystem/wwwroot/lib/jquery-validation/dist/jquery.validate.min.js` | Стандартна клієнтська валідація jQuery |
| `ShoppingSystem/wwwroot/lib/jquery-validation/LICENSE.md` | Ліцензія бібліотеки зі стандартного MVC-шаблону |
| `ShoppingSystem/wwwroot/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js` | Стандартна unobtrusive validation |
| `ShoppingSystem/wwwroot/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js` | Стандартна unobtrusive validation |
| `ShoppingSystem/wwwroot/lib/jquery-validation-unobtrusive/LICENSE.txt` | Ліцензія бібліотеки зі стандартного MVC-шаблону |
