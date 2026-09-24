// TypeScript-Typen für Tische, Bestellpositionen und Speisekarte
export type TableStatus = "frei" | "besetzt";

export type PanelTab = "bestellung" | "rechnung";

export interface OrderItem {
  id: string;
  name: string;
  price: number;
  quantity: number;
  category: string;
}

export interface Table {
  id: number;
  number: number;
  capacity: number;
  status: TableStatus;
  order: OrderItem[];
}

export interface MenuItem {
  id: string;
  name: string;
  price: number;
  category: string;
}
