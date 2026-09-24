
# Smart Restaurant – API Dokumentation für Frontend-Entwickler

> [!INFO] Basis-URL
> - Lokal (dotnet run): `http://localhost:5027`
> - Docker Compose (aus anderen Containern): `http://api:8080`
> - Interaktive API-Referenz (nur im Development-Modus): `/scalar`
>
> Alle Endpunkte erwarten/liefern `application/json`. Alle JSON-Felder sind **camelCase** (z.B. `tischId`, `artikelName`).
>
> `zeitpunkt`-Felder haben **kein** `Z`-Suffix (z.B. `"2026-09-01T12:30:00"`), sind aber intern UTC — Details und alle weiteren JSON-Formate (inkl. Fehler- und Health-Antworten) stehen in [JSONFORMAT.md](./JSONFORMAT.md).

---

## 1. Bestellungen

### 1.1 Neue Bestellung aufgeben

`POST /api/bestellungen`

Legt eine neue Bestellung für einen Tisch an. Der Status wird automatisch auf `"Aufgenommen"` gesetzt, ein `zeitpunkt` wird serverseitig erzeugt.

**Request Body**

| Feld | Typ | Pflicht | Beschreibung |
|---|---|---|---|
| `tischId` | number | ja | ID eines existierenden Tisches |
| `positionen` | array | ja, min. 1 Eintrag | Liste der bestellten Artikel |
| `positionen[].artikelId` | number | ja | ID eines existierenden Artikels |
| `positionen[].menge` | number | ja | Bestellte Menge |

```json
{
  "tischId": 4,
  "positionen": [
    { "artikelId": 1, "menge": 2 },
    { "artikelId": 3, "menge": 1 }
  ]
}
```

**Response**

