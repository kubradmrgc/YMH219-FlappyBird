namespace FlappyBird;

/// <summary>
/// Oyuncunun kontrol ettiği kuş: yer çekimiyle düşer, zıplayınca yükselir.
/// </summary>
public class Bird
{
    public const int Diameter = 34;
    private const float Gravity = 0.45f;
    private const float FlapVelocity = -7.5f;
    private const float MaxFallSpeed = 10f;
    private const float HitboxMargin = 4f;

    public Bird(float x, float y)
    {
        X = x;
        Y = y;
    }

    public float X { get; }
    public float Y { get; private set; }
    public float VelocityY { get; private set; }

    public RectangleF Bounds => new(X, Y, Diameter, Diameter);

    public RectangleF Hitbox => RectangleF.Inflate(Bounds, -HitboxMargin, -HitboxMargin);

    public float Tilt => Math.Clamp(VelocityY * 4f, -25f, 70f);

    public void Flap()
    {
        VelocityY = FlapVelocity;
    }

    public void Update()
    {
        VelocityY = Math.Min(VelocityY + Gravity, MaxFallSpeed);
        Y += VelocityY;

        if (Y < 0)
        {
            Y = 0;
            VelocityY = 0;
        }
    }

    public void LandOn(float groundTop)
    {
        Y = groundTop - Diameter;
        VelocityY = 0;
    }

    public void Reset(float y)
    {
        Y = y;
        VelocityY = 0;
    }
}
