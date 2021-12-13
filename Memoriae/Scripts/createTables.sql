CREATE TABLE memoriae."Post"
(
    "Id" uuid NOT NULL DEFAULT gen_random_uuid() primary key,
    "Title" character varying(512) COLLATE pg_catalog."default" NOT NULL,
	"Text" varchar COLLATE pg_catalog."default" NOT NULL,
	"CreateDateTime" timestamp without time zone not null 
	
)

CREATE TABLE memoriae."Tag"
(
    "Id" uuid NOT NULL DEFAULT gen_random_uuid()  primary key,
    "Name" character varying(512) COLLATE pg_catalog."default" NOT NULL	
)


CREATE TABLE memoriae."PostTagLink"
(
    "Id" uuid NOT NULL DEFAULT gen_random_uuid() primary key,
    "PostId" uuid REFERENCES memoriae."Post"("Id") not null,
	"TagId" uuid REFERENCES memoriae."Tag"("Id") not null
)
