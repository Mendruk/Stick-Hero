namespace Stick_Hero;

public class Game
{
    private static readonly FontStyle CellFontStyle = FontStyle.Bold;
    private static readonly Font scoreFont = new(FontFamily.GenericMonospace, 50, CellFontStyle);
    private static readonly StringFormat Format = new();

    private readonly Random random = new();

    private readonly int heroStartPositionX;
    private readonly int heroStartPositionY;
    private readonly int heroSize;
    private readonly Hero hero;

    private readonly int startPlatformX;
    private readonly int startPlatformY;
    private readonly int startPlatformHeight;
    private readonly int startPlatformWidth;
    private readonly Rectangle startPlatform;

    private readonly int bridgePivotX;
    private readonly int bridgePivotY;
    private readonly int bridgeMaxSize;
    private readonly Bridge bridge;
    private int bridgeSize;

    private int targetPlatformX;
    private int targetPlatformWidth;
    private Rectangle targetPlatform;

    private int bonusZoneX;
    private int bonusZoneWidth;

    private GameStates gameState = GameStates.Idle;

    private readonly int scoreX;
    private readonly int scoreY;
    public int Score { get; private set; }

    public Game(int width, int height)
    {
        Format.Alignment = StringAlignment.Center;

        heroSize = width / 7;
        heroStartPositionX = 0;
        heroStartPositionY = width - heroSize;

        startPlatformX = 0;
        startPlatformY = width;
        startPlatformWidth = heroSize;
        startPlatformHeight = height - width;

        bridgePivotX = heroSize;
        bridgePivotY = width;
        bridgeMaxSize = width - heroSize;

        scoreX = width / 2;
        scoreY = height / 8;

        hero = new Hero(heroStartPositionX, heroStartPositionY, heroSize);
        bridge = new Bridge(bridgePivotX, bridgePivotY, bridgeMaxSize);
        startPlatform = new Rectangle(startPlatformX, startPlatformY, startPlatformWidth, startPlatformHeight);

        bridge.BridgeLengthAchievedMax += OnBridgeLengthAchievedMax;
        bridge.RightEndOfBridgeAchievedTargetPlatform += OnRightEndOfBridgeAchievedTargetPlatform;
        bridge.RightEndOfBridgeAchievedDown += OnRightEndOfBridgeAchievedDown;
        hero.HeroAchievedEndOfBridge += OnHeroAchievedEndOfBridge;

        Restart();
    }

    public void Draw(Graphics graphics)
    {
        graphics.DrawString(Score.ToString(), scoreFont, Brushes.White, scoreX, scoreY, Format); //align text to the center of the window

        graphics.FillRectangle(Brushes.Black, startPlatform);
        graphics.FillRectangle(Brushes.Black, targetPlatform);
        graphics.FillRectangle(Brushes.Red, bonusZoneX, startPlatformY, bonusZoneWidth, startPlatformHeight);

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
                bridge.IncreaseLengthOnStepToMaximum();
                break;
            case GameStates.BridgeGoingDown:
                bridge.DoRotationStepToTargetPlatform();
                break;
            case GameStates.HeroWalkToEnd:
                hero.DoStepToEndOfBridge(bridgeSize);
                break;
            case GameStates.HeroAndBridgeFall:
                hero.DoStepFallDown();
                bridge.DoStepToRightEndFallDown();
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

        bridge.ResetRotationAndPosition(bridgePivotX, bridgePivotY);
        hero.ReturnToPosition(heroStartPositionX, heroStartPositionY);
    }

    private Rectangle GetRandomTargetPlatformWithBonusZone()
    {
        targetPlatformX = random.Next(startPlatformX + startPlatformWidth + heroSize, bridgeMaxSize - heroSize);
        targetPlatformWidth = random.Next(heroSize / 2, heroSize * 2);

        bonusZoneWidth = targetPlatformWidth / 3;
        bonusZoneX = targetPlatformX + bonusZoneWidth;

        return targetPlatform = new Rectangle(
            targetPlatformX, startPlatformY,
            targetPlatformWidth, startPlatformHeight);
    }

    private bool AreVictoryConditionMet()
    {
        return bridgeSize >= targetPlatformX - bridgePivotX &&
               bridgeSize <= targetPlatformX + targetPlatformWidth - bridgePivotX;
    }

    private bool AreBonusConditionMet()
    {
        return bridgeSize >= bonusZoneX - bridgePivotX &&
               bridgeSize <= bonusZoneX + bonusZoneWidth - bridgePivotX;
    }

    private void OnHeroAchievedEndOfBridge(object? sender, EventArgs e)
    {
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
    }
    private void OnBridgeLengthAchievedMax(object? sender, BridgeEventArgs e)
    {
        StartRotateBridge();
    }

    private void OnRightEndOfBridgeAchievedTargetPlatform(object? sender, BridgeEventArgs e)
    {
        gameState = GameStates.HeroWalkToEnd;
        bridgeSize = e.Size;
    }

    private void OnRightEndOfBridgeAchievedDown(object? sender, EventArgs e)
    {
        Restart();
    }
}