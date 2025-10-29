using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PbButtonDemo.App.Components;

public class PictureButtonBase : ComponentBase
{
    [Parameter] public string Text { get; set; }
    [Parameter] public bool Enabled { get; set; } = true;
    [Parameter] public bool Visible { get; set; } = true;
    [Parameter] public bool FlatStyle { get; set; }
    [Parameter] public bool Default { get; set; }
    [Parameter] public bool Cancel { get; set; }

    [Parameter] public string PictureName { get; set; }
    [Parameter] public string DisabledName { get; set; }
    [Parameter] public bool OriginalSize { get; set; }

    [Parameter] public string Width { get; set; }
    [Parameter] public string Height { get; set; }

    [Parameter] public HTextAlign HTextAlign { get; set; } = HTextAlign.Center;
    [Parameter] public VTextAlign VTextAlign { get; set; } = VTextAlign.Bottom;

    [Parameter] public string TextColor { get; set; }
    [Parameter] public string BackColor { get; set; }
    [Parameter] public string Facename { get; set; }
    [Parameter] public int TextSize { get; set; }
    [Parameter] public bool Italic { get; set; }
    [Parameter] public bool Underline { get; set; }

    [Parameter] public string AccessibleName { get; set; }
    [Parameter] public string AccessibleDescription { get; set; }

    [Parameter] public int TabOrder { get; set; }
    [Parameter] public string PowerTipText { get; set; }

    [Parameter] public EventCallback Clicked { get; set; }
    [Parameter] public EventCallback GotFocus { get; set; }
    [Parameter] public EventCallback LostFocus { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> RButtonDown { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> AdditionalAttributes { get; set; }

    protected string ImageUrl =>
        Enabled ? PictureName : (string.IsNullOrWhiteSpace(DisabledName) ? PictureName : DisabledName);
    protected async Task HandleClick()
    {
        Console.WriteLine("HandleClick invoked"); // Debugging log
        if (Enabled && Clicked.HasDelegate)
        {
            await Clicked.InvokeAsync();
        }
    }

    protected async Task HandleFocus() => await GotFocus.InvokeAsync();
    protected async Task HandleBlur() => await LostFocus.InvokeAsync();     

    protected async Task HandleRightClick(MouseEventArgs e)
    {
        if (RButtonDown.HasDelegate)
            await RButtonDown.InvokeAsync(e);
    }

    protected string BuildStyle()
    {
        if (!Visible) return "display:none;";

        var styles = new List<string>();

        if (!string.IsNullOrEmpty(Width)) styles.Add($"width:{Width}");
        if (!string.IsNullOrEmpty(Height)) styles.Add($"height:{Height}");
        if (!string.IsNullOrEmpty(BackColor)) styles.Add($"background:{BackColor}");
        styles.Add($"justify-content:{MapHAlign(HTextAlign)}");
        styles.Add($"align-items:{MapVAlign(VTextAlign)}");
        styles.Add("display:inline-flex;gap:8px;padding:6px 10px;");

        var fontParts = new List<string>();
        if (TextSize > 0) fontParts.Add($"font-size:{TextSize}px");
        if (!string.IsNullOrEmpty(Facename)) fontParts.Add($"font-family:'{Facename}', sans-serif");
        if (Italic) fontParts.Add("font-style:italic");
        if (Underline) fontParts.Add("text-decoration:underline");
        if (!string.IsNullOrEmpty(TextColor)) fontParts.Add($"color:{TextColor}");
        if (fontParts.Count > 0) styles.Add(string.Join(";", fontParts));

        return string.Join(";", styles);
    }

    private static string MapHAlign(HTextAlign align) => align switch
    {
        HTextAlign.Left => "flex-start",
        HTextAlign.Center => "center",
        HTextAlign.Right => "flex-end",
        _ => "center"
    };

    private static string MapVAlign(VTextAlign align) => align switch
    {
        VTextAlign.Top => "flex-start",
        VTextAlign.Middle => "center",
        VTextAlign.Bottom => "flex-end",
        _ => "center"
    };
}
