-- init-master.sql

-- Crée un utilisateur pour la réplication
CREATE USER 'repl'@'%' IDENTIFIED WITH mysql_native_password BY 'repl_password';
GRANT REPLICATION SLAVE ON *.* TO 'repl'@'%';

CREATE USER 'haproxy_check'@'%' IDENTIFIED BY 'somepassword';
GRANT USAGE ON *.* TO 'haproxy_check'@'%';
FLUSH PRIVILEGES;


-- -- Crée une table d'exemple
-- USE smartweather;
-- CREATE TABLE example (
--     id INT AUTO_INCREMENT PRIMARY KEY,
--     name VARCHAR(100) NOT NULL
-- );

-- -- Insère des données d'exemple
-- INSERT INTO example (name) VALUES ('Alice'), ('Bob'), ('Charlie');

-- -- Affiche les informations de la base de données
-- SELECT * FROM example;
