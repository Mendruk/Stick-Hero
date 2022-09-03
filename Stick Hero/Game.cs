namespace Stick_Hero;

public class Game
{
    private static readonly Font scoreFont = new(FontFamily.GenericMonospace, 50, FontStyle.Bold);
    private static readonly StringFormat Format = new();

    private readonly int width;
    private readonly int height;

    private readonly Random random = new();
    private readonly Rectangle startPlatform;
    private readonly Hero hero;
    private readonly Bridge bridge;

    private Rectangle targetPlatform;

    private int bonusZoneX;
    private int bonusZoneWidth;

    private GameStates gameState = GameStates.Idle;

    public int Score { get; private set; }

    public Game(int width, int height)
    {
        Format.Alignment = StringAlignment.Center;

        this.width = width;
        this.height = height;

        int heroSize = width / 7;

        hero = new Hero(0, width - heroSize, heroSize);
        bridge = new Bridge(heroSize, width, width - heroSize);
        startPlatform = new Rectangle(0, width, heroSize, height - width);

        Restart();
    }

    public void Draw(Graphics graphics)
    {
        int scoreX = width / 2;
        int scoreY = height / 8;

        graphics.DrawString(Score.ToString(), scoreFont, Brushes.White, scoreX, scoreY, Format);
        graphics.FillRectangle(Brushes.Black, startPlatform);
        graphics.FillRectangle(Brushes.Black, targetPlatform);
        graphics.FillRectangle(Brushes.Red, bonusZoneX, startPlatform.Y, bonusZoneWidth, startPlatform.Height);

        hero.Draw(graphics);
        bridge.Draw(graphics);
    }

    public bool TryUpdate()
    {
        switch (gameState)
        {
            case GameStates.Idle:
                return false;
            case GameStates.BridgeIncreases:
                if (!bridge.TryIncreaseLength())
                    gameState = GameStates.BridgeGoingDown;
                break;
            case GameStates.BridgeGoingDown:
                if (!bridge.TryRotateClockwiseToTargetPlatform())
                    gameState = GameStates.HeroWalkToEnd;
                break;
            case GameStates.HeroWalkToEnd:
                if (hero.TryDoStepToEndOfBridge(bridge.Length))
                    break;
                if (AreVictoryConditionMet())
                {
                    if (AreBonusConditionMet())
                        Score += 2;
                    else
                        Score++;

                    Restart();
                }
                else
                {
                    Score = 0;
                    gameState = GameStates.HeroAndBridgeFall;
                }

                break;
            case GameStates.HeroAndBridgeFall:
                hero.DoStepFallDown();
                if (!bridge.TryRotateClockwiseToDown())
                    Restart();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return true;
    }

    public void StartIncreasingBridge()
    {
        if (gameState == GameStates.Idle)
            gameState = GameStates.BridgeIncreases;
    }

    public void StartRotateBridge()
    {
        if (gameState == GameStates.BridgeIncreases)
            gameState = GameStates.BridgeGoingDown;
    }
    private void Restart()
    {
        gameState = GameStates.Idle;

        targetPlatform = GetRandomTargetPlatformWithBonusZone();

        bridge.Length = 0;
        bridge.Angle = -90;
        hero.X = hero.StartX;
        hero.Y = hero.StartY;
    }

    private Rectangle GetRandomTargetPlatformWithBonusZone()
    {
        targetPlatform.X = random.Next(startPlatform.X + startPlatform.Width + hero.Size, bridge.MaxLength - hero.Size);
        targetPlatform.Width = random.Next(hero.Size / 2, hero.Size * 2);

        bonusZoneWidth = targetPlatform.Width / 3;
        bonusZoneX = targetPlatform.X + bonusZoneWidth;

        return targetPlatform = new Rectangle(
            targetPlatform.X, startPlatform.Y,
            targetPlatform.Width, startPlatform.Height);
    }

    private bool AreVictoryConditionMet()
    {
        return bridge.Length >= targetPlatform.X - bridge.X &&
               bridge.Length <= targetPlatform.X + targetPlatform.Width - bridge.X;
    }

    private bool AreBonusConditionMet()
    {
        return bridge.Length >= bonusZoneX - bridge.X &&
               bridge.Length <= bonusZoneX + bonusZoneWidth - bridge.X;
    }
}