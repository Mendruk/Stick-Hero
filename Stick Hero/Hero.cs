namespace Stick_Hero;

public class Hero
{
    private readonly Bitmap heroSprite = Resource.Hero;
    private const int StepSize = 10;

    public readonly int StartX;
    public readonly int StartY;
    public int X;
    public int Y;
    public int Size;

    public Hero(int x, int y, int heroSize)
    {
        X = StartX = x;
        Y = StartY = y;
        Size = heroSize;
    }

    public void Draw(Graphics graphics)
    {
        graphics.DrawImage(heroSprite, X, Y, Size, Size);
    }

    public bool TryDoStepToEndOfBridge(int bridgeLength)
    {
        if (X < bridgeLength)
        {
            X += StepSize;
            return true;
        }

        return false;
    }

    public void DoStepFallDown()
    {
        Y += StepSize;
    }
}