CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "ClickEvents" (
    id uuid NOT NULL,
    ts timestamp with time zone NOT NULL,
    user_id uuid NOT NULL,
    button text NOT NULL,
    clicks integer NOT NULL,
    CONSTRAINT "PK_ClickEvents" PRIMARY KEY (id)
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240419084527_InitialCreate', '8.0.4');

COMMIT;

