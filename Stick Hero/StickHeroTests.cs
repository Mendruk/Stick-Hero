using NUnit.Framework;

namespace Stick_Hero;

[TestFixture]
public class StickHeroTests
{
    private const int Width = 500;
    private const int Height = 800;
    private Game game;

    [SetUp]
    public void BeforeEachTests()
    {
        game = new Game(Width, Height);
    }

    [Test]
    public void TestScore()
    {
        int expectedResult = game.Score;

        Assert.That(expectedResult, Is.EqualTo(0));
    }
}
