// Button "+" / "−" zum Erhöhen oder Verringern der Menge einer Bestellposition
interface QtyBtnProps {
  onClick: () => void;
  label: string;
  small?: boolean;
}

export default function QtyBtn({ onClick, label, small }: QtyBtnProps) {
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
