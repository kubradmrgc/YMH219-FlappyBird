using System.Drawing.Drawing2D;

namespace FlappyBird;

/// <summary>
/// Oyun alanını çizen, titremeyi önlemek için çift tamponlu panel.
/// </summary>
public class GamePanel : Panel
{
    public const int FieldWidth = 480;
    public const int FieldHeight = 600;
    public const int GroundHeight = 90;
    private const int GroundStripeWidth = 24;
    private const int GrassHeight = 18;
    private const float ScrollSpeed = 3f;

    private readonly Font titleFont = new("Segoe UI", 36f, FontStyle.Bold);
    private readonly Font infoFont = new("Segoe UI", 14f, FontStyle.Bold);
    private float groundOffset;

    public GamePanel()
    {
        DoubleBuffered = true;
        BackColor = Color.FromArgb(112, 197, 206);
    }

    private static int GroundTop => FieldHeight - GroundHeight;

    public void Advance()
    {
        groundOffset = (groundOffset + ScrollSpeed) % GroundStripeWidth;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        DrawGround(g);
        DrawCenteredText(g, "Flappy Bird", titleFont, FieldHeight / 3f);
        DrawCenteredText(g, "Oyun döngüsü çalışıyor", infoFont, FieldHeight / 3f + 60);
    }

    private void DrawGround(Graphics g)
    {
        using var dirt = new SolidBrush(Color.FromArgb(222, 216, 149));
        using var grass = new SolidBrush(Color.FromArgb(115, 191, 46));
        using var stripe = new SolidBrush(Color.FromArgb(94, 160, 36));

        g.FillRectangle(dirt, 0, GroundTop, FieldWidth, GroundHeight);
        g.FillRectangle(grass, 0, GroundTop, FieldWidth, GrassHeight);
        for (float x = -groundOffset; x < FieldWidth; x += GroundStripeWidth)
        {
            g.FillRectangle(stripe, x, GroundTop, GroundStripeWidth / 2f, GrassHeight);
        }
    }

    private static void DrawCenteredText(Graphics g, string text, Font font, float centerY)
    {
        SizeF size = g.MeasureString(text, font);
        float x = (FieldWidth - size.Width) / 2;
        float y = centerY - size.Height / 2;

        using var shadow = new SolidBrush(Color.FromArgb(120, 0, 0, 0));
        g.DrawString(text, font, shadow, x + 2, y + 2);
        g.DrawString(text, font, Brushes.White, x, y);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            titleFont.Dispose();
            infoFont.Dispose();
        }

        base.Dispose(disposing);
    }
}
