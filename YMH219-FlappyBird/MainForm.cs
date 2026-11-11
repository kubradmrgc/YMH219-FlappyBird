namespace FlappyBird;

/// <summary>
/// Ana pencere: oyun alanını barındırır ve Timer ile oyun döngüsünü çalıştırır.
/// </summary>
public class MainForm : Form
{
    private const int FrameIntervalMs = 20;

    private readonly GamePanel gamePanel = new() { Dock = DockStyle.Fill };
    private readonly System.Windows.Forms.Timer gameTimer = new() { Interval = FrameIntervalMs };
    private readonly StatusStrip statusStrip = new() { SizingGrip = false };
    private readonly ToolStripStatusLabel stateLabel = new() { Text = "Hazır - oyun döngüsü çalışıyor" };

    public MainForm()
    {
        Text = "YMH219 - Flappy Bird";
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        KeyPreview = true;

        statusStrip.Items.Add(stateLabel);

        Controls.Add(gamePanel);
        Controls.Add(statusStrip);

        gameTimer.Tick += GameTimer_Tick;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ClientSize = new Size(GamePanel.FieldWidth, GamePanel.FieldHeight + statusStrip.Height);
        CenterToScreen();
        gameTimer.Start();
    }

    private void GameTimer_Tick(object? sender, EventArgs e)
    {
        gamePanel.Advance();
        gamePanel.Invalidate();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            gameTimer.Dispose();
        }

        base.Dispose(disposing);
    }
}
