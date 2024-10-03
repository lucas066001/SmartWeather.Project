-- init-slave.sql

-- Attendre que le master soit prêt
CREATE USER 'haproxy_check'@'%' IDENTIFIED BY 'somepassword';
GRANT USAGE ON *.* TO 'haproxy_check'@'%';
FLUSH PRIVILEGES;


DO SLEEP(25); -- Délai de 10 secondes (ajuste selon tes besoins)

-- Changez l'adresse IP et le port du master
CHANGE MASTER TO
    MASTER_HOST='mysql-master',
    MASTER_USER='repl',
    MASTER_PASSWORD='repl_password',
    MASTER_LOG_FILE='mysql-bin.000001',
    MASTER_LOG_POS=0;

-- Démarre la réplication
START SLAVE;

-- Affiche l'état de la réplication
SHOW SLAVE STATUS\G;
