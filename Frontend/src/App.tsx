import { useState } from "react";

type TableStatus = "frei" | "besetzt";

interface OrderItem {
  id: string;
  name: string;
  price: number;
  quantity: number;
  category: string;
}

interface Table {
  id: number;
  number: number;
  capacity: number;
  status: TableStatus;
  order: OrderItem[];
}

interface MenuItem {
  id: string;
  name: string;
  price: number;
  category: string;
}

const MENU: MenuItem[] = [
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

const CATEGORIES = ["Essen", "Trinken"];

const INITIAL_TABLES: Table[] = [
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

function fmt(price: number) {
  return price.toFixed(2).replace(".", ",") + " €";
}

function getTotal(order: OrderItem[]) {
  return order.reduce((s, i) => s + i.price * i.quantity, 0);
}

export default function App() {
  const [tables, setTables] = useState<Table[]>(INITIAL_TABLES);
  const [selectedId, setSelectedId] = useState<number | null>(null);
  const [tab, setTab] = useState<"bestellung" | "rechnung">("bestellung");
  const [category, setCategory] = useState("Essen");
  const [showBill, setShowBill] = useState(false);
  const [isDark, setIsDark] = useState(false);

  const selected = tables.find((t) => t.id === selectedId) ?? null;

  const update = (id: number, patch: Partial<Table>) =>
    setTables((prev) => prev.map((t) => (t.id === id ? { ...t, ...patch } : t)));

  const setStatus = (id: number, status: TableStatus) => {
    if (status === "frei") {
      update(id, { status: "frei", order: [] });
    } else {
      update(id, { status: "besetzt" });
    }
  };

  const addItem = (item: MenuItem) => {
    if (!selectedId) return;
    const t = tables.find((x) => x.id === selectedId)!;
    const ex = t.order.find((o) => o.id === item.id);
    const newOrder = ex
      ? t.order.map((o) => (o.id === item.id ? { ...o, quantity: o.quantity + 1 } : o))
      : [...t.order, { ...item, quantity: 1 }];
    update(selectedId, { order: newOrder, status: "besetzt" });
  };

  const changeQty = (itemId: string, delta: number) => {
    if (!selectedId) return;
    const t = tables.find((x) => x.id === selectedId)!;
    const newOrder = t.order
      .map((o) => (o.id === itemId ? { ...o, quantity: o.quantity + delta } : o))
      .filter((o) => o.quantity > 0);
    update(selectedId, { order: newOrder });
  };

  const handlePay = () => {
    if (!selectedId) return;
    update(selectedId, { status: "frei", order: [] });
    setShowBill(false);
    setSelectedId(null);
  };

  const occupied = tables.filter((t) => t.status === "besetzt").length;
  const menuByCategory = MENU.filter((m) => m.category === category);

  return (
    <div
      className={isDark ? "dark" : ""}
      style={{
        background: "var(--c-bg)",
        color: "var(--c-text)",
        fontFamily: "'DM Sans', sans-serif",
        display: "flex",
        flexDirection: "column",
        height: "100%",
        overflow: "hidden",
      }}
    >
      {/* Header */}
      <header
        style={{
          borderBottom: "1px solid var(--c-border)",
          background: "var(--c-bg)",
          display: "flex",
          alignItems: "center",
          justifyContent: "space-between",
          padding: "0 24px",
          height: 44,
          flexShrink: 0,
        }}
      >
        <div style={{ display: "flex", alignItems: "center", gap: 10 }}>
          <span
            style={{
              background: "var(--c-amber-btn)",
              color: "#fff",
              fontWeight: 700,
              fontSize: 10,
              letterSpacing: "0.07em",
              padding: "2px 7px",
              borderRadius: 3,
            }}
          >
            SERVICE
          </span>
          <span style={{ fontSize: 14, fontWeight: 600, letterSpacing: "-0.02em" }}>
            Tischverwaltung
          </span>
        </div>

        <div style={{ display: "flex", alignItems: "center", gap: 24 }}>
          <Stat label="Gesamt" value={tables.length} />
          <Stat label="Besetzt" value={occupied} accent />
          <Stat label="Frei" value={tables.length - occupied} />

          {/* Theme toggle */}
          <button
            onClick={() => setIsDark((d) => !d)}
            title={isDark ? "Helles Design" : "Dunkles Design"}
            style={{
              width: 30,
              height: 30,
              borderRadius: 6,
              border: "1px solid var(--c-border)",
              background: "var(--c-surface)",
              color: "var(--c-muted)",
              cursor: "pointer",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              fontSize: 14,
              transition: "all 0.15s",
            }}
          >
            {isDark ? "☀" : "◑"}
          </button>
        </div>
      </header>

      {/* Body */}
      <div style={{ display: "flex", flex: 1, overflow: "hidden" }}>
        {/* Table Grid */}
        <main style={{ flex: 1, overflowY: "auto", padding: 24 }}>
          <div
            style={{
              display: "grid",
              gridTemplateColumns: selectedId
                ? "repeat(3, 1fr)"
                : "repeat(auto-fill, minmax(160px, 1fr))",
              gap: 10,
            }}
          >
            {tables.map((t) => (
              <TableCard
                key={t.id}
                table={t}
                selected={selectedId === t.id}
                onClick={() => {
                  setSelectedId(t.id);
                  setTab("bestellung");
                  setShowBill(false);
                }}
              />
            ))}
          </div>
        </main>

        {/* Side Panel */}
        {selected && (
          <aside
            style={{
              width: 420,
              borderLeft: "1px solid var(--c-border)",
              background: "var(--c-panel)",
              display: "flex",
              flexDirection: "column",
            }}
          >
            {/* Panel Header */}
            <div
              style={{
                borderBottom: "1px solid var(--c-border)",
                padding: "12px 20px 10px",
                flexShrink: 0,
              }}
            >
              <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between", marginBottom: 10 }}>
                <div style={{ fontSize: 18, fontWeight: 700, letterSpacing: "-0.03em" }}>
                  Tisch {selected.number}
                </div>
                <button
                  onClick={() => setSelectedId(null)}
                  style={{
                    background: "none",
                    border: "none",
                    color: "var(--c-muted)",
                    fontSize: 20,
                    cursor: "pointer",
                    padding: "2px 6px",
                    lineHeight: 1,
                  }}
                  aria-label="Schließen"
                >
                  ×
                </button>
              </div>
              <div style={{ display: "flex", gap: 8 }}>
                <StatusBtn
                  active={selected.status === "frei"}
                  color="var(--c-green)"
                  label="Frei"
                  onClick={() => setStatus(selected.id, "frei")}
                />
                <StatusBtn
                  active={selected.status === "besetzt"}
                  color="var(--c-amber)"
                  label="Besetzt"
                  onClick={() => setStatus(selected.id, "besetzt")}
                />
              </div>
            </div>

            {/* Tabs: Bestellung / Rechnung */}
            <div
              style={{
                borderBottom: "1px solid var(--c-border)",
                display: "flex",
                flexShrink: 0,
              }}
            >
              {(["bestellung", "rechnung"] as const).map((t) => (
                <button
                  key={t}
                  onClick={() => setTab(t)}
                  style={{
                    flex: 1,
                    padding: "10px 0",
                    background: "none",
                    border: "none",
                    borderBottom: tab === t ? "2px solid var(--c-amber)" : "2px solid transparent",
                    color: tab === t ? "var(--c-tab-active-text)" : "var(--c-muted)",
                    fontFamily: "'DM Sans', sans-serif",
                    fontSize: 12,
                    fontWeight: 600,
                    cursor: "pointer",
                    letterSpacing: "0.05em",
                    textTransform: "uppercase",
                    transition: "color 0.15s",
                    marginBottom: -1,
                  }}
                >
                  {t === "bestellung" ? "Bestellung" : "Rechnung"}
                </button>
              ))}
            </div>

            {/* Tab Content */}
            <div style={{ flex: 1, overflowY: "auto" }}>
              {tab === "bestellung" ? (
                <OrderTab
                  table={selected}
                  category={category}
                  setCategory={setCategory}
                  menuByCategory={menuByCategory}
                  onAdd={addItem}
                  onChangeQty={changeQty}
                />
              ) : (
                <BillTab table={selected} onChangeQty={changeQty} onPay={handlePay} />
              )}
            </div>

            {/* Footer */}
            {tab === "bestellung" && selected.order.length > 0 && (
              <div
                style={{
                  padding: "10px 20px",
                  borderTop: "1px solid var(--c-border)",
                  flexShrink: 0,
                }}
              >
                <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between", marginBottom: 10 }}>
                  <span style={{ color: "var(--c-muted)", fontSize: 13 }}>Zwischensumme</span>
                  <span style={{ fontWeight: 700, fontSize: 16 }}>{fmt(getTotal(selected.order))}</span>
                </div>
                <button
                  onClick={() => setTab("rechnung")}
                  style={{
                    width: "100%",
                    background: "var(--c-amber-btn)",
                    color: "#fff",
                    border: "none",
                    borderRadius: 6,
                    padding: "11px 0",
                    fontFamily: "'DM Sans', sans-serif",
                    fontWeight: 700,
                    fontSize: 14,
                    cursor: "pointer",
                    letterSpacing: "0.02em",
                  }}
                >
                  Zur Rechnung →
                </button>
              </div>
            )}
          </aside>
        )}
      </div>

      {showBill && selected && (
        <BillModal table={selected} onClose={() => setShowBill(false)} onPay={handlePay} />
      )}
    </div>
  );
}

/* ---- Sub-components ---- */

function Stat({ label, value, accent }: { label: string; value: number; accent?: boolean }) {
  return (
    <div style={{ textAlign: "right" }}>
      <div
        style={{
          fontSize: 16,
          fontWeight: 700,
          letterSpacing: "-0.03em",
          color: accent ? "var(--c-amber-stat)" : "var(--c-text)",
        }}
      >
        {value}
      </div>
      <div style={{ fontSize: 10, color: "var(--c-muted-2)", letterSpacing: "0.04em", textTransform: "uppercase" }}>
        {label}
      </div>
    </div>
  );
}

function TableCard({ table, selected, onClick }: { table: Table; selected: boolean; onClick: () => void }) {
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
          {table.order.reduce((s, i) => s + i.quantity, 0)} Pos. ·{" "}
          {fmt(table.order.reduce((s, i) => s + i.price * i.quantity, 0))}
        </div>
      )}
    </button>
  );
}

