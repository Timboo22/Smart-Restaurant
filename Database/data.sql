-- Startdaten für die Smart-Restaurant-Datenbank: Tische, Speisekarte und Mitarbeiter.
-- Wird beim ersten Start des DB-Containers nach schema.sql ausgeführt (nur bei leerem Volume).

INSERT INTO tisch (tisch_id, plaetze, status) VALUES
    (1, 2, false),
    (2, 2, false),
    (3, 4, false),
    (4, 4, false),
    (5, 4, false),
    (6, 6, false),
    (7, 6, false),
    (8, 4, false),
    (9, 2, false),
    (10, 4, false),
    (11, 6, false),
    (12, 8, false);

-- Tisch-Ids sind explizit gesetzt, daher die Sequenz nachziehen, damit neue Tische bei 13 weitermachen.
SELECT setval(pg_get_serial_sequence('tisch', 'tisch_id'), (SELECT MAX(tisch_id) FROM tisch));

INSERT INTO artikel (name, preis, kategorie) VALUES
    ('Hausgemachte Suppe', 5.90, 'Essen'),
    ('Gemischter Salat', 7.50, 'Essen'),
    ('Bruschetta', 6.80, 'Essen'),
    ('Lachs Carpaccio', 11.50, 'Essen'),
    ('Wiener Schnitzel', 18.90, 'Essen'),
    ('Spaghetti Bolognese', 13.50, 'Essen'),
    ('Gegrillter Lachs', 22.00, 'Essen'),
    ('Rumpsteak 200g', 28.50, 'Essen'),
    ('Veganer Burger', 15.90, 'Essen'),
    ('Risotto mit Pilzen', 16.50, 'Essen'),
    ('Crème Brûlée', 7.50, 'Essen'),
    ('Schokoladenkuchen', 6.90, 'Essen'),
    ('Gemischtes Eis', 5.50, 'Essen'),
    ('Apfelstrudel', 6.50, 'Essen'),
    ('Mineralwasser 0,5l', 3.50, 'Trinken'),
    ('Weizenbier 0,5l', 4.20, 'Trinken'),
    ('Hauswein 0,25l', 5.80, 'Trinken'),
    ('Espresso', 2.80, 'Trinken'),
    ('Orangensaft 0,3l', 3.90, 'Trinken'),
    ('Cappuccino', 3.50, 'Trinken');

INSERT INTO mitarbeiter (name, benutzername, rolle) VALUES
    ('Anna Schmidt', 'aschmidt', 'Service'),
    ('Ben Müller', 'bmueller', 'Service'),
    ('Clara Weber', 'cweber', 'Küche'),
    ('David Fischer', 'dfischer', 'Manager');