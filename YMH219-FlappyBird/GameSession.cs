namespace FlappyBird;

public enum GameState
{
    Ready,
    Playing,
    GameOver
}

/// <summary>
/// Oyunun durumunu, skorunu, hızını ve oyun nesnelerini yöneten sınıf.
/// </summary>
public class GameSession
{
    public const int FieldWidth = 480;
    public const int FieldHeight = 600;
    public const int GroundHeight = 90;
    public const int GroundStripeWidth = 24;
    private const float BirdStartX = 110f;
    private const float StartSpeed = 3f;
    private const int PipeCount = 3;
    private const float PipeSpacing = 240f;
    private const float FirstPipeX = FieldWidth + 60f;
    private const int MinGapTop = 60;

    private readonly List<Pipe> pipes = new();
    private readonly Random random = new();

    public GameSession()
    {
        Bird = new Bird(BirdStartX, BirdStartY);
        Reset();
    }

    public event EventHandler? ScoreChanged;
    public event EventHandler? StateChanged;

    public Bird Bird { get; }
    public GameState State { get; private set; }
    public int Score { get; private set; }
    public float Speed { get; private set; }
    public float GroundOffset { get; private set; }
    public IReadOnlyList<Pipe> Pipes => pipes;

    public static int GroundTop => FieldHeight - GroundHeight;
    private static float BirdStartY => (GroundTop - Bird.Diameter) / 2f;

    public void Reset()
    {
        Bird.Reset(BirdStartY);
        Speed = StartSpeed;
        GroundOffset = 0;

        pipes.Clear();
        for (int i = 0; i < PipeCount; i++)
        {
            pipes.Add(new Pipe(FirstPipeX + i * PipeSpacing, RandomGapTop(), GroundTop));
        }

        Score = 0;
        ScoreChanged?.Invoke(this, EventArgs.Empty);
        SetState(GameState.Ready);
    }

    public void Flap()
    {
        if (State == GameState.GameOver)
        {
            return;
        }

        if (State == GameState.Ready)
        {
            SetState(GameState.Playing);
        }

        Bird.Flap();
    }

    public void Update()
    {
        if (State == GameState.GameOver)
        {
            return;
        }

        GroundOffset = (GroundOffset + Speed) % GroundStripeWidth;

        if (State == GameState.Ready)
        {
            return;
        }

        Bird.Update();
        MovePipes();
        UpdateScore();

        if (HasCollision())
        {
            SetState(GameState.GameOver);
        }
    }

    private void MovePipes()
    {
        foreach (Pipe pipe in pipes)
        {
            pipe.Move(Speed);
        }

        foreach (Pipe pipe in pipes.Where(p => p.IsOffScreen))
        {
            float rightmostX = pipes.Max(p => p.X);
            pipe.Reposition(rightmostX + PipeSpacing, RandomGapTop());
        }
    }

    private void UpdateScore()
    {
        foreach (Pipe pipe in pipes.Where(p => !p.Passed && p.Right < Bird.X))
        {
            pipe.MarkPassed();
            Score++;
            ScoreChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool HasCollision()
    {
        if (Bird.Bounds.Bottom >= GroundTop)
        {
            Bird.LandOn(GroundTop);
            return true;
        }

        RectangleF hitbox = Bird.Hitbox;
        return pipes.Any(p => p.CollidesWith(hitbox));
    }

    private void SetState(GameState state)
    {
        State = state;
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    private int RandomGapTop()
    {
        int maxGapTop = GroundTop - Pipe.GapHeight - MinGapTop;
        return random.Next(MinGapTop, maxGapTop + 1);
    }
}
