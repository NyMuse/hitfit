CREATE TABLE public."MeasurementsTypes"
(
  "Id" SERIAL PRIMARY KEY,
  "Key" character varying(64) NOT NULL,
  "Description" character varying (512),
  UNIQUE ("Key")
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public."MeasurementsTypes"
  OWNER TO postgres;

CREATE TABLE public."ProgramTypes"
(
  "Id" SERIAL PRIMARY KEY,
  "Key" character varying(64) NOT NULL,
  "Description" character varying (512)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public."ProgramTypes"
  OWNER TO postgres;

CREATE TABLE public."ReportTypes"
(
  "Id" SERIAL PRIMARY KEY,
  "Key" character varying(64) NOT NULL,
  "Description" character varying (512)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public."ReportTypes"
  OWNER TO postgres;