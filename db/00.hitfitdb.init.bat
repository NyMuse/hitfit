@echo off

SET dbserver=localhost
SET dbname=hitfitdb
SET user=postgres
SET PGPASSWORD=Tsunami9

psql --host=localhost --dbname=postgres --username=postgres -c "DROP DATABASE IF EXISTS hitfitdb"
psql --host=localhost --dbname=postgres --username=postgres --file="01.hitfitdb.create.sql"

psql \connect hitfitdb

psql --host=localhost --dbname=hitfitdb --username=postgres --file="02.hitfitdb.dictionaries.sql"
psql --host=localhost --dbname=hitfitdb --username=postgres --file="03.hitfitdb.main.sql"
