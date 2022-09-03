namespace Stick_Hero;

public class Bridge
{
    private readonly Pen pen = new(Brushes.DarkBlue, 10);
    private readonly int rotationStep = 2;
    private readonly int increaseStep = 10;

    public readonly int MaxLength;
    //pivot coordinate
    public readonly int X;
    public readonly int Y;

    public int Length;
    public int Angle = -90;

    public Bridge(int x, int y, int maxLength)
    {
        this.X = x;
        this.Y = y;
        MaxLength = maxLength;
    }

    public void Draw(Graphics graphics)
    {
        graphics.DrawLine(pen, X, Y,
            (int)(X + Math.Cos(Angle * Math.PI / 180) * Length),
            (int)(Y + Math.Sin(Angle * Math.PI / 180) * Length));
    }

    public bool TryIncreaseLength()
    {
        int increasedLength = Length + increaseStep;

        if (increasedLength < MaxLength)
        {
            Length = increasedLength;
            return true;
        }

        return false;
    }

    public bool TryRotateClockwiseToTargetPlatform()
    {
        int increasedAngle = (int)Angle + rotationStep;

        if (increasedAngle <= 0)
        {
            Angle = increasedAngle;
            return true;
        }
        return false;
    }

    public bool TryRotateClockwiseToDown()
    {
        int increasedAngle = (int)Angle + rotationStep;

        if (increasedAngle <= 90)
        {
            Angle = increasedAngle;
            return true;
        }
        return false;
    }
}