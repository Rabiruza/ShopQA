# seed-database.ps1
# Populates the ShopQA SQLite database with demo test data
# AI-assisted: SQL generation helped by Microsoft Copilot

param(
    [string]$DbPath = ".\shopqa.db",
    [int]$ProductCount = 50
)

Write-Host "Seeding ShopQA database..." -ForegroundColor Cyan

# Check sqlite3 is available
if (-not (Get-Command sqlite3 -ErrorAction SilentlyContinue)) {
    Write-Error "sqlite3 not found. Install from https://sqlite.org/download.html"
    exit 1
}

$sql = @"
-- Users
INSERT OR IGNORE INTO Users (Email, Password, FirstName, LastName, Role) VALUES
('admin@shopqa.com',    'Admin123!',  'Admin',  'User',    'admin'),
('test@shopqa.com',     'Test123!',   'Test',   'User',    'user'),
('rosina@shopqa.com',   'Rosina123!', 'Rosina', 'Poloka',  'user');

-- Products
INSERT OR IGNORE INTO Products (Name, Category, Price, Stock, Description) VALUES
('Laptop Pro 15',        'Electronics',  1299.99, 25, 'High-performance laptop'),
('Wireless Mouse',       'Electronics',    29.99, 150,'Ergonomic wireless mouse'),
('Mechanical Keyboard',  'Electronics',    89.99,  80, 'RGB mechanical keyboard'),
('USB-C Hub',            'Electronics',    49.99, 120, '7-in-1 USB-C hub'),
('Monitor 27"',          'Electronics',   449.99,  30, '4K IPS monitor'),
('Running Shoes',        'Clothing',       79.99,  60, 'Lightweight running shoes'),
('Winter Jacket',        'Clothing',      129.99,  40, 'Waterproof winter jacket'),
('Cotton T-Shirt',       'Clothing',       19.99, 200, 'Premium cotton t-shirt'),
('SQL for Testers',      'Books',          34.99, 999, 'Database testing guide'),
('Clean Code',           'Books',          29.99, 999, 'Writing maintainable code'),
('Desk Lamp LED',        'Home',           39.99,  90, 'Adjustable LED desk lamp'),
('Coffee Maker',         'Home',           59.99,  55, 'Programmable coffee maker'),
('Yoga Mat',             'Sports',         24.99,  75, 'Non-slip yoga mat'),
('Water Bottle',         'Sports',         14.99, 300, 'Insulated water bottle'),
('Backpack 30L',         'Accessories',    59.99,  85, 'Travel backpack');
"@

$sql | sqlite3 $DbPath
Write-Host "Database seeded with users and $ProductCount products." -ForegroundColor Green
