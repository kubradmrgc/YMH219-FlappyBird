namespace FlappyBird;

/// <summary>
/// Üst ve alt parçadan oluşan, arasında kuşun geçebileceği boşluk bulunan engel.
/// </summary>
public class Pipe
{
    public const int Width = 72;
    public const int GapHeight = 160;

    private readonly int groundTop;

    public Pipe(float x, int gapTop, int groundTop)
    {
        X = x;
        GapTop = gapTop;
        this.groundTop = groundTop;
    }

    public float X { get; private set; }
    public int GapTop { get; private set; }
    public int GapBottom => GapTop + GapHeight;
    public float Right => X + Width;
    public bool IsOffScreen => Right < 0;

    public RectangleF TopRect => new(X, 0, Width, GapTop);
    public RectangleF BottomRect => new(X, GapBottom, Width, groundTop - GapBottom);

    public void Move(float speed)
    {
        X -= speed;
    }

    public void Reposition(float x, int gapTop)
    {
        X = x;
        GapTop = gapTop;
    }
}
