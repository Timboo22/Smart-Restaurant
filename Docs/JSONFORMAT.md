
# API JSON-Formate & Datenstrukturen

> [!INFO] Übersicht Nachfolgend findest du die reinen JSON-Datenstrukturen für alle Endpunkte des **Smart Restaurant Systems** (Bestellungen, Speisekarte, Lager, Mitarbeiter & Tische).

## 1. Bestellungen (Orders)

### 1.1 Neue Bestellung aufgeben (`POST /api/bestellungen`)

> [!NOTE] Request Payload Wird gesendet, wenn an einem Tisch neue Artikel bestellt werden.

JSON

```
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

> [!NOTE] Response Payload Vollständiges Aggregat inklusive Positionen und Status-Historie.

JSON

```
{
  "bestellungId": 101,
  "tischId": 4,
  "status": "In Zubereitung",
  "zeitpunkt": "2026-09-01T12:30:00Z",
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
      "zeitpunkt": "2026-09-01T12:30:00Z"
    },
    {
      "logId": 202,
      "mitarbeiterId": 5,
      "mitarbeiterName": "Anna Schmidt",
      "status": "In Zubereitung",
      "zeitpunkt": "2026-09-01T12:35:00Z"
    }
  ]
}
```

### 1.3 Bestellstatus ändern (`PATCH /api/bestellungen/{id}/status`)

> [!NOTE] Request Payload Aktualisiert den Status der Bestellung und erzeugt einen Eintrag im Status-Log.

JSON

```
{
  "mitarbeiterId": 5,
  "neuerStatus": "Servierbereit"
}
```

## 2. Speisekarte & Artikel (Menu Items)

### 2.1 Artikel mit Zutatenliste (`GET /api/artikel`)

JSON

```
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

## 3.  Lager & Zutatenbestand (Inventory)

### 3.1 Lagerbestand abfragen (`GET /api/lager`)

JSON

```
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

JSON

```
{
  "soll": 40,
  "ist": 35
}
```

## 4.  Tische & Mitarbeiter (Master Data)

### 4.1 Tischübersicht (`GET /api/tische`)

JSON

```
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

### 4.2 Mitarbeiter (`GET /api/mitarbeiter`)

JSON

```
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