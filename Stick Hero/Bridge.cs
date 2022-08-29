namespace Stick_Hero;

public class Bridge
{
    private readonly Pen pen = new(Brushes.DarkBlue, 15);

    private double angle = -90;
    private readonly int maxSize;

    private int size;

    private int x;
    private int y;

    public Bridge(int x, int y, int maxSize)
    {
        this.x = x;
        this.y = y;
        this.maxSize = maxSize;
    }

    public event EventHandler<BridgeEventArgs> BridgeWentDown = delegate { };
    public event EventHandler BridgeWentFall = delegate { };

    public void Draw(Graphics graphics)
    {
        graphics.DrawLine(pen,
            x, y,
            (int)(x + Math.Cos(angle * Math.PI / 180) * size),
            (int)(y + Math.Sin(angle * Math.PI / 180) * size));
    }

    public void MoveToPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
        size = 0;
        angle = -90;
    }

    public void IncreaseSize()
    {
        if (size < maxSize)
            size += 10;
    }

    public void Rotate()
    {
        if (angle < 0)
            angle += 2;
        else
            BridgeWentDown(this, new BridgeEventArgs(size)); //todo
    }

    public void Fall()
    {
        if (angle < 90)
            angle += 3;
        else
            BridgeWentFall(this, EventArgs.Empty);
    }
}

//todo
public class BridgeEventArgs : EventArgs
{
    public BridgeEventArgs(int size)
    {
        Size = size;
    }

    public int Size { get; set; }
}