// Button zum Umschalten zwischen hellem und dunklem Design
interface ThemeToggleProps {
  isDark: boolean;
  onToggle: () => void;
}

export default function ThemeToggle({ isDark, onToggle }: ThemeToggleProps) {
  return (
    <button
      onClick={onToggle}
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
  );
}
