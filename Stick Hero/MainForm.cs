using System.Drawing.Drawing2D;

namespace Stick_Hero
{
    public partial class MainForm : Form
    {
        private const int GameFieldWidth = 800;
        private const int GameFieldHeight = 600;

        private Game game;

        private readonly LinearGradientBrush gradientBrush = new LinearGradientBrush(
            new Point(0, 0),
            new Point(0, GameFieldHeight),
            Color.FromArgb(255, 100, 100, 255), 
            Color.FromArgb(255, 255, 255, 255)); 

        public MainForm()
        {
            InitializeComponent();

            Width=GameFieldWidth;
            Height=GameFieldHeight;

            game=new Game(Width,Height);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(gradientBrush,0,0,Width,Height);

            game.Draw(e.Graphics);
        }
    }
}