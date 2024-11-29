create table "Instruments"
(
    "Name" text,
    "Id"   uuid not null
        constraint instrument_pkey
            primary key
);

alter table "Instruments"
    owner to postgres;

create table "Musicians"
(
    "Name"     text not null,
    "Lastname" text not null,
    "Surname"  text,
    "Id"       uuid not null
        constraint musician_pkey
            primary key
);

alter table "Musicians"
    owner to postgres;

create table "Concerts"
(
    "Date" text,
    "Id"   uuid not null
        constraint concert_pkey
            primary key,
    "Name" text not null,
    "Type" text
);

alter table "Concerts"
    owner to postgres;

create table "Sounds"
(
    "Name"   text,
    "Author" text,
    "Id"     uuid not null
        constraint sound_pkey
            primary key
);

alter table "Sounds"
    owner to postgres;

create table "MusicianOnConcerts"
(
    "ConcertId"  uuid not null
        constraint concert_id
            references "Concerts",
    "MusicianId" uuid not null
        constraint musician_id
            references "Musicians"
);

alter table "MusicianOnConcerts"
    owner to postgres;

create table "SoundOnConcerts"
(
    "ConcertId" uuid not null
        constraint concert_id
            references "Concerts",
    "SoundId"   uuid not null
        constraint sound_id
            references "Sounds"
);

alter table "SoundOnConcerts"
    owner to postgres;

create table "MusicianInstruments"
(
    "MusicianId"   uuid not null
        constraint musician_id
            references "Musicians",
    "InstrumentId" uuid not null
        constraint instrument_id
            references "Instruments"
);

alter table "MusicianInstruments"
    owner to postgres;

