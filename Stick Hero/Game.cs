
namespace Stick_Hero
{
    public class Game
    {
        public static Random random = new();
        private readonly Bitmap heroSprite = Resource.Hero;

        private Rectangle startPlatformRectangle;
        private Rectangle targetPlatformRectangle;
        private Rectangle heroRectangle;

        public Game(int width, int height)
        {
            startPlatformRectangle = new Rectangle(0, width, 150, height-width);
            targetPlatformRectangle = new Rectangle(random.Next(200, width - 150), width, random.Next(50,150), height - width);
            heroRectangle =new Rectangle(50, width-100, 100, 100);
        }

        public void Draw(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.Black, startPlatformRectangle);
            graphics.FillRectangle(Brushes.Black, targetPlatformRectangle);
            graphics.DrawImage(heroSprite,heroRectangle);
        }
    }
}
