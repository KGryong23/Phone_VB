CREATE TABLE brands (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    created_at DATETIME NOT NULL,
    last_modified DATETIME NOT NULL
);

CREATE TABLE phones (
    id INT AUTO_INCREMENT PRIMARY KEY,
    model VARCHAR(255) NOT NULL,
    price DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    stock INT NOT NULL DEFAULT 0,
    brand_id INT DEFAULT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    last_modified DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    FOREIGN KEY (brand_id) REFERENCES brands(id)
);

CREATE TABLE roles (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(255)
);

-- 2. Bảng permissions
CREATE TABLE permissions (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    description VARCHAR(255)
);

-- 3. Bảng role_permissions (nhiều-nhiều)
CREATE TABLE role_permissions (
    role_id INT NOT NULL,
    permission_id INT NOT NULL,
    PRIMARY KEY (role_id, permission_id),
    FOREIGN KEY (role_id) REFERENCES roles(id) ON DELETE CASCADE,
    FOREIGN KEY (permission_id) REFERENCES permissions(id) ON DELETE CASCADE
);

-- 4. Bảng users (tham chiếu đến role_id)
CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    role_id INT NOT NULL,
    created_at DATETIME NOT NULL,
    last_modified DATETIME NOT NULL,
    FOREIGN KEY (role_id) REFERENCES roles(id) ON DELETE RESTRICT
);


CREATE TABLE stock_transactions (
    id INT AUTO_INCREMENT PRIMARY KEY,
    phone_id INT NOT NULL,
    user_id INT NOT NULL,              -- người yêu cầu nhập/xuất
    approved_by INT DEFAULT NULL,      -- người duyệt (có thể là admin)
    quantity INT NOT NULL,
    transaction_type ENUM('import', 'export') NOT NULL,
    status ENUM('pending', 'approved', 'rejected') NOT NULL DEFAULT 'pending',
    note TEXT,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    approved_at DATETIME DEFAULT NULL,

    FOREIGN KEY (phone_id) REFERENCES phones(id),
    FOREIGN KEY (user_id) REFERENCES users(id),
    FOREIGN KEY (approved_by) REFERENCES users(id)
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

INSERT INTO roles (name, description) VALUES ('admin', 'hihihihihi');
INSERT INTO roles (name, description) VALUES ('user', 'hahahahaha');

INSERT INTO users (username, password, role_id, created_at, last_modified)
VALUES 
-- Admin user
('elite', 'hashed', 1, NOW(), NOW()),

-- Normal user
('sync', 'hashed', 2, NOW(), NOW());

CREATE PROCEDURE upsert_phone (
    IN p_id INT,
    IN p_model VARCHAR(255),
    IN p_price DECIMAL(10,2),
    IN p_stock INT,
    IN p_brand_id INT
)
BEGIN
    IF p_id IS NULL OR p_id = 0 THEN
        -- Thêm mới
        INSERT INTO phones (model, price, stock, brand_id)
        VALUES (p_model, p_price, p_stock, p_brand_id);
    ELSE
        -- Cập nhật
        UPDATE phones
        SET
            model = p_model,
            price = p_price,
            stock = p_stock,
            brand_id = p_brand_id,
            last_modified = CURRENT_TIMESTAMP
        WHERE id = p_id;
    END IF;
END; 

