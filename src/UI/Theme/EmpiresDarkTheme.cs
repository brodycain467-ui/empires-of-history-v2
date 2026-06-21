using Godot;

namespace EmpiresOfHistoryV2.UI.Theme;

/// <summary>
/// Programmatic Godot Theme matching the approved dark/gold visual style.
///
/// Color palette (from docs/ui/approved/README.md):
///   Background ........... #1a1208  (deep near-black brown)
///   Panel surface ........ #2a1f0e  (dark olive-brown)
///   Gold accent .......... #c9a84c  (warm gold — headers, highlights, borders)
///   Text primary ......... #f0e6cc  (cream off-white)
///   Text secondary ....... #a08060  (muted tan)
///   Ocean / map bg ....... #0d1b2a  (dark navy)
///   Button CTA ........... #8b6914  (dark gold/bronze)
///   Button danger ........ #8b1414  (deep red)
///
/// Call Create() once at startup and assign the result to the root Control's Theme property.
/// </summary>
public static class EmpiresDarkTheme
{
    // ── Palette constants ──────────────────────────────────────────────────────────

    public static readonly Color Background    = Color.FromHtml("#1a1208");
    public static readonly Color PanelSurface  = Color.FromHtml("#2a1f0e");
    public static readonly Color GoldAccent    = Color.FromHtml("#c9a84c");
    public static readonly Color TextPrimary   = Color.FromHtml("#f0e6cc");
    public static readonly Color TextSecondary = Color.FromHtml("#a08060");
    public static readonly Color OceanColor    = Color.FromHtml("#0d1b2a");
    public static readonly Color ButtonCta     = Color.FromHtml("#8b6914");
    public static readonly Color ButtonDanger  = Color.FromHtml("#8b1414");
    public static readonly Color BorderSubtle  = new Color(0.788f, 0.659f, 0.298f, 0.35f); // gold @ 35% alpha

    // ── Create ─────────────────────────────────────────────────────────────────────

    /// <summary>Creates and returns a fully configured Godot Theme resource.</summary>
    public static Godot.Theme Create()
    {
        var theme = new Godot.Theme();

        // ── Panel ──────────────────────────────────────────────────────────────────
        var panelStyle = MakeFlatBox(PanelSurface, BorderSubtle, cornerRadius: 0);
        theme.SetStylebox("panel", "Panel", panelStyle);

        // ── Label ──────────────────────────────────────────────────────────────────
        theme.SetColor("font_color", "Label", TextPrimary);

        // ── Button (default) ───────────────────────────────────────────────────────
        var btnNormal = MakeFlatBox(PanelSurface, new Color(GoldAccent.R, GoldAccent.G, GoldAccent.B, 1f), cornerRadius: 0);
        var btnHover  = MakeFlatBox(Color.FromHtml("#3a2f1e"), new Color(GoldAccent.R, GoldAccent.G, GoldAccent.B, 0.6f), cornerRadius: 0);
        var btnPress  = MakeFlatBox(PanelSurface.Darkened(0.08f), GoldAccent, cornerRadius: 0);
        var btnFocus  = MakeFlatBox(PanelSurface, GoldAccent, cornerRadius: 0);

        theme.SetStylebox("normal",   "Button", btnNormal);
        theme.SetStylebox("hover",    "Button", btnHover);
        theme.SetStylebox("pressed",  "Button", btnPress);
        theme.SetStylebox("focus",    "Button", btnFocus);
        theme.SetStylebox("disabled", "Button", MakeFlatBox(PanelSurface.Darkened(0.2f), new Color(0.4f, 0.4f, 0.4f, 0.25f), cornerRadius: 0));

        theme.SetColor("font_color",          "Button", TextPrimary);
        theme.SetColor("font_hover_color",    "Button", GoldAccent);
        theme.SetColor("font_pressed_color",  "Button", GoldAccent);
        theme.SetColor("font_disabled_color", "Button", new Color(TextSecondary.R, TextSecondary.G, TextSecondary.B, 0.5f));

        // ── HSeparator ─────────────────────────────────────────────────────────────
        var separator = new StyleBoxLine
        {
            Color = Color.FromHtml("#c9a84c4d"),
            Thickness = 1
        };
        theme.SetStylebox("separator", "HSeparator", separator);

        return theme;
    }

    // ── Helpers ────────────────────────────────────────────────────────────────────

    private static StyleBoxFlat MakeFlatBox(Color bg, Color borderColor, int cornerRadius = 0)
    {
        return new StyleBoxFlat
        {
            BgColor                 = bg,
            BorderColor             = borderColor,
            BorderWidthTop          = 1,
            BorderWidthRight        = 1,
            BorderWidthBottom       = 1,
            BorderWidthLeft         = 1,
            CornerRadiusTopLeft     = cornerRadius,
            CornerRadiusTopRight    = cornerRadius,
            CornerRadiusBottomLeft  = cornerRadius,
            CornerRadiusBottomRight = cornerRadius
        };
    }
}
