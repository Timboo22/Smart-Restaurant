// Tab "Bestellung": Speisekarte nach Kategorie, Artikel hinzufügen und Menge ändern
import type { MenuItem, Table } from "../../types";
import { CATEGORIES, MENU } from "../../data";
import { fmt } from "../../utils";
import QtyBtn from "../buttons/QtyBtn";

interface OrderTabProps {
  table: Table;
  category: string;
  setCategory: (category: string) => void;
  onAdd: (item: MenuItem) => void;
  onChangeQty: (id: string, delta: number) => void;
}

export default function OrderTab({ table, category, setCategory, onAdd, onChangeQty }: OrderTabProps) {
  const menuByCategory = MENU.filter((menuItem) => menuItem.category === category);

  return (
    <div style={{ display: "flex", flexDirection: "column", height: "100%" }}>
      {/* Category pill tabs */}
      <div
        style={{
          padding: "10px 20px",
          borderBottom: "1px solid var(--c-border-soft)",
          flexShrink: 0,
        }}
      >
        <div style={{ display: "flex", gap: 6 }}>
          {CATEGORIES.map((categoryName) => {
            const active = category === categoryName;
            return (
              <button
                key={categoryName}
                onClick={() => setCategory(categoryName)}
                style={{
                  padding: "5px 12px",
                  borderRadius: 20,
                  border: active ? "1.5px solid var(--c-amber)" : "1.5px solid var(--c-border)",
                  background: active ? "var(--c-amber-bg)" : "transparent",
                  color: active ? "var(--c-amber)" : "var(--c-muted)",
                  fontFamily: "'DM Sans', sans-serif",
                  fontSize: 12,
                  fontWeight: 600,
                  cursor: "pointer",
                  transition: "all 0.15s",
                  whiteSpace: "nowrap",
                }}
              >
                {categoryName}
              </button>
            );
          })}
        </div>
      </div>

      {/* Content */}
      <div style={{ flex: 1, overflowY: "auto", padding: "10px 20px 16px" }}>
        <div style={{ display: "flex", flexDirection: "column", gap: 6 }}>
          {menuByCategory.map((item) => {
            const inOrder = table.order.find((orderItem) => orderItem.id === item.id);
            return (
              <div
                key={item.id}
                role="button"
                onClick={() => onAdd(item)}
                style={{
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "space-between",
                  padding: "13px 14px",
                  background: inOrder ? "var(--c-item-ordered-bg)" : "var(--c-surface)",
                  border: inOrder
                    ? "1.5px solid var(--c-amber-border)"
                    : "1.5px solid var(--c-border-soft)",
                  borderRadius: 9,
                  cursor: "pointer",
                  transition: "all 0.12s",
                  textAlign: "left",
                  minHeight: 56,
                }}
              >
                <div style={{ flex: 1, minWidth: 0 }}>
                  <div style={{ fontSize: 14, fontWeight: 500, color: "var(--c-text)", lineHeight: 1.3 }}>
                    {item.name}
                  </div>
                  <div style={{ fontFamily: "DM Mono, monospace", fontSize: 12, color: "var(--c-muted)", marginTop: 3 }}>
                    {fmt(item.price)}
                  </div>
                </div>
                <div
                  onClick={(event) => inOrder && event.stopPropagation()}
                  style={{ display: "flex", alignItems: "center", gap: 8, marginLeft: 12, flexShrink: 0 }}
                >
                  {inOrder ? (
                    <>
                      <QtyBtn onClick={() => onChangeQty(item.id, -1)} label="−" />
                      <span
                        style={{
                          fontFamily: "DM Mono, monospace",
                          fontSize: 14,
                          fontWeight: 500,
                          color: "var(--c-amber)",
                          minWidth: 16,
                          textAlign: "center",
                        }}
                      >
                        {inOrder.quantity}
                      </span>
                      <QtyBtn onClick={() => onChangeQty(item.id, 1)} label="+" />
                    </>
                  ) : (
                    <span
                      style={{
                        width: 28,
                        height: 28,
                        borderRadius: 6,
                        border: "1.5px solid var(--c-border)",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        color: "var(--c-muted)",
                        fontSize: 18,
                        lineHeight: 1,
                      }}
                    >
                      +
                    </span>
                  )}
                </div>
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
}
