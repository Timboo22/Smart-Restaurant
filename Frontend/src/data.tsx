// Datenquelle ("DB"): Speisekarte, Kategorien und Anfangszustand der Tische
import type { MenuItem, Table } from "./types";

export const MENU: MenuItem[] = [
  { id: "v1", name: "Hausgemachte Suppe", price: 5.9, category: "Essen" },
  { id: "v2", name: "Gemischter Salat", price: 7.5, category: "Essen" },
  { id: "v3", name: "Bruschetta", price: 6.8, category: "Essen" },
  { id: "v4", name: "Lachs Carpaccio", price: 11.5, category: "Essen" },
  { id: "h1", name: "Wiener Schnitzel", price: 18.9, category: "Essen" },
  { id: "h2", name: "Spaghetti Bolognese", price: 13.5, category: "Essen" },
  { id: "h3", name: "Gegrillter Lachs", price: 22.0, category: "Essen" },
  { id: "h4", name: "Rumpsteak 200g", price: 28.5, category: "Essen" },
  { id: "h5", name: "Veganer Burger", price: 15.9, category: "Essen" },
  { id: "h6", name: "Risotto mit Pilzen", price: 16.5, category: "Essen" },
  { id: "d1", name: "Crème Brûlée", price: 7.5, category: "Essen" },
  { id: "d2", name: "Schokoladenkuchen", price: 6.9, category: "Essen" },
  { id: "d3", name: "Gemischtes Eis", price: 5.5, category: "Essen" },
  { id: "d4", name: "Apfelstrudel", price: 6.5, category: "Essen" },
  { id: "g1", name: "Mineralwasser 0,5l", price: 3.5, category: "Trinken" },
  { id: "g2", name: "Weizenbier 0,5l", price: 4.2, category: "Trinken" },
  { id: "g3", name: "Hauswein 0,25l", price: 5.8, category: "Trinken" },
  { id: "g4", name: "Espresso", price: 2.8, category: "Trinken" },
  { id: "g5", name: "Orangensaft 0,3l", price: 3.9, category: "Trinken" },
  { id: "g6", name: "Cappuccino", price: 3.5, category: "Trinken" },
];

export const CATEGORIES = ["Essen", "Trinken"];

export const INITIAL_TABLES: Table[] = [
  { id: 1, number: 1, capacity: 2, status: "frei", order: [] },
  { id: 2, number: 2, capacity: 2, status: "frei", order: [] },
  { id: 3, number: 3, capacity: 4, status: "frei", order: [] },
  { id: 4, number: 4, capacity: 4, status: "frei", order: [] },
  { id: 5, number: 5, capacity: 4, status: "frei", order: [] },
  { id: 6, number: 6, capacity: 6, status: "frei", order: [] },
  { id: 7, number: 7, capacity: 6, status: "frei", order: [] },
  { id: 8, number: 8, capacity: 4, status: "frei", order: [] },
  { id: 9, number: 9, capacity: 2, status: "frei", order: [] },
  { id: 10, number: 10, capacity: 4, status: "frei", order: [] },
  { id: 11, number: 11, capacity: 6, status: "frei", order: [] },
  { id: 12, number: 12, capacity: 8, status: "frei", order: [] },
];
