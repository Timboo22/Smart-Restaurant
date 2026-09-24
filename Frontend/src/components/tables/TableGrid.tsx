// Raster, das alle Tische als Karten anzeigt
import type { Table } from "../../types";
import TableCard from "./TableCard";

interface TableGridProps {
  tables: Table[];
  selectedId: number | null;
  onSelect: (id: number) => void;
}

export default function TableGrid({ tables, selectedId, onSelect }: TableGridProps) {
  return (
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
        {tables.map((table) => (
          <TableCard
            key={table.id}
            table={table}
            selected={selectedId === table.id}
            onClick={() => onSelect(table.id)}
          />
        ))}
      </div>
    </main>
  );
}
