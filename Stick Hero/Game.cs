
namespace Stick_Hero
{
    public class Game
    {
        private Hero hero;

        public Game(int width, int height)
        {
            hero = new Hero();
        }

        public void Draw(Graphics graphics)
        {
            hero.Draw(graphics);
        }
    }
}
