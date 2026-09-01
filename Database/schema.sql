CREATE TABLE mitarbeiter(
    mitarbeiter_id SERIAL PRIMARY KEY,
    name varchar(100) NOT NULL ,
    benutzername varchar(100) UNIQUE NOT NULL ,
    rolle varchar(50) NOT NULL 
);

CREATE TABLE status_log(
    status_log_id SERIAL PRIMARY KEY ,
    bestellung_id INTEGER NOT NULL ,
    mitarbeiter_id INTEGER NOT NULL ,
    status varchar(50) NOT NULL ,
    zeitpunkt timestamp NOT NULL 
);

CREATE TABLE bestellung(
    bestellung_id SERIAL PRIMARY KEY ,
    tisch_id INTEGER NOT NULL ,
    status varchar(50) NOT NULL ,
    zeitpunkt timestamp NOT NULL 
);

CREATE TABLE tisch(
    tisch_id SERIAL PRIMARY KEY ,
    plaetze INTEGER NOT NULL ,
    status BOOLEAN NOT NULL
);

CREATE TABLE bestellposition (
    bestellposition_id SERIAL PRIMARY KEY ,
    bestellung_id INTEGER NOT NULL,
    aritkel_id INTEGER NOT NULL,
    menge INTEGER NOT NULL
);

CREATE TABLE artikel (
    artikel_id SERIAL PRIMARY KEY ,
    name varchar(100) NOT NULL,
    preis numeric NOT NULL,
    kategorie varchar(50) NOT NULL
);

CREATE TABLE artikel_zutaten(
    artikel_zutaten_id SERIAL PRIMARY KEY ,
    zutaten_id INTEGER NOT NULL ,
    menge INTEGER NOT NULL 
);

CREATE TABLE zutaten(
    zutaten_id SERIAL PRIMARY KEY ,
    zutaten_name varchar(50) NOT NULL 
);

CREATE TABLE lager(
    zutaten_lager_id SERIAL PRIMARY KEY ,
    soll INTEGER NOT NULL ,
    ist INTEGER NOT NULL 
)