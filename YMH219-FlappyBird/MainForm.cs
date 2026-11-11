namespace FlappyBird;

/// <summary>
/// Ana pencere: oyun alanını barındırır, Timer ile oyun döngüsünü çalıştırır ve klavyeyi dinler.
/// </summary>
public class MainForm : Form
{
    private const int FrameIntervalMs = 20;

    private readonly GameSession session = new();
    private readonly GamePanel gamePanel;
    private readonly System.Windows.Forms.Timer gameTimer = new() { Interval = FrameIntervalMs };
    private readonly StatusStrip statusStrip = new() { SizingGrip = false };
    private readonly ToolStripStatusLabel scoreLabel = new();
    private readonly ToolStripStatusLabel stateLabel = new() { Spring = true, TextAlign = ContentAlignment.MiddleRight };

    public MainForm()
    {
        gamePanel = new GamePanel(session) { Dock = DockStyle.Fill };

        Text = "YMH219 - Flappy Bird";
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        KeyPreview = true;

        statusStrip.Items.AddRange(new ToolStripItem[] { scoreLabel, stateLabel });

        Controls.Add(gamePanel);
        Controls.Add(statusStrip);

        session.ScoreChanged += (_, _) => UpdateStatusBar();
        session.StateChanged += (_, _) => UpdateStatusBar();
        gameTimer.Tick += GameTimer_Tick;
        KeyDown += MainForm_KeyDown;

        UpdateStatusBar();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ClientSize = new Size(GameSession.FieldWidth, GameSession.FieldHeight + statusStrip.Height);
        CenterToScreen();
        gameTimer.Start();
    }

    private void GameTimer_Tick(object? sender, EventArgs e)
    {
        session.Update();
        gamePanel.Invalidate();
    }

    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode is Keys.Space or Keys.Up)
        {
            session.Flap();
            e.SuppressKeyPress = true;
        }
    }

    private void UpdateStatusBar()
    {
        scoreLabel.Text = $"Skor: {session.Score}";
        stateLabel.Text = session.State switch
        {
            GameState.Ready => "Hazır - Space / Yukarı Ok ile başla",
            GameState.Playing => "Oynanıyor",
            _ => "Oyun bitti"
        };
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
