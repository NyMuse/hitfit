CREATE TABLE public."Users"
(
  "Id" SERIAL PRIMARY KEY,
  "IsAdministrator" boolean NOT NULL DEFAULT(false),
  "IsActive" boolean NOT NULL DEFAULT(true),
  "UserName" character varying(64) NOT NULL CONSTRAINT "UC_UserName" UNIQUE,
  "NormalizedUserName" character varying(64) NOT NULL CONSTRAINT "UC_NormalizedUserName" UNIQUE,
  "Email" character varying(256) NOT NULL CONSTRAINT "UC_Email" UNIQUE,
  "NormalizedEmail" character varying(256) NOT NULL CONSTRAINT "UC_NormalizedEmail" UNIQUE,
  "EmailConfirmed" boolean NOT NULL DEFAULT(false),
  "PhoneNumber" character varying(256) NOT NULL CONSTRAINT "UC_PhoneNumber" UNIQUE,
  "PhoneNumberConfirmed" boolean NOT NULL DEFAULT(false),
  "PasswordHash" character varying(256) NOT NULL,
  "FirstName" character varying(256) NOT NULL,
  "LastName" character varying(256) NOT NULL,
  "CreatedOn" timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  "ModifiedOn" timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  "LockoutEnabled" boolean NOT NULL DEFAULT(true),
  "AccessFailedCount" smallint NOT NULL DEFAULT(0),
  "LockoutEnd" timestamp without time zone
);
ALTER TABLE public."Users" OWNER TO postgres;

CREATE TABLE public."Roles"
(
  "Id" SERIAL PRIMARY KEY,
  "Name" character varying(256),
  "NormalizedName" character varying(256),
  "ConcurrencyStamp" character varying(256)
);
ALTER TABLE public."Roles" OWNER TO postgres;

CREATE TABLE public."UserRoles"
(
  "Id" SERIAL PRIMARY KEY,
  "UserId" int,
  "RoleId" int
);
ALTER TABLE public."UserRoles" OWNER TO postgres;

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

CREATE TABLE public."Images"
(
  "Id" SERIAL PRIMARY KEY,
  "RelationType" character varying(64) REFERENCES public."ImageRelationTypes" ("Key"),
  "OwnerId" int NOT NULL,
  "Image" bytea
);
ALTER TABLE public."Images" OWNER TO postgres;

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
  "Activated" boolean NOT NULL DEFAULT(false),
  "CreatedOn" timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  "ModifiedOn" timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
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
  "ModifiedOn" timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
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
  "Description" text,

  CONSTRAINT "Reports_UserId_fkey" FOREIGN KEY ("UserId")
      REFERENCES public."Users" ("Id") MATCH SIMPLE,
  CONSTRAINT "Reports_UserProgramId_fkey" FOREIGN KEY ("UserProgramId")
      REFERENCES public."UsersPrograms" ("Id") MATCH SIMPLE
);
ALTER TABLE public."Reports" OWNER TO postgres;

CREATE TABLE public."Articles"
(
  "Id" SERIAL PRIMARY KEY,
  "AuthorId" int NOT NULL,
  "CreatorId" int NOT NULL,
  "CategoryId" int NOT NULL,
  "Title" character varying(512),
  "SubTitle" character varying(512),
  "Content" text,
  "Image" bytea,
  "CreatedOn" timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  "UpdatedOn" timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  "IsPublished" boolean NOT NULL DEFAULT(false),

  CONSTRAINT "Articles_AuthorId_fkey" FOREIGN KEY ("AuthorId")
      REFERENCES public."Users" ("Id") MATCH SIMPLE,
  CONSTRAINT "Articles_CategoryId_fkey" FOREIGN KEY ("CategoryId")
      REFERENCES public."ArticleCategories" ("Id") MATCH SIMPLE
);
ALTER TABLE public."Articles" OWNER TO postgres;