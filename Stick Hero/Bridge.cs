namespace Stick_Hero;

public class Bridge
{
    private readonly Pen pen = new(Brushes.DarkBlue, 10);

    // 90 % rotationStep = 0;
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
        X = x;
        Y = y;
        MaxLength = maxLength;
    }

    public void Draw(Graphics graphics)
    {
        graphics.DrawLine(pen, X, Y,
            (int)(X + Math.Cos(Angle * Math.PI / 180) * Length),
            (int)(Y + Math.Sin(Angle * Math.PI / 180) * Length));
    }

    public void IncreaseLength()
    {
        Length += increaseStep;
    }

    public void RotateClockwise()
    {
        Angle += rotationStep;
    }

    public bool IsLengthAchivedMaximum()
    {
        if (Length >= MaxLength)
            return true;

        return false;
    }

    public bool IsStandHorizontal()
    {
        if (Angle == 0)
            return true;

        return false;
    }

    public bool IsStandVertical()
    {
        if (Angle == 90)
            return true;

        return false;
    }
}