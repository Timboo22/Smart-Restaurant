// Seitenleiste des ausgewählten Tisches: Status, Tabs (Bestellung/Rechnung) und Zwischensumme
import type { MenuItem, PanelTab, Table, TableStatus } from "../../types";
import { fmt, getTotal } from "../../utils";
import StatusBtn from "../buttons/StatusBtn";
import OrderTab from "./OrderTab";
import BillTab from "./BillTab";

interface SidePanelProps {
  table: Table;
  tab: PanelTab;
  setTab: (tab: PanelTab) => void;
  category: string;
  setCategory: (category: string) => void;
  menu: MenuItem[];
  categories: string[];
  onClose: () => void;
  onSetStatus: (id: number, status: TableStatus) => void;
  onAdd: (item: MenuItem) => void;
  onChangeQty: (id: string, delta: number) => void;
  onPay: () => void;
  isPaying?: boolean;
  payError?: string | null;
}

export default function SidePanel({
  table,
  tab,
  setTab,
  category,
  setCategory,
  menu,
  categories,
  onClose,
  onSetStatus,
  onAdd,
  onChangeQty,
  onPay,
  isPaying,
  payError,
}: SidePanelProps) {
  return (
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
            Tisch {table.number}
          </div>
          <button
            onClick={onClose}
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
            active={table.status === "frei"}
            color="var(--c-green)"
            label="Frei"
            onClick={() => onSetStatus(table.id, "frei")}
          />
          <StatusBtn
            active={table.status === "besetzt"}
            color="var(--c-amber)"
            label="Besetzt"
            onClick={() => onSetStatus(table.id, "besetzt")}
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
        {(["bestellung", "rechnung"] as const).map((tabName) => (
          <button
            key={tabName}
            onClick={() => setTab(tabName)}
            style={{
              flex: 1,
              padding: "10px 0",
              background: "none",
              border: "none",
              borderBottom: tab === tabName ? "2px solid var(--c-amber)" : "2px solid transparent",
              color: tab === tabName ? "var(--c-tab-active-text)" : "var(--c-muted)",
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
            {tabName === "bestellung" ? "Bestellung" : "Rechnung"}
          </button>
        ))}
      </div>

      {/* Tab Content */}
      <div style={{ flex: 1, overflowY: "auto" }}>
        {tab === "bestellung" ? (
          <OrderTab
            table={table}
            category={category}
            setCategory={setCategory}
            menu={menu}
            categories={categories}
            onAdd={onAdd}
            onChangeQty={onChangeQty}
          />
        ) : (
          <BillTab
            table={table}
            onChangeQty={onChangeQty}
            onPay={onPay}
            isPaying={isPaying}
            payError={payError}
          />
        )}
      </div>

      {/* Footer */}
      {tab === "bestellung" && table.order.length > 0 && (
        <div
          style={{
            padding: "10px 20px",
            borderTop: "1px solid var(--c-border)",
            flexShrink: 0,
          }}
        >
          <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between", marginBottom: 10 }}>
            <span style={{ color: "var(--c-muted)", fontSize: 13 }}>Zwischensumme</span>
            <span style={{ fontWeight: 700, fontSize: 16 }}>{fmt(getTotal(table.order))}</span>
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
  );
}
