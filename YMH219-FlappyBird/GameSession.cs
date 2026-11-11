namespace FlappyBird;

public enum GameState
{
    Ready,
    Playing,
    GameOver
}

/// <summary>
/// Oyunun durumunu, hızını ve oyun nesnelerini yöneten sınıf.
/// </summary>
public class GameSession
{
    public const int FieldWidth = 480;
    public const int FieldHeight = 600;
    public const int GroundHeight = 90;
    public const int GroundStripeWidth = 24;
    private const float BirdStartX = 110f;
    private const float StartSpeed = 3f;

    public GameSession()
    {
        Bird = new Bird(BirdStartX, BirdStartY);
        Reset();
    }

    public Bird Bird { get; }
    public GameState State { get; private set; }
    public float Speed { get; private set; }
    public float GroundOffset { get; private set; }

    public static int GroundTop => FieldHeight - GroundHeight;
    private static float BirdStartY => (GroundTop - Bird.Diameter) / 2f;

    public void Reset()
    {
        Bird.Reset(BirdStartY);
        Speed = StartSpeed;
        GroundOffset = 0;
        State = GameState.Ready;
    }

    public void Flap()
    {
        if (State == GameState.GameOver)
        {
            return;
        }

        if (State == GameState.Ready)
        {
            State = GameState.Playing;
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
        if (Bird.Bounds.Bottom >= GroundTop)
        {
            Bird.LandOn(GroundTop);
        }
    }
}
