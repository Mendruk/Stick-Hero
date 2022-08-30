namespace Stick_Hero;

public class Hero
{
    private const int Size = 100;
    private const int StepSize = 15;
    private readonly Bitmap heroSprite = Resource.Hero;

    private int x;
    private int y;

    public Hero(int x, int y)
    {
        this.x = x ;
        this.y = y ;
    }

    public event EventHandler HeroAchievedEndOfBridge = delegate { };

    public void Draw(Graphics graphics)
    {
        graphics.DrawImage(heroSprite, x, y, Size, Size);
    }

    public void ReturnToPosition(int x, int y)
    {
        this.x = x ;
        this.y = y ;
    }

    public void DoStepToEndOfBridge(int bridgeLength)
    {
        if (x < bridgeLength + Size/2)
            x += StepSize;
        else
            HeroAchievedEndOfBridge(this, EventArgs.Empty);
    }

    public void DoStepFallDown()
    {
        y += StepSize;
    }
}