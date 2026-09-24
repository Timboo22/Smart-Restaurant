// Hauptkomponente: hält den gesamten State (Tische, Auswahl, Theme) und verbindet alle Komponenten
import { useEffect, useState } from "react";
import type { MenuItem, PanelTab, Table, TableStatus } from "./types";
import {
  createBestellung,
  getArtikel,
  getMitarbeiter,
  getTische,
  updateBestellungStatus,
  updateTischStatus,
} from "./api";
import type { ApiMitarbeiter } from "./api";
import Header from "./components/header/Header";
import TableGrid from "./components/tables/TableGrid";
import SidePanel from "./components/sidepanel/SidePanel";
import BillModal from "./components/modal/BillModal";

export default function App() {
  const [tables, setTables] = useState<Table[]>([]);
  const [menu, setMenu] = useState<MenuItem[]>([]);
  const [categories, setCategories] = useState<string[]>([]);
  const [mitarbeiter, setMitarbeiter] = useState<ApiMitarbeiter[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [loadError, setLoadError] = useState<string | null>(null);

  const [selectedId, setSelectedId] = useState<number | null>(null);
  const [tab, setTab] = useState<PanelTab>("bestellung");
  const [category, setCategory] = useState("");
  const [showBill, setShowBill] = useState(false);
  const [isDark, setIsDark] = useState(false);
  const [isPaying, setIsPaying] = useState(false);
  const [payError, setPayError] = useState<string | null>(null);

  const loadData = async () => {
    setIsLoading(true);
    setLoadError(null);
    try {
      const [tischeResponse, artikelResponse, mitarbeiterResponse] = await Promise.all([
        getTische(),
        getArtikel(),
        getMitarbeiter(),
      ]);

      setTables(
        tischeResponse.map((tisch) => ({
          id: tisch.tischId,
          number: tisch.tischId,
          capacity: tisch.plaetze,
          status: tisch.istBelegt ? "besetzt" : "frei",
          order: [],
        }))
      );

      const menuItems: MenuItem[] = artikelResponse.map((artikel) => ({
        id: String(artikel.artikelId),
        name: artikel.name,
        price: artikel.preis,
        category: artikel.kategorie,
      }));
      setMenu(menuItems);

      const menuCategories = Array.from(new Set(menuItems.map((item) => item.category)));
      setCategories(menuCategories);
      setCategory((current) => (current && menuCategories.includes(current) ? current : menuCategories[0] ?? ""));

      setMitarbeiter(mitarbeiterResponse);
    } catch (error) {
      setLoadError(error instanceof Error ? error.message : "Unbekannter Fehler beim Laden der Daten.");
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  const selected = tables.find((table) => table.id === selectedId) ?? null;

  const update = (id: number, patch: Partial<Table>) =>
    setTables((prevTables) =>
      prevTables.map((table) => (table.id === id ? { ...table, ...patch } : table))
    );

  // Persistiert den Belegungsstatus im Backend (best effort — UI wurde bereits optimistisch aktualisiert).
  const syncTischStatus = (id: number, istBelegt: boolean) => {
    updateTischStatus(id, istBelegt).catch((error) => {
      console.error("Tisch-Status konnte nicht gespeichert werden:", error);
    });
  };

  const setStatus = (id: number, status: TableStatus) => {
    if (status === "frei") {
      update(id, { status: "frei", order: [] });
    } else {
      update(id, { status: "besetzt" });
    }
    syncTischStatus(id, status === "besetzt");
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
    if (selectedTable.status !== "besetzt") {
      syncTischStatus(selectedId, true);
    }
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

  // Legt die Bestellung im Backend an und markiert sie sofort als "Bezahlt"
  // (die App hat keinen eigenen "Bezahlen"-Endpoint, nur den generischen Status-Wechsel).
  const handlePay = async () => {
    if (!selectedId) return;
    const table = tables.find((t) => t.id === selectedId);
    if (!table || table.order.length === 0) return;

    if (mitarbeiter.length === 0) {
      setPayError("Keine Mitarbeiter im Backend gefunden — Zahlung kann nicht abgeschlossen werden.");
      return;
    }

    setIsPaying(true);
    setPayError(null);
    try {
      const bestellung = await createBestellung({
        tischId: table.id,
        positionen: table.order.map((item) => ({ artikelId: Number(item.id), menge: item.quantity })),
      });

      await updateBestellungStatus(bestellung.bestellungId, {
        mitarbeiterId: mitarbeiter[0].mitarbeiterId,
        neuerStatus: "Bezahlt",
      });

      syncTischStatus(selectedId, false);
      update(selectedId, { status: "frei", order: [] });
      setShowBill(false);
      setSelectedId(null);
    } catch (error) {
      setPayError(error instanceof Error ? error.message : "Unbekannter Fehler beim Bezahlen.");
    } finally {
      setIsPaying(false);
    }
  };

  const selectTable = (id: number) => {
    setSelectedId(id);
    setTab("bestellung");
    setShowBill(false);
    setPayError(null);
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
        {loadError ? (
          <div
            style={{
              flex: 1,
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
              justifyContent: "center",
              gap: 12,
              color: "var(--c-muted)",
              padding: 24,
              textAlign: "center",
            }}
          >
            <div style={{ fontSize: 14 }}>Backend nicht erreichbar: {loadError}</div>
            <button
              onClick={loadData}
              style={{
                padding: "8px 16px",
                borderRadius: 6,
                border: "1px solid var(--c-border)",
                background: "var(--c-surface)",
                color: "var(--c-text)",
                cursor: "pointer",
                fontFamily: "'DM Sans', sans-serif",
                fontWeight: 600,
                fontSize: 13,
              }}
            >
              Erneut versuchen
            </button>
          </div>
        ) : isLoading ? (
          <div style={{ flex: 1, display: "flex", alignItems: "center", justifyContent: "center", color: "var(--c-muted)" }}>
            Lädt…
          </div>
        ) : (
          <TableGrid tables={tables} selectedId={selectedId} onSelect={selectTable} />
        )}

        {selected && (
          <SidePanel
            table={selected}
            tab={tab}
            setTab={setTab}
            category={category}
            setCategory={setCategory}
            menu={menu}
            categories={categories}
            onClose={() => setSelectedId(null)}
            onSetStatus={setStatus}
            onAdd={addItem}
            onChangeQty={changeQty}
            onPay={handlePay}
            isPaying={isPaying}
            payError={payError}
          />
        )}
      </div>

      {showBill && selected && (
        <BillModal table={selected} onClose={() => setShowBill(false)} onPay={handlePay} />
      )}
    </div>
  );
}