function StatusBtn({ active, color, label, onClick }: { active: boolean; color: string; label: string; onClick: () => void }) {
  return (
    <button
      onClick={onClick}
      style={{
        flex: 1,
        padding: "7px 0",
        borderRadius: 6,
        border: active ? `1.5px solid ${color}` : "1.5px solid var(--c-border)",
        background: active ? `color-mix(in srgb, ${color} 12%, transparent)` : "transparent",
        color: active ? color : "var(--c-muted)",
        fontFamily: "'DM Sans', sans-serif",
        fontWeight: 600,
        fontSize: 13,
        cursor: "pointer",
        transition: "all 0.15s",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        gap: 6,
      }}
    >
      <span
        style={{
          width: 7,
          height: 7,
          borderRadius: "50%",
          background: active ? color : "var(--c-dot-inactive)",
          display: "inline-block",
          transition: "background 0.15s",
        }}
      />
      {label}
    </button>
  );
}

function OrderTab({
  table,
  category,
  setCategory,
  menuByCategory,
  onAdd,
  onChangeQty,
}: {
  table: Table;
  category: string;
  setCategory: (c: string) => void;
  menuByCategory: MenuItem[];
  onAdd: (item: MenuItem) => void;
  onChangeQty: (id: string, delta: number) => void;
}) {
  const orderCount = table.order.reduce((s, i) => s + i.quantity, 0);
  const allTabs = [
    { id: "Bestellt", label: orderCount > 0 ? `Bestellt (${orderCount})` : "Bestellt" },
    ...CATEGORIES.map((c) => ({ id: c, label: c })),
  ];

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
          {allTabs.map((t) => {
            const active = category === t.id;
            const isOrder = t.id === "Bestellt";
            const activeColor = isOrder ? "var(--c-purple)" : "var(--c-amber)";
            const activeBg = isOrder ? "var(--c-purple-bg)" : "var(--c-amber-bg)";
            const activeBorder = isOrder ? "var(--c-purple)" : "var(--c-amber)";
            return (
              <button
                key={t.id}
                onClick={() => setCategory(t.id)}
                style={{
                  padding: "5px 12px",
                  borderRadius: 20,
                  border: active ? `1.5px solid ${activeBorder}` : "1.5px solid var(--c-border)",
                  background: active ? activeBg : "transparent",
                  color: active ? activeColor : "var(--c-muted)",
                  fontFamily: "'DM Sans', sans-serif",
                  fontSize: 12,
                  fontWeight: 600,
                  cursor: "pointer",
                  transition: "all 0.15s",
                  whiteSpace: "nowrap",
                }}
              >
                {t.label}
              </button>
            );
          })}
        </div>
      </div>

      {/* Content */}
      <div style={{ flex: 1, overflowY: "auto", padding: "10px 20px 16px" }}>
        {category === "Bestellt" ? (
          table.order.length === 0 ? (
            <div
              style={{
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                height: "100%",
                color: "var(--c-muted-2)",
                fontSize: 13,
              }}
            >
              Noch keine Positionen
            </div>
          ) : (
            <div style={{ display: "flex", flexDirection: "column", gap: 6 }}>
              {table.order.map((item) => (
                <div
                  key={item.id}
                  style={{
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "space-between",
                    padding: "11px 14px",
                    background: "var(--c-surface)",
                    border: "1.5px solid var(--c-border-soft)",
                    borderRadius: 9,
                    minHeight: 56,
                  }}
                >
                  <div style={{ flex: 1, minWidth: 0 }}>
                    <div
                      style={{
                        fontSize: 14,
                        fontWeight: 500,
                        color: "var(--c-text)",
                        lineHeight: 1.3,
                        whiteSpace: "nowrap",
                        overflow: "hidden",
                        textOverflow: "ellipsis",
                      }}
                    >
                      {item.name}
                    </div>
                    <div style={{ fontFamily: "DM Mono, monospace", fontSize: 12, color: "var(--c-muted)", marginTop: 2 }}>
                      {fmt(item.price * item.quantity)}
                    </div>
                  </div>
                  <div style={{ display: "flex", alignItems: "center", gap: 8, marginLeft: 12, flexShrink: 0 }}>
                    <QtyBtn onClick={() => onChangeQty(item.id, -1)} label="−" />
                    <span style={{ fontFamily: "DM Mono, monospace", fontSize: 14, color: "var(--c-text)", minWidth: 16, textAlign: "center" }}>
                      {item.quantity}
                    </span>
                    <QtyBtn onClick={() => onChangeQty(item.id, 1)} label="+" />
                  </div>
                </div>
              ))}
            </div>
          )
        ) : (
          <div style={{ display: "flex", flexDirection: "column", gap: 6 }}>
            {menuByCategory.map((item) => {
              const inOrder = table.order.find((o) => o.id === item.id);
              return (
                <button
                  key={item.id}
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
                  <div style={{ display: "flex", alignItems: "center", gap: 8, marginLeft: 12, flexShrink: 0 }}>
                    {inOrder ? (
                      <span
                        style={{
                          fontFamily: "DM Mono, monospace",
                          fontSize: 13,
                          fontWeight: 500,
                          color: "var(--c-amber)",
                          background: "var(--c-amber-bg)",
                          border: "1px solid var(--c-amber-border)",
                          padding: "3px 10px",
                          borderRadius: 5,
                        }}
                      >
                        ×{inOrder.quantity}
                      </span>
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
                </button>
              );
            })}
          </div>
        )}
      </div>
    </div>
  );
}

function BillTab({ table, onChangeQty, onPay }: { table: Table; onChangeQty: (id: string, delta: number) => void; onPay: () => void }) {
  const [payMethod, setPayMethod] = useState<"bar" | "karte" | null>(null);
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
          {(["bar", "karte"] as const).map((m) => {
            const active = payMethod === m;
            return (
              <button
                key={m}
                onClick={() => setPayMethod(m)}
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
                <span style={{ fontSize: 20 }}>{m === "bar" ? "💵" : "💳"}</span>
                <span>{m === "bar" ? "Bar" : "Karte"}</span>
              </button>
            );
          })}
        </div>
      </div>

      <button
        onClick={onPay}
        disabled={!payMethod}
        style={{
          width: "100%",
          background: payMethod ? "var(--c-green)" : "var(--c-surface-2)",
          color: payMethod ? "#fff" : "var(--c-muted-2)",
          border: "none",
          borderRadius: 8,
          padding: "13px 0",
          fontFamily: "'DM Sans', sans-serif",
          fontWeight: 700,
          fontSize: 15,
          cursor: payMethod ? "pointer" : "not-allowed",
          letterSpacing: "0.01em",
          transition: "all 0.2s",
        }}
      >
        {payMethod
          ? `Bezahlt (${payMethod === "bar" ? "Bar" : "Karte"}) — Tisch freigeben`
          : "Zahlungsart wählen"}
      </button>

      <div style={{ textAlign: "center", marginTop: 10, fontSize: 11, color: "var(--c-muted-2)" }}>
        Tisch wird nach Zahlung auf "Frei" gesetzt
      </div>
    </div>
  );
}

function QtyBtn({ onClick, label, small }: { onClick: () => void; label: string; small?: boolean }) {
  return (
    <button
      onClick={onClick}
      style={{
        width: small ? 22 : 24,
        height: small ? 22 : 24,
        borderRadius: 5,
        border: "1px solid var(--c-border)",
        background: "var(--c-qty-bg)",
        color: "var(--c-muted)",
        fontFamily: "'DM Sans', sans-serif",
        fontSize: 14,
        fontWeight: 600,
        cursor: "pointer",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        lineHeight: 1,
        padding: 0,
        transition: "all 0.12s",
      }}
    >
      {label}
    </button>
  );
}

function BillModal({ table, onClose, onPay }: { table: Table; onClose: () => void; onPay: () => void }) {
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
        onClick={(e) => e.stopPropagation()}
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
