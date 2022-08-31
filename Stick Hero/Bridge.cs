namespace Stick_Hero;

public class Bridge
{
    private readonly Pen pen = new(Brushes.DarkBlue, 10);
    private readonly int rotationStep = 2;//90 % rotationStep = 0!
    private readonly int increaseStep = 10;

    private double angle = -90;

    private readonly int maxLength;

    private int length;

    private int x;
    private int y;

    public Bridge(int x, int y, int maxLength)
    {
        this.x = x;
        this.y = y;
        this.maxLength = maxLength;
    }

    public event EventHandler<BridgeEventArgs> BridgeLengthAchievedMax = delegate { };
    public event EventHandler<BridgeEventArgs> RightEndOfBridgeAchievedTargetPlatform = delegate { };
    public event EventHandler RightEndOfBridgeAchievedDown = delegate { };

    public void Draw(Graphics graphics)
    {
        graphics.DrawLine(pen, x, y,
            (int)(x + Math.Cos(angle * Math.PI / 180) * length),
            (int)(y + Math.Sin(angle * Math.PI / 180) * length));
    }

    public void ResetRotationAndPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
        length = 0;
        angle = -90;
    }

    public void IncreaseLengthOnStepToMaximum()
    {
        if (length < maxLength)
            length += increaseStep;
        else
            BridgeLengthAchievedMax(this, new BridgeEventArgs(length));
    }

    public void DoRotationStepToTargetPlatform()
    {
        if (angle < 0)
            angle += rotationStep;
        else
            RightEndOfBridgeAchievedTargetPlatform(this, new BridgeEventArgs(length));
    }

    public void DoStepToRightEndFallDown()
    {
        if (angle < 90)
            angle += rotationStep;
        else
            RightEndOfBridgeAchievedDown(this, EventArgs.Empty);
    }
}