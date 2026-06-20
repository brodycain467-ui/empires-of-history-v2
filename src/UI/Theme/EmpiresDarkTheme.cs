using Godot;

namespace EmpiresOfHistoryV2.UI.Theme;

public static class EmpiresDarkTheme
{
    public static Godot.Theme Create()
    {
        var theme = new Godot.Theme();

        var panelStyle = new StyleBoxFlat
        {
            BgColor = Color.FromHtml("#1a1208"),
            CornerRadiusTopLeft = 0,
            CornerRadiusTopRight = 0,
            CornerRadiusBottomLeft = 0,
            CornerRadiusBottomRight = 0
        };

        var buttonNormal = new StyleBoxFlat
        {
            BgColor = Color.FromHtml("#2a1f0e"),
            BorderColor = Color.FromHtml("#c9a84c"),
            BorderWidthBottom = 1,
            BorderWidthTop = 1,
            BorderWidthLeft = 1,
            BorderWidthRight = 1
        };

        var buttonHover = buttonNormal.Duplicate() as StyleBoxFlat ?? new StyleBoxFlat();
        buttonHover.BgColor = Color.FromHtml("#3a2f1e");

        var separator = new StyleBoxLine
        {
            Color = Color.FromHtml("#4dc9a84c"),
            Thickness = 1
        };

        theme.SetStylebox("panel", "Panel", panelStyle);
        theme.SetStylebox("normal", "Button", buttonNormal);
        theme.SetStylebox("hover", "Button", buttonHover);
        theme.SetColor("font_color", "Label", Color.FromHtml("#f0e6cc"));
        theme.SetColor("font_color", "Button", Color.FromHtml("#f0e6cc"));
        theme.SetStylebox("separator", "HSeparator", separator);

        return theme;
    }
}
