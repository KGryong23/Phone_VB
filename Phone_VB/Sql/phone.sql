CREATE TABLE brands (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    created_at DATETIME NOT NULL,
    last_modified DATETIME NOT NULL
);

CREATE TABLE phones (
    id INT AUTO_INCREMENT PRIMARY KEY,
    model VARCHAR(255) NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    stock INT NOT NULL,
    brand_id INT,
    created_at DATETIME NOT NULL,
    last_modified DATETIME NOT NULL,
    FOREIGN KEY (brand_id) REFERENCES brands(id)
);

CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    role VARCHAR(50) NOT NULL,
    created_at DATETIME NOT NULL,
    last_modified DATETIME NOT NULL
);

INSERT INTO brands (name, created_at, last_modified)
VALUES
('Apple', NOW(), NOW()),
('Samsung', NOW(), NOW()),
('Xiaomi', NOW(), NOW());

INSERT INTO phones (model, price, stock, brand_id, created_at, last_modified)
VALUES
('iPhone 14', 999.99, 50, 1, NOW(), NOW()),
('iPhone 13', 799.99, 30, 1, NOW(), NOW()),
('iPhone SE', 429.99, 20, 1, NOW(), NOW()),
('Galaxy S23', 899.99, 40, 2, NOW(), NOW()),
('Galaxy A54', 499.99, 60, 2, NOW(), NOW()),
('Galaxy Z Flip5', 1199.99, 15, 2, NOW(), NOW()),
('Xiaomi 13', 649.99, 25, 3, NOW(), NOW()),
('Redmi Note 12', 229.99, 100, 3, NOW(), NOW()),
('Poco X5 Pro', 299.99, 80, 3, NOW(), NOW()),
('Mi 11 Ultra', 749.99, 10, 3, NOW(), NOW());

INSERT INTO users (username, password, role, created_at, last_modified)
VALUES
('elite', 'hashpassword', 'admin', NOW(), NOW()),
('elite23', 'hashpassword', 'user', NOW(), NOW());

INSERT INTO phones (model, price, stock, brand_id, created_at, last_modified)
VALUES
('iPhone 15', 999.99, 50, 1, NOW(), NOW());