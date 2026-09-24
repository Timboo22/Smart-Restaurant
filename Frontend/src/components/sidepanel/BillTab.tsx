// Tab "Rechnung": Rechnungsübersicht mit MwSt., Zahlungsart wählen und bezahlen
import { useState } from "react";
import type { Table } from "../../types";
import { fmt, getTotal } from "../../utils";
import QtyBtn from "../buttons/QtyBtn";

interface BillTabProps {
  table: Table;
  onChangeQty: (id: string, delta: number) => void;
  onPay: () => void;
}

export default function BillTab({ table, onChangeQty, onPay }: BillTabProps) {
  const [selectedPayType, setSelectedPayType] = useState<"bar" | "karte" | null>(null);
  const total = getTotal(table.order);
  const tax = total * 0.19;
  const net = total - tax;

  if (table.order.length === 0) {
    return (
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          justifyContent: "center",
          height: "100%",
          color: "var(--c-muted)",
        }}
      >
        <div style={{ fontSize: 32, marginBottom: 8 }}>—</div>
        <div style={{ fontSize: 14 }}>Keine Bestellung vorhanden</div>
      </div>
    );
  }

  return (
    <div style={{ padding: 20 }}>
      <div
        style={{
          background: "var(--c-bill-bg)",
          border: "1px solid var(--c-border)",
          borderRadius: 10,
          padding: 18,
          marginBottom: 14,
        }}
      >
        <div
          style={{
            fontSize: 10,
            fontWeight: 600,
            letterSpacing: "0.07em",
            color: "var(--c-muted)",
            textTransform: "uppercase",
            marginBottom: 14,
          }}
        >
          Rechnung · Tisch {table.number}
        </div>

        <div style={{ display: "flex", flexDirection: "column", gap: 10, marginBottom: 16 }}>
          {table.order.map((item) => (
            <div key={item.id}>
              <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between" }}>
                <div style={{ fontSize: 13, fontWeight: 500, color: "var(--c-text)" }}>{item.name}</div>
                <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
                  <QtyBtn onClick={() => onChangeQty(item.id, -1)} label="−" small />
                  <span style={{ fontFamily: "DM Mono, monospace", fontSize: 13, color: "var(--c-muted)", minWidth: 14, textAlign: "center" }}>
                    {item.quantity}
                  </span>
                  <QtyBtn onClick={() => onChangeQty(item.id, 1)} label="+" small />
                </div>
              </div>
              <div style={{ display: "flex", justifyContent: "space-between", marginTop: 1 }}>
                <span style={{ fontSize: 12, color: "var(--c-muted)" }}>
                  {item.quantity} × {fmt(item.price)}
                </span>
                <span style={{ fontFamily: "DM Mono, monospace", fontSize: 13, color: "var(--c-text-dim)" }}>
                  {fmt(item.price * item.quantity)}
                </span>
              </div>
            </div>
          ))}
        </div>

        <div style={{ borderTop: "1px solid var(--c-border)", paddingTop: 14 }}>
          <div style={{ display: "flex", justifyContent: "space-between", marginBottom: 4 }}>
            <span style={{ fontSize: 12, color: "var(--c-muted)" }}>Netto</span>
            <span style={{ fontFamily: "DM Mono, monospace", fontSize: 12, color: "var(--c-muted)" }}>{fmt(net)}</span>
          </div>
          <div style={{ display: "flex", justifyContent: "space-between", marginBottom: 12 }}>
            <span style={{ fontSize: 12, color: "var(--c-muted)" }}>MwSt. 19%</span>
            <span style={{ fontFamily: "DM Mono, monospace", fontSize: 12, color: "var(--c-muted)" }}>{fmt(tax)}</span>
          </div>
          <div style={{ display: "flex", justifyContent: "space-between" }}>
            <span style={{ fontSize: 16, fontWeight: 700, color: "var(--c-text)" }}>Gesamt</span>
            <span style={{ fontFamily: "DM Mono, monospace", fontSize: 18, fontWeight: 700, color: "var(--c-amber)" }}>
              {fmt(total)}
            </span>
          </div>
        </div>
      </div>

      {/* Payment method */}
      <div style={{ marginBottom: 12 }}>
        <div style={{ fontSize: 10, fontWeight: 600, letterSpacing: "0.07em", color: "var(--c-muted)", textTransform: "uppercase", marginBottom: 8 }}>
          Zahlungsart
        </div>
        <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 8 }}>
          {(["bar", "karte"] as const).map((payType) => {
            const active = selectedPayType === payType;
            return (
              <button
                key={payType}
                onClick={() => setSelectedPayType(payType)}
                style={{
                  padding: "12px 0",
                  borderRadius: 8,
                  border: active ? "1.5px solid var(--c-green)" : "1.5px solid var(--c-border)",
                  background: active ? "color-mix(in srgb, var(--c-green) 10%, transparent)" : "var(--c-surface)",
                  color: active ? "var(--c-green)" : "var(--c-muted)",
                  fontFamily: "'DM Sans', sans-serif",
                  fontWeight: 600,
                  fontSize: 14,
                  cursor: "pointer",
                  transition: "all 0.15s",
                  display: "flex",
                  flexDirection: "column",
                  alignItems: "center",
                  gap: 5,
                }}
              >
                <span style={{ fontSize: 20 }}>{payType === "bar" ? "💵" : "💳"}</span>
                <span>{payType === "bar" ? "Bar" : "Karte"}</span>
              </button>
            );
          })}
        </div>
      </div>

      <button
        onClick={onPay}
        disabled={!selectedPayType}
        style={{
          width: "100%",
          background: selectedPayType ? "var(--c-green)" : "var(--c-surface-2)",
          color: selectedPayType ? "#fff" : "var(--c-muted-2)",
          border: "none",
          borderRadius: 8,
          padding: "13px 0",
          fontFamily: "'DM Sans', sans-serif",
          fontWeight: 700,
          fontSize: 15,
          cursor: selectedPayType ? "pointer" : "not-allowed",
          letterSpacing: "0.01em",
          transition: "all 0.2s",
        }}
      >
        {selectedPayType
          ? `Bezahlt (${selectedPayType === "bar" ? "Bar" : "Karte"}) — Tisch freigeben`
          : "Zahlungsart wählen"}
      </button>

      <div style={{ textAlign: "center", marginTop: 10, fontSize: 11, color: "var(--c-muted-2)" }}>
        Tisch wird nach Zahlung auf "Frei" gesetzt
      </div>
    </div>
  );
}
