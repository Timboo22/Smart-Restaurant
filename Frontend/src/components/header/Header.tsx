// Kopfzeile: Titel, Tisch-Statistik und Theme-Umschalter
import Stat from "./Stat";
import ThemeToggle from "../buttons/ThemeToggle";

interface HeaderProps {
  total: number;
  occupied: number;
  isDark: boolean;
  onToggleTheme: () => void;
}

export default function Header({ total, occupied, isDark, onToggleTheme }: HeaderProps) {
  return (
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
        <Stat label="Gesamt" value={total} />
        <Stat label="Besetzt" value={occupied} accent />
        <Stat label="Frei" value={total - occupied} />
        <ThemeToggle isDark={isDark} onToggle={onToggleTheme} />
      </div>
    </header>
  );
}
