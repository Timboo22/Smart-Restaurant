
# API JSON-Formate & Datenstrukturen

> [!INFO] Übersicht
> Nachfolgend findest du **jede** JSON-Datenstruktur, die die Smart-Restaurant-API sendet oder erwartet — Bestellungen, Speisekarte, Lager, Mitarbeiter & Tische, plus alle Fehler- und Health-Formate. Diese Datei ist die reine Datenreferenz; Pfade, HTTP-Methoden und Statuscode-Bedeutungen stehen in [API.md](./API.md).

## 0. Allgemeine Hinweise

- Alle Feldnamen sind **camelCase** (z.B. `tischId`, `artikelName`).
- Alle Request-/Response-Bodies sind `application/json`.
- **`zeitpunkt`-Felder** (`Bestellung.zeitpunkt`, `StatusLog.zeitpunkt`) werden **ohne Zeitzonen-Suffix** serialisiert, z.B. `"2026-09-01T12:30:00"` — **kein** `Z` am Ende, obwohl der Wert intern in UTC erzeugt wird. Grund: Die Spalte ist in Postgres als `timestamp without time zone` angelegt, .NET liefert daher `DateTimeKind.Unspecified`, und `System.Text.Json` hängt bei `Unspecified` keinen `Z`/Offset an. Im Frontend beim Parsen also **davon ausgehen, dass der Wert UTC ist**, auch ohne `Z`-Suffix (nicht ungeprüft als lokale Zeit interpretieren).
- Dezimalwerte (`preis`, `einzelpreis`, `gesamtpreis`, `gesamtbetrag`) sind JSON-Zahlen (`decimal` im Backend), keine Strings.

---

## 1. Bestellungen (Orders)

### 1.1 Neue Bestellung aufgeben (`POST /api/bestellungen`)

> [!NOTE] Request Payload Wird gesendet, wenn an einem Tisch neue Artikel bestellt werden.

```json
{
  "tischId": 4,
  "positionen": [
    {
      "artikelId": 1,
      "menge": 2
    },
    {
      "artikelId": 3,
      "menge": 1
    }
  ]
}
```

### 1.2 Bestellung Details abrufen (`GET /api/bestellungen/{id}`)

