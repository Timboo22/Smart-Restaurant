// Popup-Fenster mit Rechnungsübersicht und Bezahl-Button
import type { Table } from "../../types";
import { fmt, getTotal } from "../../utils";

interface BillModalProps {
  table: Table;
  onClose: () => void;
  onPay: () => void;
}

export default function BillModal({ table, onClose, onPay }: BillModalProps) {
  const total = getTotal(table.order);
  return (
    <div
      style={{
        position: "fixed",
        inset: 0,
        background: "rgba(0,0,0,0.5)",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        zIndex: 100,
      }}
      onClick={onClose}
    >
      <div
        style={{
          background: "var(--c-surface)",
          border: "1px solid var(--c-border)",
          borderRadius: 14,
          padding: 28,
          minWidth: 340,
          maxWidth: 420,
        }}
        onClick={(event) => event.stopPropagation()}
      >
        <div style={{ fontSize: 18, fontWeight: 700, marginBottom: 20, color: "var(--c-text)" }}>
          Rechnung · Tisch {table.number}
        </div>
        {table.order.map((item) => (
          <div key={item.id} style={{ display: "flex", justifyContent: "space-between", marginBottom: 8 }}>
            <span style={{ fontSize: 13, color: "var(--c-text)" }}>
              {item.quantity}× {item.name}
            </span>
            <span style={{ fontFamily: "DM Mono, monospace", fontSize: 13, color: "var(--c-text-dim)" }}>
              {fmt(item.price * item.quantity)}
            </span>
          </div>
        ))}
        <div
          style={{ borderTop: "1px solid var(--c-border)", marginTop: 14, paddingTop: 14, display: "flex", justifyContent: "space-between" }}
        >
          <span style={{ fontWeight: 700, fontSize: 16, color: "var(--c-text)" }}>Gesamt</span>
          <span style={{ fontFamily: "DM Mono, monospace", fontWeight: 700, fontSize: 18, color: "var(--c-amber)" }}>
            {fmt(total)}
          </span>
        </div>
        <button
          onClick={onPay}
          style={{
            marginTop: 20,
            width: "100%",
            background: "var(--c-green)",
            color: "#fff",
            border: "none",
            borderRadius: 8,
            padding: "12px 0",
            fontFamily: "'DM Sans', sans-serif",
            fontWeight: 700,
            fontSize: 15,
            cursor: "pointer",
          }}
        >
          Bezahlt — Tisch freigeben
        </button>
      </div>
    </div>
  );
}
