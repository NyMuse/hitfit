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
  "Type" character varying(64) REFERENCES public."MeasurementsTypes" ("Key"),
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

CREATE TABLE public."Reports"
(
  "Id" SERIAL PRIMARY KEY,
  "UserId" int NOT NULL,
  "Type" smallint,
  "ReportTime" date,


  CONSTRAINT "FK_Reports_UserId" FOREIGN KEY ("UserId")
      REFERENCES public."Users" ("Id") MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public."Reports"
  OWNER TO postgres;