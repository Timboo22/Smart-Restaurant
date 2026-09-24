// Karte eines einzelnen Tisches mit Nummer, Status und Bestellsumme
import type { Table } from "../../types";
import { fmt, getTotal } from "../../utils";

interface TableCardProps {
  table: Table;
  selected: boolean;
  onClick: () => void;
}

export default function TableCard({ table, selected, onClick }: TableCardProps) {
  const occupied = table.status === "besetzt";
  return (
    <button
      onClick={onClick}
      style={{
        background: selected ? "var(--c-card-selected-bg)" : "var(--c-surface)",
        border: selected
          ? "1.5px solid var(--c-amber)"
          : "1.5px solid var(--c-border-soft)",
        borderRadius: 10,
        padding: "14px 12px 12px",
        cursor: "pointer",
        textAlign: "left",
        transition: "all 0.15s",
        position: "relative",
        overflow: "hidden",
      }}
    >
      <div
        style={{
          position: "absolute",
          top: 0, left: 0, right: 0,
          height: 3,
          background: occupied ? "var(--c-amber)" : "var(--c-green)",
          borderRadius: "10px 10px 0 0",
        }}
      />
      <div style={{ fontSize: 24, fontWeight: 700, letterSpacing: "-0.04em", color: "var(--c-text)" }}>
        {table.number}
      </div>
      <div style={{ fontSize: 10, color: "var(--c-muted-2)", marginTop: 1, marginBottom: 8 }}>
        Tisch
      </div>
      <div style={{ display: "flex", justifyContent: "flex-end" }}>
        <span
          style={{
            fontSize: 10,
            fontWeight: 600,
            letterSpacing: "0.06em",
            textTransform: "uppercase",
            color: occupied ? "var(--c-amber)" : "var(--c-green)",
          }}
        >
          {table.status}
        </span>
      </div>
      {table.order.length > 0 && (
        <div
          style={{
            marginTop: 6,
            fontSize: 11,
            color: "var(--c-muted)",
            borderTop: "1px solid var(--c-border-soft)",
            paddingTop: 6,
          }}
        >
          {table.order.reduce((sum, orderItem) => sum + orderItem.quantity, 0)} Pos. ·{" "}
          {fmt(getTotal(table.order))}
        </div>
      )}
    </button>
  );
}
