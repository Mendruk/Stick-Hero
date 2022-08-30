namespace Stick_Hero;

public class BridgeEventArgs : EventArgs
{
    public BridgeEventArgs(int size)
    {
        Size = size;
    }
    public int Size { get; set; }
}