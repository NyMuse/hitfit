-- Database: "hitfit.db"

DROP DATABASE IF EXISTS "hitfit.db";

CREATE DATABASE "hitfit.db"
  WITH OWNER = postgres
       ENCODING = 'UTF8'
       TABLESPACE = pg_default
       LC_COLLATE = 'English_United States.1252'
       LC_CTYPE = 'English_United States.1252'
       CONNECTION LIMIT = -1;

CREATE TABLE public."Users"
(
  "Id" SERIAL PRIMARY KEY,
  "FirstName" character varying(255),
  "MiddleName" character varying(255),
  "LastName" character varying(255),
  "Birthday" date,
  "Growth" smallint,
  "Weight" smallint
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public."Users"
  OWNER TO postgres;
