CREATE TABLE public."MeasurementTypes"
(
  "Id" SERIAL PRIMARY KEY,
  "Key" character varying(64) NOT NULL,
  "Description" character varying (512),
  "IsActive" boolean NOT NULL DEFAULT (true),
  UNIQUE ("Key")
);
ALTER TABLE public."MeasurementTypes" OWNER TO postgres;

CREATE TABLE public."ProgramTypes"
(
  "Id" SERIAL PRIMARY KEY,
  "Key" character varying(64) NOT NULL,
  "Description" character varying (512),
  "IsActive" boolean NOT NULL DEFAULT (true),
  UNIQUE ("Key")
);
ALTER TABLE public."ProgramTypes" OWNER TO postgres;

CREATE TABLE public."ReportTypes"
(
  "Id" SERIAL PRIMARY KEY,
  "Key" character varying(64) NOT NULL,
  "Description" character varying (512),
  "IsActive" boolean NOT NULL DEFAULT (true),
  UNIQUE ("Key")
);
ALTER TABLE public."ReportTypes" OWNER TO postgres;