> [!NOTE] Response Payload Vollständiges Aggregat inklusive Positionen und Status-Historie. Identisches Format wie die `201`-Response von 1.1 und die `200`-Response von 1.3.

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
    },
    {
      "bestellpositionId": 2,
      "artikelId": 3,
      "artikelName": "Tiramisu",
      "einzelpreis": 9.50,
      "menge": 1,
      "gesamtpreis": 9.50
    }
  ],
  "statusLogs": [
    {
      "logId": 201,
      "mitarbeiterId": 2,
      "mitarbeiterName": "Max Mustermann",
      "status": "Aufgenommen",
      "zeitpunkt": "2026-09-01T12:30:00"
    },
    {
      "logId": 202,
      "mitarbeiterId": 5,
      "mitarbeiterName": "Anna Schmidt",
      "status": "In Zubereitung",
      "zeitpunkt": "2026-09-01T12:35:00"
    }
  ]
}
```

### 1.3 Bestellstatus ändern (`PATCH /api/bestellungen/{id}/status`)

> [!NOTE] Request Payload Aktualisiert den Status der Bestellung und erzeugt einen Eintrag im Status-Log. Response-Format entspricht 1.2.

```json
{
  "mitarbeiterId": 5,
  "neuerStatus": "Servierbereit"
}
```

## 2. Speisekarte & Artikel (Menu Items)

### 2.1 Artikel mit Zutatenliste (`GET /api/artikel`)

```json
[
  {
    "artikelId": 1,
    "name": "Pizza Margherita",
    "preis": 9.50,
    "kategorie": "Hauptspeise",
    "zutaten": [
      {
        "zutatenId": 1,
        "zutatenName": "Teigling",
        "anzahl": 1
      },
      {
        "zutatenId": 2,
        "zutatenName": "Tomatensauce",
        "anzahl": 1
      },
      {
        "zutatenId": 3,
        "zutatenName": "Mozzarella",
        "anzahl": 1
      }
    ]
  },
  {
    "artikelId": 3,
    "name": "Tiramisu",
    "preis": 6.50,
    "kategorie": "Dessert",
    "zutaten": [
      {
        "zutatenId": 8,
        "zutatenName": "Mascarpone",
        "anzahl": 1
      }
    ]
  }
]
```

> [!NOTE] Bei einem Artikel ohne Zutaten ist `zutaten` ein leeres Array `[]`, nie `null`.

## 3. Lager & Zutatenbestand (Inventory)

### 3.1 Lagerbestand abfragen (`GET /api/lager`)

```json
[
  {
    "zutatenId": 1,
    "zutatenName": "Teigling",
    "soll": 100,
    "ist": 85,
    "nachbestellenErforderlich": false
  },
  {
    "zutatenId": 3,
    "zutatenName": "Mozzarella",
    "soll": 40,
    "ist": 7,
    "nachbestellenErforderlich": true
  }
]
```

### 3.2 Lagerbestand anpassen (`PUT /api/lager/{zutatenId}`)

> [!NOTE] Request Payload Response-Format entspricht einem einzelnen Eintrag aus 3.1.

```json
{
  "soll": 40,
  "ist": 35
}
```

## 4. Tische & Mitarbeiter (Master Data)

### 4.1 Tischübersicht (`GET /api/tische`)

```json
[
  {
    "tischId": 1,
    "plaetze": 4,
    "istBelegt": true
  },
  {
    "tischId": 2,
    "plaetze": 2,
    "istBelegt": false
  }
]
```

### 4.2 Belegungsstatus setzen (`PUT /api/tische/{id}/status`)

> [!NOTE] Request Payload `istBelegt` wird **nicht** automatisch aus offenen Bestellungen abgeleitet — das Frontend muss diesen Endpoint selbst aufrufen (bei der ersten Position einer neuen Bestellung mit `true`, nach vollständiger Bezahlung mit `false`). Response-Format entspricht einem einzelnen Eintrag aus 4.1.

```json
{
  "istBelegt": true
}
```

### 4.3 Mitarbeiter (`GET /api/mitarbeiter`)

```json
[
  {
    "mitarbeiterId": 2,
    "name": "Max Mustermann",
    "benutzername": "mmustermann",
    "rolle": "Kellner"
  },
  {
    "mitarbeiterId": 5,
    "name": "Anna Schmidt",
    "benutzername": "aschmidt",
    "rolle": "Kueche"
  }
]
```

## 5. Fehlerantworten

Es gibt **drei** verschiedene Fehler-Formate — je nachdem, welche Art von Fehler auftritt.

### 5.1 Erwartete Fehler mit Kontext (`400` / `404`)

Kommt bei Validierungsfehlern und "nicht gefunden"-Fällen, bei denen die API einen Grund mitgeben kann (z.B. Tisch/Artikel/Mitarbeiter/Lagerbestand nicht gefunden, leere Positionsliste).

```json
{
  "message": "Tisch mit Id 99 wurde nicht gefunden."
}
```

Tritt auf bei:

| Endpoint | Code | Auslöser |
|---|---|---|
| `POST /api/bestellungen` | `400` | `positionen` ist leer |
| `POST /api/bestellungen` | `404` | `tischId` oder eine `artikelId` existiert nicht |
| `PATCH /api/bestellungen/{id}/status` | `404` | Bestellung oder `mitarbeiterId` existiert nicht |
| `PUT /api/lager/{zutatenId}` | `404` | Für `zutatenId` existiert kein Lagerbestand-Eintrag |
| `PUT /api/tische/{id}/status` | `404` | Für `id` existiert kein Tisch |

### 5.2 Einfaches "nicht gefunden" (`404` ohne Body)

`GET /api/bestellungen/{id}` liefert bei unbekannter `id` **keinen Body**, nur den Statuscode `404`.

### 5.3 Unerwartete Serverfehler (`500`)

Alles, was nicht durch die Anwendungslogik abgefangen wird (z.B. Datenbank nicht erreichbar), landet im globalen Exception Handler und kommt als [RFC 9110 `ProblemDetails`](https://www.rfc-editor.org/rfc/rfc9110#section-15.6.1) zurück:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.6.1",
  "title": "Ein unerwarteter Fehler ist aufgetreten.",
  "status": 500,
  "instance": "/api/artikel"
}
```

> [!WARNING] Nur im **Development**-Environment enthält die Antwort zusätzlich ein `detail`-Feld mit vollem Exception-Stacktrace als String. In Production fehlt `detail` komplett — im Frontend also nicht darauf verlassen, dass es existiert.

## 6. Health Check (`GET /health`)

Kein Business-Endpoint, sondern für Monitoring/Docker gedacht. Prüft u.a. die Datenbankverbindung.

```json
{
  "status": "Healthy",
  "checks": [
    {
      "name": "database",
      "status": "Healthy",
      "description": null
    }
  ],
  "durationMs": 12.4
}
```

`status` und `checks[].status` sind einer von `"Healthy"`, `"Degraded"`, `"Unhealthy"`. Bei `"Unhealthy"` antwortet die API mit Statuscode `503` statt `200`, der JSON-Body hat aber dieselbe Struktur.
