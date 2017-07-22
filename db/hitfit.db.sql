-- Database: "hitfitdb"

DROP DATABASE IF EXISTS hitfitdb;

CREATE DATABASE hitfitdb
  WITH OWNER = postgres
       ENCODING = 'UTF8'
       TABLESPACE = pg_default
       LC_COLLATE = 'English_United States.1252'
       LC_CTYPE = 'English_United States.1252'
       CONNECTION LIMIT = -1;

CREATE TABLE public."Users"
(
  "Id" SERIAL PRIMARY KEY,
  "IsAdministrator" boolean,
  "Name" character varying(256) NOT NULL CONSTRAINT "UC_Name" UNIQUE,
  "Password" character varying(256),
  "PasswordSalt" character varying(256),
  "FirstName" character varying(256),
  "MiddleName" character varying(256),
  "LastName" character varying(256),
  "Birthday" date
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public."Users"
  OWNER TO postgres;

CREATE TABLE public."UserMeasurements"
(
  "Id" SERIAL PRIMARY KEY,
  "UserId" int NOT NULL,
  "Type" smallint,
  "Growth" smallint,
  "Weight" smallint,
  "Wrist" smallint,
  "Hand" smallint,
  "Breast" smallint,
  "WaistTop" smallint,
  "WaistMiddle" smallint,
  "WaistBottom" smallint,
  "Buttocks" smallint,
  "Things" smallint,
  "Leg" smallint,
  "KneeTop" smallint,
  CONSTRAINT "FK_UserMeasurements_UserId" FOREIGN KEY ("UserId")
      REFERENCES public."Users" ("Id") MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public."UserMeasurements"
  OWNER TO postgres;