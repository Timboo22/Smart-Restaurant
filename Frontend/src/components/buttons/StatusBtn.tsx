// Button zum Setzen des Tischstatus ("Frei" / "Besetzt") mit farbigem Punkt
interface StatusBtnProps {
  active: boolean;
  color: string;
  label: string;
  onClick: () => void;
}

export default function StatusBtn({ active, color, label, onClick }: StatusBtnProps) {
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
