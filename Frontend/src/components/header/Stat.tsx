// Einzelne Kennzahl in der Kopfzeile (z. B. Anzahl besetzter Tische)
interface StatProps {
  label: string;
  value: number;
  accent?: boolean;
}

export default function Stat({ label, value, accent }: StatProps) {
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
