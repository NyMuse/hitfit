CREATE TABLE public."Users"
(
  "Id" SERIAL PRIMARY KEY,
  "IsAdministrator" boolean NOT NULL DEFAULT(false),
  "IsActive" boolean NOT NULL DEFAULT(true),
  "Login" character varying(64) NOT NULL CONSTRAINT "UC_Login" UNIQUE,
  "Email" character varying(256) NOT NULL CONSTRAINT "UC_Email" UNIQUE,
  "Password" character varying(256) NOT NULL,
  "PasswordSalt" character varying(256) NOT NULL,
  "SecurityStamp" character varying(256) NOT NULL,
  "FirstName" character varying(256) NOT NULL,
  "MiddleName" character varying(256),
  "LastName" character varying(256) NOT NULL,
  "CreatedOn" timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  "ModifiedOn" timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL
);
ALTER TABLE public."Users" OWNER TO postgres;

CREATE TABLE public."UserClaims"
(
  "Id" SERIAL PRIMARY KEY,
  "UserId" int,
  "ClaimTYpe" character varying(256),
  "ClaimValue" character varying(256)
);
ALTER TABLE public."UserClaims" OWNER TO postgres;

CREATE TABLE public."UsersDetails"
(
  "Id" SERIAL PRIMARY KEY,
  "UserId" int NOT NULL CONSTRAINT "UC_UserId" UNIQUE,
  "Birthday" timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  "Sex" character varying(8),
  "Photo" bytea,
  "Notes" text,

  CONSTRAINT "UsersDetails_UserId_fkey" FOREIGN KEY ("UserId")
      REFERENCES public."Users" ("Id") MATCH SIMPLE
);
ALTER TABLE public."Users" OWNER TO postgres;

CREATE TABLE public."Programs"
(
  "Id" SERIAL PRIMARY KEY,
  "Name" character varying(256),
  "Type" character varying(64),
  "IsActive" boolean NOT NULL DEFAULT(true),
  "CreatedOn" timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  "ModifiedOn" timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  "StartDate" timestamp without time zone,
  "FinishDate" timestamp without time zone,
  "Description" text
);
ALTER TABLE public."Programs" OWNER TO postgres;

CREATE TABLE public."UsersPrograms"
(
  "Id" SERIAL PRIMARY KEY,
  "UserId" int NOT NULL,
  "ProgramId" int NOT NULL,
  "CreatedOn" timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  "StartedOn" timestamp without time zone,
  "FinishedOn" timestamp without time zone,
  "Notes" text,

  CONSTRAINT "UsersPrograms_UserId_fkey" FOREIGN KEY ("UserId")
      REFERENCES public."Users" ("Id") MATCH SIMPLE,
  CONSTRAINT "UsersPrograms_ProgramId_fkey" FOREIGN KEY ("ProgramId")
      REFERENCES public."Programs" ("Id") MATCH SIMPLE
);
ALTER TABLE public."Users" OWNER TO postgres;

CREATE TABLE public."UsersMeasurements"
(
  "Id" SERIAL PRIMARY KEY,
  "UserId" int NOT NULL,
  "UserProgramId" int NOT NULL,
  "Type" character varying(64) REFERENCES public."MeasurementTypes" ("Key"),
  "CreatedOn" timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
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

  CONSTRAINT "UsersMeasurements_UserId_fkey" FOREIGN KEY ("UserId")
      REFERENCES public."Users" ("Id") MATCH SIMPLE,
  CONSTRAINT "UsersMeasurements_UserProgramId_fkey" FOREIGN KEY ("UserProgramId")
      REFERENCES public."UsersPrograms" ("Id") MATCH SIMPLE
);
ALTER TABLE public."UsersMeasurements" OWNER TO postgres;

CREATE TABLE public."Reports"
(
  "Id" SERIAL PRIMARY KEY,
  "UserId" int NOT NULL,
  "UserProgramId" int NOT NULL,
  "Type" character varying(64),
  "CreatedOn" timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  "Photo" bytea,
  "Description" text,

  CONSTRAINT "Reports_UserId_fkey" FOREIGN KEY ("UserId")
      REFERENCES public."Users" ("Id") MATCH SIMPLE,
  CONSTRAINT "Reports_UserProgramId_fkey" FOREIGN KEY ("UserProgramId")
      REFERENCES public."UsersPrograms" ("Id") MATCH SIMPLE
);
ALTER TABLE public."Reports" OWNER TO postgres;