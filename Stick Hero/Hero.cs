namespace Stick_Hero;

public class Hero
{
    private const int StepSize = 10;
    private readonly int size;
    private readonly Bitmap heroSprite = Resource.Hero;

    private int x;
    private int y;

    public Hero(int x, int y, int heroSize)
    {
        this.x = x;
        this.y = y;
        size = heroSize;
    }

    public event EventHandler HeroAchievedEndOfBridge = delegate { };

    public void Draw(Graphics graphics)
    {
        graphics.DrawImage(heroSprite, x, y, size, size);
    }

    public void ReturnToPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void DoStepToEndOfBridge(int bridgeLength)
    {
        if (x < bridgeLength)
            x += StepSize;
        else
            HeroAchievedEndOfBridge(this, EventArgs.Empty);
    }

    public void DoStepFallDown()
    {
        y += StepSize;
    }
}