- `201 Created` – Body enthält die vollständige neu erstellte Bestellung (identisch zum Format von [1.2](#12-bestellung-details-abrufen)). Der Header `Location` zeigt auf `GET /api/bestellungen/{id}`.
- `400 Bad Request` – `positionen` ist leer.
- `404 Not Found` – `tischId` existiert nicht, oder eine/mehrere `artikelId` existieren nicht (Response-Body enthält eine `message` mit Details).

---

### 1.2 Bestellung Details abrufen

`GET /api/bestellungen/{id}`

Liefert eine einzelne Bestellung inkl. aller Positionen (mit aktuellem Artikelnamen/-preis) und der vollständigen Status-Historie.

**Response**

```json
{
  "bestellungId": 101,
  "tischId": 4,
  "status": "In Zubereitung",
  "zeitpunkt": "2026-09-01T12:30:00",
  "gesamtbetrag": 28.50,
  "positionen": [
    {
      "bestellpositionId": 1,
      "artikelId": 1,
      "artikelName": "Pizza Margherita",
      "einzelpreis": 9.50,
      "menge": 2,
      "gesamtpreis": 19.00
    }
  ],
  "statusLogs": [
    {
      "logId": 201,
      "mitarbeiterId": 2,
      "mitarbeiterName": "Max Mustermann",
      "status": "Aufgenommen",
      "zeitpunkt": "2026-09-01T12:30:00"
    }
  ]
}
```

- `einzelpreis`/`gesamtpreis` werden aus dem **aktuellen** Artikelpreis berechnet, nicht aus einem zum Bestellzeitpunkt eingefrorenen Preis.
- `gesamtbetrag` ist die Summe aller `gesamtpreis`-Werte der Positionen.
- `statusLogs` ist chronologisch aufsteigend sortiert.

**Status Codes**: `200 OK` / `404 Not Found` (Bestellung existiert nicht).

---

### 1.3 Bestellstatus ändern

`PATCH /api/bestellungen/{id}/status`

Setzt den Status der Bestellung neu und erzeugt automatisch einen Eintrag in der Status-Historie.

**Request Body**

| Feld | Typ | Pflicht | Beschreibung |
|---|---|---|---|
| `mitarbeiterId` | number | ja | ID des Mitarbeiters, der die Änderung vornimmt |
| `neuerStatus` | string | ja | Neuer Status-Text |

```json
{
  "mitarbeiterId": 5,
  "neuerStatus": "Servierbereit"
}
```

> [!WARNING] `neuerStatus` ist ein freies Textfeld (kein serverseitiges Enum). Damit Status-Filter im Frontend funktionieren, immer dieselben Strings verwenden, z.B. `"Aufgenommen"`, `"In Zubereitung"`, `"Servierbereit"`, `"Bezahlt"`.

**Response**

- `200 OK` – Body enthält die aktualisierte Bestellung (gleiches Format wie [1.2](#12-bestellung-details-abrufen)), inklusive des neuen Eintrags in `statusLogs`.
- `404 Not Found` – Bestellung existiert nicht, oder `mitarbeiterId` existiert nicht.

---

## 2. Speisekarte & Artikel

### 2.1 Artikel mit Zutatenliste

`GET /api/artikel`

Liefert alle Artikel inkl. der für jeden Artikel benötigten Zutaten (für z.B. Anzeige in der Speisekarte oder Verfügbarkeitsprüfung im Frontend).

```json
[
  {
    "artikelId": 1,
    "name": "Pizza Margherita",
    "preis": 9.50,
    "kategorie": "Hauptspeise",
    "zutaten": [
      { "zutatenId": 1, "zutatenName": "Teigling", "anzahl": 1 },
      { "zutatenId": 2, "zutatenName": "Tomatensauce", "anzahl": 1 }
    ]
  }
]
```

**Status Codes**: `200 OK` (auch bei leerer Liste).

---

## 3. Lager & Zutatenbestand

### 3.1 Lagerbestand abfragen

`GET /api/lager`

```json
[
  { "zutatenId": 1, "zutatenName": "Teigling", "soll": 100, "ist": 85, "nachbestellenErforderlich": false },
  { "zutatenId": 3, "zutatenName": "Mozzarella", "soll": 40, "ist": 7, "nachbestellenErforderlich": true }
]
```

`nachbestellenErforderlich` wird serverseitig berechnet (`true`, wenn `ist < soll`) — im Frontend nicht selbst berechnen, sondern diesen Wert direkt für z.B. eine Warnanzeige nutzen.

**Status Codes**: `200 OK`.

---

### 3.2 Lagerbestand anpassen

`PUT /api/lager/{zutatenId}`

> [!WARNING] `{zutatenId}` ist die ID der **Zutat** (`zutatenId` aus 3.1 / 2.1), **nicht** eine interne Lagerbestand-ID.

**Request Body**

| Feld | Typ | Pflicht |
|---|---|---|
| `soll` | number | ja |
| `ist` | number | ja |

```json
{
  "soll": 40,
  "ist": 35
}
```

**Response**

- `200 OK` – Body enthält den aktualisierten Lagerbestand-Eintrag (gleiches Format wie ein Eintrag aus [3.1](#31-lagerbestand-abfragen)).
- `404 Not Found` – Für die angegebene `zutatenId` existiert kein Lagerbestand-Eintrag.

---

## 4. Tische & Mitarbeiter (Stammdaten)

### 4.1 Tischübersicht

`GET /api/tische`

```json
[
  { "tischId": 1, "plaetze": 4, "istBelegt": true },
  { "tischId": 2, "plaetze": 2, "istBelegt": false }
]
```

**Status Codes**: `200 OK`.

### 4.2 Belegungsstatus eines Tisches setzen

`PUT /api/tische/{id}/status`

Setzt, ob ein Tisch belegt ist. Das Frontend ruft dies auf, wenn an einem Tisch die erste Position bestellt wird (`istBelegt: true`) bzw. wenn eine Bestellung vollständig bezahlt wurde (`istBelegt: false`) — nur so bleibt der Belegungsstatus auch nach einem Neuladen der Seite korrekt (er wird **nicht** automatisch aus offenen Bestellungen abgeleitet).

**Request Body**

| Feld | Typ | Pflicht |
|---|---|---|
| `istBelegt` | boolean | ja |

```json
{
  "istBelegt": true
}
```

**Response**

- `200 OK` – Body enthält den aktualisierten Tisch (gleiches Format wie ein Eintrag aus [4.1](#41-tischübersicht)).
- `404 Not Found` – Für die angegebene `id` existiert kein Tisch.

### 4.3 Mitarbeiter

`GET /api/mitarbeiter`

```json
[
  { "mitarbeiterId": 2, "name": "Max Mustermann", "benutzername": "mmustermann", "rolle": "Kellner" }
]
```

**Status Codes**: `200 OK`.

---

## Fehlerformat

Fehlerantworten (`400`/`404`) mit zusätzlichem Kontext liefern ein einfaches Objekt:

```json
{ "message": "Tisch mit Id 99 wurde nicht gefunden." }
```

`404`-Antworten ohne Zusatzinformation (z.B. Bestellung/Lagerbestand nicht gefunden) haben keinen Body.

Unerwartete Serverfehler (z.B. Datenbank nicht erreichbar) werden global abgefangen und als [RFC 9110 `ProblemDetails`](https://www.rfc-editor.org/rfc/rfc9110#section-15.6.1) mit Statuscode `500` geliefert:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.6.1",
  "title": "Ein unerwarteter Fehler ist aufgetreten.",
  "status": 500,
  "instance": "/api/artikel"
}
```

> [!INFO] Im Development-Modus (`ASPNETCORE_ENVIRONMENT=Development`) enthält die Antwort zusätzlich ein `detail`-Feld mit Exception-Stacktrace. In Production wird dieses Feld weggelassen.

---

## Health Check

`GET /health`

Prüft den Zustand der API inkl. Datenbankverbindung. Gedacht für Monitoring/Docker-Healthchecks, nicht für Frontend-Business-Logik.

```json
{
  "status": "Healthy",
  "checks": [
    { "name": "database", "status": "Healthy", "description": null }
  ],
  "durationMs": 12.4
}
```

**Status Codes**: `200 OK` (alle Checks `Healthy`) / `503 Service Unavailable` (mind. ein Check `Unhealthy`, z.B. Datenbank nicht erreichbar).

---

## Endpunkt-Übersicht

| Methode | Pfad | Zweck |
|---|---|---|
| POST | `/api/bestellungen` | Neue Bestellung aufgeben |
| GET | `/api/bestellungen/{id}` | Bestellung inkl. Positionen & Status-Historie abrufen |
| PATCH | `/api/bestellungen/{id}/status` | Bestellstatus ändern |
| GET | `/api/artikel` | Speisekarte inkl. Zutaten |
| GET | `/api/lager` | Lagerbestand abfragen |
| PUT | `/api/lager/{zutatenId}` | Lagerbestand anpassen |
| GET | `/api/tische` | Tischübersicht |
| PUT | `/api/tische/{id}/status` | Belegungsstatus eines Tisches setzen |
| GET | `/api/mitarbeiter` | Mitarbeiterliste |
| GET | `/health` | Health-Status (API & Datenbank) |
