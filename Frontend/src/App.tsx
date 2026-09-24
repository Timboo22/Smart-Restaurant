// Hauptkomponente: hält den gesamten State (Tische, Auswahl, Theme) und verbindet alle Komponenten
import { useState } from "react";
import type { MenuItem, PanelTab, Table, TableStatus } from "./types";
import { INITIAL_TABLES } from "./data";
import Header from "./components/header/Header";
import TableGrid from "./components/tables/TableGrid";
import SidePanel from "./components/sidepanel/SidePanel";
import BillModal from "./components/modal/BillModal";

export default function App() {
  const [tables, setTables] = useState<Table[]>(INITIAL_TABLES);
  const [selectedId, setSelectedId] = useState<number | null>(null);
  const [tab, setTab] = useState<PanelTab>("bestellung");
  const [category, setCategory] = useState("Essen");
  const [showBill, setShowBill] = useState(false);
  const [isDark, setIsDark] = useState(false);

  const selected = tables.find((table) => table.id === selectedId) ?? null;

  const update = (id: number, patch: Partial<Table>) =>
    setTables((prevTables) =>
      prevTables.map((table) => (table.id === id ? { ...table, ...patch } : table))
    );

  const setStatus = (id: number, status: TableStatus) => {
    if (status === "frei") {
      update(id, { status: "frei", order: [] });
    } else {
      update(id, { status: "besetzt" });
    }
  };

  const addItem = (item: MenuItem) => {
    if (!selectedId) return;
    const selectedTable = tables.find((table) => table.id === selectedId)!;
    const existingItem = selectedTable.order.find((orderItem) => orderItem.id === item.id);
    const newOrder = existingItem
      ? selectedTable.order.map((orderItem) =>
          orderItem.id === item.id ? { ...orderItem, quantity: orderItem.quantity + 1 } : orderItem
        )
      : [...selectedTable.order, { ...item, quantity: 1 }];
    update(selectedId, { order: newOrder, status: "besetzt" });
  };

  const changeQty = (itemId: string, delta: number) => {
    if (!selectedId) return;
    const selectedTable = tables.find((table) => table.id === selectedId)!;
    const newOrder = selectedTable.order
      .map((orderItem) =>
        orderItem.id === itemId ? { ...orderItem, quantity: orderItem.quantity + delta } : orderItem
      )
      .filter((orderItem) => orderItem.quantity > 0);
    update(selectedId, { order: newOrder });
  };

  const handlePay = () => {
    if (!selectedId) return;
    update(selectedId, { status: "frei", order: [] });
    setShowBill(false);
    setSelectedId(null);
  };

  const selectTable = (id: number) => {
    setSelectedId(id);
    setTab("bestellung");
    setShowBill(false);
  };

  const occupied = tables.filter((table) => table.status === "besetzt").length;

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
      <Header
        total={tables.length}
        occupied={occupied}
        isDark={isDark}
        onToggleTheme={() => setIsDark((prevIsDark) => !prevIsDark)}
      />

      <div style={{ display: "flex", flex: 1, overflow: "hidden" }}>
        <TableGrid tables={tables} selectedId={selectedId} onSelect={selectTable} />

        {selected && (
          <SidePanel
            table={selected}
            tab={tab}
            setTab={setTab}
            category={category}
            setCategory={setCategory}
            onClose={() => setSelectedId(null)}
            onSetStatus={setStatus}
            onAdd={addItem}
            onChangeQty={changeQty}
            onPay={handlePay}
          />
        )}
      </div>

      {showBill && selected && (
        <BillModal table={selected} onClose={() => setShowBill(false)} onPay={handlePay} />
      )}
    </div>
  );
}
