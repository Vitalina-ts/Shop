$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Data
$connection = New-Object System.Data.SqlClient.SqlConnection 'Server=(localdb)\MSSQLLocalDB;Database=ShoppingLab5Db;Integrated Security=True;TrustServerCertificate=True'
$connection.Open()
function Query($sql) {
    $cmd = $connection.CreateCommand(); $cmd.CommandText = $sql
    $adapter = New-Object System.Data.SqlClient.SqlDataAdapter $cmd
    $table = New-Object System.Data.DataTable
    [void]$adapter.Fill($table)
    return ,$table
}
function Assert($condition, $message) { if (!$condition) { throw $message }; Write-Output "PASS: $message" }
try {
    $counts = Query 'SELECT (SELECT COUNT(*) FROM Products) Products, (SELECT COUNT(*) FROM Customers) Customers, (SELECT COUNT(*) FROM Supermarkets) Supermarkets, (SELECT COUNT(*) FROM Orders) Orders, (SELECT COUNT(*) FROM OrderDetails) OrderDetails'
    $counts | Format-Table
    $r = $counts.Rows[0]
    Assert ($r.Products -ge 8 -and $r.Customers -ge 6 -and $r.Supermarkets -ge 12 -and $r.Orders -ge 5 -and $r.OrderDetails -ge 15) 'Seed data minimum counts'
    $tables = Query "SELECT name FROM sys.tables WHERE name IN ('Products','Customers','Supermarkets','Orders','OrderDetails')"
    Assert ($tables.Rows.Count -eq 5) 'Five entity tables exist'
    $keys = Query "SELECT t.name FROM sys.key_constraints k JOIN sys.tables t ON t.object_id=k.parent_object_id WHERE k.type='PK' AND t.name IN ('Products','Customers','Supermarkets','Orders','OrderDetails')"
    Assert ($keys.Rows.Count -eq 5) 'Five primary keys exist'
    $fk = Query 'SELECT name, delete_referential_action_desc FROM sys.foreign_keys ORDER BY name'
    $fk | Format-Table
    Assert ($fk.Rows.Count -eq 4) 'Four foreign keys exist'
    Assert (@($fk.Rows | Where-Object { $_.delete_referential_action_desc -eq 'CASCADE' -and $_.name -eq 'FK_OrderDetails_Orders_OrderId' }).Count -eq 1) 'Only order items cascade'
    Assert (@($fk.Rows | Where-Object { $_.delete_referential_action_desc -eq 'NO_ACTION' }).Count -eq 3) 'Three protected relationships'
    $columns = Query "SELECT t.name TableName, c.name ColumnName, ty.name DataType, c.max_length, c.precision, c.scale, c.is_nullable FROM sys.columns c JOIN sys.tables t ON t.object_id=c.object_id JOIN sys.types ty ON ty.user_type_id=c.user_type_id WHERE t.name IN ('Products','Customers','Supermarkets','Orders','OrderDetails') ORDER BY t.name,c.column_id"
    $columns | Format-Table
    $price = @($columns.Rows | Where-Object { $_.TableName -eq 'Products' -and $_.ColumnName -eq 'Price' })[0]
    Assert ($price.DataType -eq 'decimal' -and $price.precision -eq 18 -and $price.scale -eq 2 -and !$price.is_nullable) 'Price is required decimal(18,2)'
    $expected = @{ 'Products.Name'=100; 'Customers.FirstName'=100; 'Customers.LastName'=100; 'Customers.Address'=100; 'Supermarkets.Name'=100; 'Supermarkets.Address'=200 }
    foreach ($key in $expected.Keys) {
        $parts = $key.Split('.')
        $col = @($columns.Rows | Where-Object { $_.TableName -eq $parts[0] -and $_.ColumnName -eq $parts[1] })[0]
        Assert ($col.DataType -eq 'nvarchar' -and $col.max_length -eq $expected[$key]) "String length $key"
        if ($parts[1] -ne 'Address') { Assert (!$col.is_nullable) "Required $key" }
    }
    $checks = Query 'SELECT name, definition FROM sys.check_constraints'
    Assert ($checks.Rows.Count -eq 3) 'Three SQL check constraints'
    $unique = Query "SELECT name FROM sys.indexes WHERE name='IX_OrderDetails_OrderId_ProductId' AND is_unique=1"
    Assert ($unique.Rows.Count -eq 1) 'Unique order-product constraint'
    $migration = Query 'SELECT MigrationId, ProductVersion FROM __EFMigrationsHistory'
    Assert ($migration.Rows.Count -eq 1 -and $migration.Rows[0].MigrationId -like '*_InitialShoppingSchema') 'Initial migration applied'
    $total = Query 'SELECT TOP(1) o.Id, SUM(p.Price*d.Quantity) Total FROM Orders o JOIN OrderDetails d ON o.Id=d.OrderId JOIN Products p ON p.Id=d.ProductId GROUP BY o.Id ORDER BY o.Id'
    Assert ($total.Rows[0].Total -eq 230) 'Seed order SQL total equals 230.00'
    $orphans = Query 'SELECT COUNT(*) BadRows FROM OrderDetails d LEFT JOIN Orders o ON o.Id=d.OrderId LEFT JOIN Products p ON p.Id=d.ProductId WHERE o.Id IS NULL OR p.Id IS NULL OR d.Quantity<=0'
    Assert ($orphans.Rows[0].BadRows -eq 0) 'No orphan or invalid order items'
} finally { $connection.Dispose() }