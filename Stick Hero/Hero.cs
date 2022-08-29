namespace Stick_Hero;

public class Hero
{
    private const int Size = 100;
    private readonly Bitmap heroSprite = Resource.Hero;
    public int X;
    public int Y;

    public Hero(int x, int y)
    {
        X = x;
        Y = y;
    }

    public event EventHandler ReachedToEnd = delegate { };

    public void Draw(Graphics graphics)
    {
        graphics.DrawImage(heroSprite, X, Y, Size, Size);
    }

    public void MoveToPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void MoveToEndOfBridge(int size)
    {
        if (X < size + Size)
            X += 10;
        else
            ReachedToEnd(this, EventArgs.Empty);
    }

    public void Fall()
    {
        Y += 8;
    }
}