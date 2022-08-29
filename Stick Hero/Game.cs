namespace Stick_Hero;

public class Game
{
    private readonly Random random = new();

    private readonly int heroStartPositionX;
    private readonly int heroStartPositionY;
    private readonly Hero hero;

    private readonly int startPlatformX;
    private readonly int startPlatformY;
    private readonly int startPlatformHeight;
    private readonly int startPlatformWidth;
    private readonly Rectangle startPlatform;

    private readonly int bridgeX;
    private readonly int bridgeY;
    private readonly int bridgeMaxSize;
    private readonly Bridge bridge;
    private int bridgeSize;

    private int targetPlatformX;
    private Rectangle targetPlatform;
    private int targetPlatformWidth;

    private GameStates gameState = GameStates.Idle;

    private readonly int width;
    private readonly int height;

    private static readonly FontStyle CellFontStyle = FontStyle.Bold;
    private static readonly Font scoreFont = new(FontFamily.GenericMonospace, 50, CellFontStyle);
    private static readonly StringFormat Format = new();

    private int Score;

    public Game(int width, int height)
    {
        Format.Alignment = StringAlignment.Center;

        this.width = width;
        this.height = height;

        heroStartPositionX = 50;
        heroStartPositionY = width - 100;

        bridgeX = 150;
        bridgeY = width;
        bridgeMaxSize = width - 150;

        startPlatformX = 0;
        startPlatformY = width;
        startPlatformWidth = 150;
        startPlatformHeight = height - width;

        hero = new Hero(heroStartPositionX, heroStartPositionY);
        bridge = new Bridge(bridgeX, bridgeY, bridgeMaxSize);
        startPlatform = new Rectangle(startPlatformX, startPlatformY, startPlatformWidth, startPlatformHeight);

        bridge.BridgeWentDown += Bridge_BridgeWentDown;
        bridge.BridgeWentFall += Bridge_BridgeWentFall;
        hero.ReachedToEnd += Hero_ReachedToEnd;

        Start();
    }

    public void Draw(Graphics graphics)
    {
        graphics.DrawString(Score.ToString(), scoreFont, Brushes.White, width / 2 - 25, height / 8); //align text to the center of the window

        graphics.FillRectangle(Brushes.Black, startPlatform);
        graphics.FillRectangle(Brushes.Black, targetPlatform);

        hero.Draw(graphics);
        bridge.Draw(graphics);

    }

    public void Start()
    {
        targetPlatform = GetRandomTargetPlatform();
        hero.MoveToPosition(heroStartPositionX, heroStartPositionY);
        bridge.MoveToPosition(bridgeX, bridgeY);

        gameState = GameStates.Idle;
    }

    public void Update()
    {
        switch (gameState)
        {
            case GameStates.Idle:
                break;
            case GameStates.BridgeIncreases:
                bridge.IncreaseSize();
                break;
            case GameStates.BridgeGoingDown:
                bridge.Rotate();
                break;
            case GameStates.HeroWalkToEnd:
                hero.MoveToEndOfBridge(bridgeSize);
                break;
            case GameStates.HeroAndBridgeFall:
                bridge.Fall();
                hero.Fall();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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

    private Rectangle GetRandomTargetPlatform()
    {
        targetPlatformX = random.Next(startPlatformX + startPlatformWidth, bridgeMaxSize);
        targetPlatformWidth = random.Next(25, 150); //todo

        return targetPlatform = new Rectangle(
            targetPlatformX, startPlatformY,
            targetPlatformWidth, startPlatformHeight);
    }

    private bool AreVictoryConditionMet()
    {
        return bridgeSize >= targetPlatformX - bridgeX &&
               bridgeSize <= targetPlatformX + targetPlatformWidth - bridgeX;
    }

    private void Hero_ReachedToEnd(object? sender, EventArgs e)
    {
        if (AreVictoryConditionMet())
        {
            Score++;
            Start();

        }
        else
        {
            Score = 0;
            gameState = GameStates.HeroAndBridgeFall;
        }
    }

    private void Bridge_BridgeWentDown(object? sender, BridgeEventArgs e)
    {
        gameState = GameStates.HeroWalkToEnd;
        bridgeSize = e.Size;
    }

    private void Bridge_BridgeWentFall(object? sender, EventArgs e)
    {
        Start();
    }
}

internal enum GameStates
{
    Idle,
    BridgeIncreases,
    BridgeGoingDown,
    HeroWalkToEnd,
    HeroAndBridgeFall
}