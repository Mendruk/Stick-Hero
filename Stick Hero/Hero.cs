namespace Stick_Hero
{
    public class Hero
    {
        private readonly Bitmap heroSprite = Resource.Hero;

        public int X;
        public int Y;

        public void Draw(Graphics graphics)
        {
            graphics.DrawImage(heroSprite, X, Y);
        }
    }
}
