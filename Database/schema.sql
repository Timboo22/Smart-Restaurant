CREATE TABLE mitarbeiter(
    mitarbeiter_id SERIAL PRIMARY KEY,
    name varchar(100) NOT NULL ,
    benutzername varchar(100) UNIQUE NOT NULL ,
    rolle varchar(50) NOT NULL 
);

CREATE TABLE tisch(
    tisch_id SERIAL PRIMARY KEY ,
    plaetze INTEGER NOT NULL ,
    status BOOLEAN NOT NULL
);

CREATE TABLE artikel (
    artikel_id SERIAL PRIMARY KEY ,
    name varchar(100) NOT NULL,
    preis numeric(10,2) NOT NULL,
    kategorie varchar(50) NOT NULL
);

CREATE TABLE zutaten(
    zutaten_id SERIAL PRIMARY KEY ,
    zutaten_name varchar(50) NOT NULL
);

CREATE TABLE bestellung(
    bestellung_id SERIAL PRIMARY KEY ,
    tisch_id INTEGER NOT NULL ,
    status varchar(50) NOT NULL ,
    zeitpunkt timestamp NOT NULL,

    CONSTRAINT fk_bestelung_tisch
        FOREIGN KEY (tisch_id)
        REFERENCES tisch(tisch_id)
);


CREATE TABLE status_log(
    status_log_id SERIAL PRIMARY KEY ,
    bestellung_id INTEGER NOT NULL ,
    mitarbeiter_id INTEGER NOT NULL ,
    status varchar(50) NOT NULL ,
    zeitpunkt TIMESTAMP NOT NULL,

    CONSTRAINT fk_status_log_bestellung
        FOREIGN KEY (bestellung_id)
        REFERENCES bestellung(bestellung_id),

    CONSTRAINT fk_status_log_mitarbeiter
        FOREIGN KEY (mitarbeiter_id)
        REFERENCES mitarbeiter(mitarbeiter_id)
);

CREATE TABLE bestellposition (
    bestellposition_id SERIAL PRIMARY KEY ,
    bestellung_id INTEGER NOT NULL,
    artikel_id INTEGER NOT NULL,
    menge INTEGER NOT NULL,

    CONSTRAINT fk_bestellposition_bestellung
        FOREIGN KEY (bestellung_id)
        REFERENCES bestellung(bestellung_id),

    CONSTRAINT fk_bestellposition_artikel
        FOREIGN KEY (artikel_id)
        REFERENCES artikel(artikel_id)                               
);

CREATE TABLE artikel_zutaten(
    artikel_id INTEGER NOT NULL ,
    zutaten_id INTEGER NOT NULL ,
    menge INTEGER NOT NULL,

    PRIMARY KEY (artikel_id, zutaten_id),

    CONSTRAINT fk_artikel_zutaten_artikel
        FOREIGN KEY (artikel_id)
        REFERENCES artikel(artikel_id),

    CONSTRAINT fk_artikel_zutaten_zutat
        FOREIGN KEY (zutaten_id)
        REFERENCES zutaten(zutaten_id)
);

CREATE TABLE lager(
    zutaten_lager_id SERIAL PRIMARY KEY ,
    zutaten_id INTEGER NOT NULL,
    soll INTEGER NOT NULL ,
    ist INTEGER NOT NULL,

    CONSTRAINT fk_lager_zutat
        FOREIGN KEY (zutaten_id)
        REFERENCES zutaten(zutaten_id)
);