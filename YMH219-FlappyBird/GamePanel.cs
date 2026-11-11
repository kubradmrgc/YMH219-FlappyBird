using System.Drawing.Drawing2D;

namespace FlappyBird;

/// <summary>
/// Oyun alanını çizen, titremeyi önlemek için çift tamponlu panel.
/// </summary>
public class GamePanel : Panel
{
    private const int GrassHeight = 18;

    private readonly GameSession session;
    private readonly Font titleFont = new("Segoe UI", 36f, FontStyle.Bold);
    private readonly Font infoFont = new("Segoe UI", 14f, FontStyle.Bold);

    public GamePanel(GameSession session)
    {
        this.session = session;
        DoubleBuffered = true;
        BackColor = Color.FromArgb(112, 197, 206);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        DrawGround(g);
        DrawBird(g, session.Bird);

        if (session.State == GameState.Ready)
        {
            DrawCenteredText(g, "Flappy Bird", titleFont, GameSession.FieldHeight / 4f);
            DrawCenteredText(g, "Başlamak için Space / Yukarı Ok", infoFont, GameSession.FieldHeight / 4f + 60);
        }
    }

    private void DrawGround(Graphics g)
    {
        int top = GameSession.GroundTop;
        using var dirt = new SolidBrush(Color.FromArgb(222, 216, 149));
        using var grass = new SolidBrush(Color.FromArgb(115, 191, 46));
        using var stripe = new SolidBrush(Color.FromArgb(94, 160, 36));

        g.FillRectangle(dirt, 0, top, GameSession.FieldWidth, GameSession.GroundHeight);
        g.FillRectangle(grass, 0, top, GameSession.FieldWidth, GrassHeight);
        for (float x = -session.GroundOffset; x < GameSession.FieldWidth; x += GameSession.GroundStripeWidth)
        {
            g.FillRectangle(stripe, x, top, GameSession.GroundStripeWidth / 2f, GrassHeight);
        }
    }

    private static void DrawBird(Graphics g, Bird bird)
    {
        RectangleF bounds = bird.Bounds;
        float half = bounds.Width / 2;
        GraphicsState saved = g.Save();
        g.TranslateTransform(bounds.X + half, bounds.Y + half);
        g.RotateTransform(bird.Tilt);

        var body = new RectangleF(-half, -half, bounds.Width, bounds.Height);
        using var bodyBrush = new SolidBrush(Color.FromArgb(250, 204, 21));
        using var wingBrush = new SolidBrush(Color.FromArgb(253, 230, 138));
        using var beakBrush = new SolidBrush(Color.FromArgb(249, 115, 22));
        using var outline = new Pen(Color.FromArgb(120, 70, 0), 2f);

        g.FillEllipse(bodyBrush, body);
        g.DrawEllipse(outline, body);
        g.FillEllipse(wingBrush, -half + 4, 0, 16, 10);
        g.FillEllipse(Brushes.White, 4, -half + 5, 12, 12);
        g.FillEllipse(Brushes.Black, 10, -half + 8, 5, 5);
        g.FillPolygon(beakBrush, new[]
        {
            new PointF(half - 4, -2),
            new PointF(half + 10, 3),
            new PointF(half - 4, 8)
        });

        g.Restore(saved);
    }

    private static void DrawCenteredText(Graphics g, string text, Font font, float centerY)
    {
        SizeF size = g.MeasureString(text, font);
        float x = (GameSession.FieldWidth - size.Width) / 2;
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
