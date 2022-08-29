using System.Drawing.Drawing2D;

namespace Stick_Hero;

public partial class MainForm : Form
{
    private const int GameFieldWidth = 700;
    private const int GameFieldHeight = 900;

    private readonly LinearGradientBrush gradientBrush = new(
        new Point(0, 0),
        new Point(0, GameFieldHeight),
        Color.FromArgb(255, 0, 200, 255),
        Color.FromArgb(255, 255, 255, 255));

    private readonly Game game;

    public MainForm()
    {
        InitializeComponent();

        Width = GameFieldWidth;
        Height = GameFieldHeight;

        game = new Game(Width, Height);
    }

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
        game.StartIncreasingBridge();
        pictureGameField.Refresh();
    }

    private void MainForm_KeyUp(object sender, KeyEventArgs e)
    {
        game.StartRotateBridge();
        pictureGameField.Refresh();
    }

    private void pictureGameField_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.FillRectangle(gradientBrush, 0, 0, Width, Height);

        game.Draw(e.Graphics);
    }

    private void timer_Tick(object sender, EventArgs e)
    {
        game.Update();
        Refresh();
    }
}