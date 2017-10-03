@echo off

SET dbserver=localhost
SET dbname=hitfitdb
SET user=user
SET PGPASSWORD=password

psql --host=localhost --dbname=postgres --username=postgres -c "DROP DATABASE IF EXISTS hitfitdb"
psql --host=localhost --dbname=postgres --username=postgres --file="01.hitfitdb.create.sql"

psql \connect hitfitdb

psql --host=localhost --dbname=hitfitdb --username=postgres --file="02.hitfitdb.dictionaries.sql"
psql --host=localhost --dbname=hitfitdb --username=postgres --file="03.hitfitdb.main.sql"

IF "%1" == "-s" GOTO SAMPLEDATA
GOTO DONE

:SAMPLEDATA

ECHO Sample Data
psql --host=localhost --dbname=hitfitdb --username=postgres --file="04.hitfitdb.sampledata.sql"

GOTO DONE

:DONE

ECHO Done
