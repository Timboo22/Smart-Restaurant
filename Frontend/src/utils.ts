// Hilfsfunktionen: Preisformatierung und Summenberechnung einer Bestellung
import type { OrderItem } from "./types";

export function fmt(price: number) {
  return price.toFixed(2).replace(".", ",") + " €";
}

export function getTotal(order: OrderItem[]) {
  return order.reduce((sum, orderItem) => sum + orderItem.price * orderItem.quantity, 0);
}
