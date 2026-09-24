// Schlanker Client für die Smart-Restaurant-Backend-API.
// Basis-URL ist überschreibbar via VITE_API_BASE_URL (Build-Zeit), Default passt sowohl
// für "dotnet run" (launchSettings.json) als auch für docker-compose (Port-Mapping "5027:8080").
const API_BASE_URL = (import.meta.env.VITE_API_BASE_URL as string | undefined) ?? "http://localhost:5027";

export interface ApiTisch {
  tischId: number;
  plaetze: number;
  istBelegt: boolean;
}

export interface ApiArtikelZutat {
  zutatenId: number;
  zutatenName: string;
  anzahl: number;
}

export interface ApiArtikel {
  artikelId: number;
  name: string;
  preis: number;
  kategorie: string;
  zutaten: ApiArtikelZutat[];
}

export interface ApiMitarbeiter {
  mitarbeiterId: number;
  name: string;
  benutzername: string;
  rolle: string;
}

export interface ApiBestellposition {
  bestellpositionId: number;
  artikelId: number;
  artikelName: string;
  einzelpreis: number;
  menge: number;
  gesamtpreis: number;
}

export interface ApiStatusLog {
  logId: number;
  mitarbeiterId: number;
  mitarbeiterName: string;
  status: string;
  zeitpunkt: string;
}

export interface ApiBestellung {
  bestellungId: number;
  tischId: number;
  status: string;
  zeitpunkt: string;
  gesamtbetrag: number;
  positionen: ApiBestellposition[];
  statusLogs: ApiStatusLog[];
}

export interface NeueBestellungPayload {
  tischId: number;
  positionen: { artikelId: number; menge: number }[];
}

export interface BestellungStatusPayload {
  mitarbeiterId: number;
  neuerStatus: string;
}

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  let response: Response;
  try {
    response = await fetch(`${API_BASE_URL}${path}`, {
      ...init,
      headers: { "Content-Type": "application/json", ...init?.headers },
    });
  } catch {
    throw new Error(`Backend unter ${API_BASE_URL} nicht erreichbar.`);
  }

  if (!response.ok) {
    let message = `${response.status} ${response.statusText}`;
    try {
      const body = await response.json();
      if (typeof body?.message === "string") message = body.message;
      else if (typeof body?.title === "string") message = body.title;
    } catch {
      // Antwort ohne (verwertbaren) JSON-Body, z.B. 404 ohne Body.
    }
    throw new Error(message);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return (await response.json()) as T;
}

export const getTische = () => request<ApiTisch[]>("/api/tische");

export const getArtikel = () => request<ApiArtikel[]>("/api/artikel");

export const getMitarbeiter = () => request<ApiMitarbeiter[]>("/api/mitarbeiter");

export const createBestellung = (payload: NeueBestellungPayload) =>
  request<ApiBestellung>("/api/bestellungen", {
    method: "POST",
    body: JSON.stringify(payload),
  });

export const updateBestellungStatus = (bestellungId: number, payload: BestellungStatusPayload) =>
  request<ApiBestellung>(`/api/bestellungen/${bestellungId}/status`, {
    method: "PATCH",
    body: JSON.stringify(payload),
  });

export const updateTischStatus = (tischId: number, istBelegt: boolean) =>
  request<ApiTisch>(`/api/tische/${tischId}/status`, {
    method: "PUT",
    body: JSON.stringify({ istBelegt }),
  });
