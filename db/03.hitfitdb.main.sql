CREATE TABLE public.Users
(
  Id SERIAL PRIMARY KEY,
  IsActive boolean NOT NULL DEFAULT(true),
  UserName character varying(64) NOT NULL CONSTRAINT UC_UserName UNIQUE,
  NormalizedUserName character varying(64) NOT NULL CONSTRAINT UC_NormalizedUserName UNIQUE,
  Email character varying(256) NOT NULL CONSTRAINT UC_Email UNIQUE,
  NormalizedEmail character varying(256) NOT NULL CONSTRAINT UC_NormalizedEmail UNIQUE,
  EmailConfirmed boolean NOT NULL DEFAULT(false),
  PhoneNumber character varying(256) NOT NULL CONSTRAINT UC_PhoneNumber UNIQUE,
  PhoneNumberConfirmed boolean NOT NULL DEFAULT(false),
  PasswordHash character varying(256) NOT NULL,
  FirstName character varying(256) NOT NULL,
  LastName character varying(256) NOT NULL,
  CreatedOn timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  ModifiedOn timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  LockoutEnabled boolean NOT NULL DEFAULT(true),
  AccessFailedCount smallint NOT NULL DEFAULT(0),
  LockoutEnd timestamp without time zone,
  Birthday timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  Sex character varying(8),
  Photo bytea,
  Notes text
);
ALTER TABLE public.Users OWNER TO postgres;

CREATE TABLE public.Roles
(
  Id SERIAL PRIMARY KEY,
  Name character varying(256) NOT NULL CONSTRAINT UC_RolNeame UNIQUE,
  NormalizedName character varying(256),
  ConcurrencyStamp character varying(256)
);
ALTER TABLE public.Roles OWNER TO postgres;

CREATE TABLE public.UserRoles
(
  Id SERIAL PRIMARY KEY,
  UserId int,
  RoleId int
);
ALTER TABLE public.UserRoles OWNER TO postgres;

CREATE TABLE public.Images
(
  Id SERIAL PRIMARY KEY,
  RelationType character varying(64) REFERENCES public.ImageRelationTypes (Key),
  OwnerId int NOT NULL,
  Image bytea
);
ALTER TABLE public.Images OWNER TO postgres;

CREATE TABLE public.Programs
(
  Id SERIAL PRIMARY KEY,
  Name character varying(256),
  Type character varying(64),
  IsActive boolean NOT NULL DEFAULT(true),
  CreatedOn timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  ModifiedOn timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  StartDate timestamp without time zone,
  FinishDate timestamp without time zone,
  Description text
);
ALTER TABLE public.Programs OWNER TO postgres;

CREATE TABLE public.UserPrograms
(
  Id SERIAL PRIMARY KEY,
  UserId int NOT NULL,
  ProgramId int NOT NULL,
  IsActive boolean NOT NULL DEFAULT(false),
  CreatedOn timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  ModifiedOn timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  StartedOn timestamp without time zone,
  FinishedOn timestamp without time zone,
  Notes text,

  CONSTRAINT UserPrograms_UserId_fkey FOREIGN KEY (UserId)
      REFERENCES public.Users (Id) MATCH SIMPLE,
  CONSTRAINT UserPrograms_ProgramId_fkey FOREIGN KEY (ProgramId)
      REFERENCES public.Programs (Id) MATCH SIMPLE
);
ALTER TABLE public.Users OWNER TO postgres;

CREATE TABLE public.UserMeasurements
(
  Id SERIAL PRIMARY KEY,
  UserId int NOT NULL,
  UserProgramId int NOT NULL,
  Type character varying(64) REFERENCES public.MeasurementTypes (Key),
  CreatedOn timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  ModifiedOn timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  Growth smallint,
  Weight smallint,
  Wrist smallint,
  Hand smallint,
  Breast smallint,
  WaistTop smallint,
  WaistMiddle smallint,
  WaistBottom smallint,
  Buttocks smallint,
  Thighs smallint,
  Leg smallint,
  KneeTop smallint,

  CONSTRAINT UserMeasurements_UserId_fkey FOREIGN KEY (UserId)
      REFERENCES public.Users (Id) MATCH SIMPLE,
  CONSTRAINT UserMeasurements_UserProgramId_fkey FOREIGN KEY (UserProgramId)
      REFERENCES public.UserPrograms (Id) MATCH SIMPLE
);
ALTER TABLE public.UserMeasurements OWNER TO postgres;

CREATE TABLE public.Reports
(
  Id SERIAL PRIMARY KEY,
  UserId int NOT NULL,
  UserProgramId int NOT NULL,
  Type character varying(64),
  CreatedOn timestamp without time zone DEFAULT (now() at time zone 'utc') NOT NULL,
  Description text,

  CONSTRAINT Reports_UserId_fkey FOREIGN KEY (UserId)
      REFERENCES public.Users (Id) MATCH SIMPLE,
  CONSTRAINT Reports_UserProgramId_fkey FOREIGN KEY (UserProgramId)
      REFERENCES public.UserPrograms (Id) MATCH SIMPLE
);
ALTER TABLE public.Reports OWNER TO postgres;

CREATE TABLE public.Articles
(
  Id SERIAL PRIMARY KEY,
  DocumentId character varying(512) NOT NULL CONSTRAINT UC_DocumentId UNIQUE,
  Title character varying(512) NOT NULL,
  HeaderImage text NOT NULL,
  Content text NOT NULL,
  Published boolean NOT NULL DEFAULT(false)
);
ALTER TABLE public.Articles OWNER TO postgres;