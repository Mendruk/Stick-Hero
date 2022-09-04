namespace Stick_Hero;

public class Game
{
    private static readonly Font ScoreFont = new(FontFamily.GenericMonospace, 50, FontStyle.Bold);
    private static readonly StringFormat Format = new();
   
    private readonly Random random = new();
    private readonly Bridge bridge;
    private readonly Hero hero;
    private readonly Rectangle startPlatform;
    
    private readonly int height;
    private readonly int width;

    private int bonusZoneWidth;
    private int bonusZoneX;

    private Rectangle targetPlatform;

    public Game(int width, int height)
    {
        Format.Alignment = StringAlignment.Center;

        this.width = width;
        this.height = height;

        int heroSize = width / 7;

        hero = new Hero(0, width - heroSize, heroSize);
        bridge = new Bridge(heroSize, width, width - heroSize);
        startPlatform = new Rectangle(0, width, heroSize, height - width);

        StartNextLevel();
    }

    public GameState CurrentGameState { get; private set; }
    public int Score { get; private set; }

    public void Draw(Graphics graphics)
    {
        int scoreX = width / 2;
        int scoreY = height / 8;

        graphics.DrawString(Score.ToString(), ScoreFont, Brushes.White, scoreX, scoreY, Format);
        graphics.FillRectangle(Brushes.Black, startPlatform);
        graphics.FillRectangle(Brushes.Black, targetPlatform);
        graphics.FillRectangle(Brushes.Red, bonusZoneX, startPlatform.Y, bonusZoneWidth, startPlatform.Height);

        hero.Draw(graphics);
        bridge.Draw(graphics);
    }

    public void Update(out bool isGameStateIdle)
    {
        switch (CurrentGameState)
        {
            case GameState.Idle:
                isGameStateIdle = true;
                return;
            case GameState.BridgeIncreases:
                bridge.IncreaseLength();

                if (bridge.IsLengthAchivedMaximum())
                    CurrentGameState = GameState.BridgeGoingDown;

                break;
            case GameState.BridgeGoingDown:
                bridge.RotateClockwise();

                if (bridge.IsStandHorizontal())
                    CurrentGameState = GameState.HeroWalkToEnd;
                break;
            case GameState.HeroWalkToEnd:
                hero.DoStepToEndOfBridge();

                if (hero.X <= bridge.Length)
                    break;

                if (IsBridgeHitOnTargetPlatform())
                {
                    if (IsBridgeHitInBonusZone())
                        Score += 2;
                    else
                        Score++;

                    StartNextLevel();
                }
                else
                {
                    Score = 0;
                    CurrentGameState = GameState.HeroAndBridgeFall;
                }

                break;
            case GameState.HeroAndBridgeFall:
                hero.DoStepFallDown();
                bridge.RotateClockwise();

                if (bridge.IsStandVertical())
                    StartNextLevel();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        isGameStateIdle = false;
    }

    public void StartIncreasingBridge()
    {
        if (CurrentGameState == GameState.Idle)
            CurrentGameState = GameState.BridgeIncreases;
    }

    public void StopIncreasingBridge()
    {
        if (CurrentGameState == GameState.BridgeIncreases)
            CurrentGameState = GameState.BridgeGoingDown;
    }

    private void StartNextLevel()
    {
        CurrentGameState = GameState.Idle;

        targetPlatform = GetRandomTargetPlatform();
        bonusZoneWidth = targetPlatform.Width / 3;
        bonusZoneX = targetPlatform.X + bonusZoneWidth;

        bridge.Length = 0;
        bridge.Angle = -90;
        hero.X = hero.StartX;
        hero.Y = hero.StartY;
    }

    private Rectangle GetRandomTargetPlatform()
    {
        int randomX = random.Next(startPlatform.X + startPlatform.Width + hero.Size, bridge.MaxLength - hero.Size);
        int randomWidth = random.Next(hero.Size / 2, hero.Size * 2);

        return new Rectangle(randomX, startPlatform.Y, randomWidth, startPlatform.Height);
    }

    private bool IsBridgeHitOnTargetPlatform()
    {
        return bridge.Length >= targetPlatform.X - bridge.X &&
               bridge.Length <= targetPlatform.X + targetPlatform.Width - bridge.X;
    }

    private bool IsBridgeHitInBonusZone()
    {
        return bridge.Length >= bonusZoneX - bridge.X &&
               bridge.Length <= bonusZoneX + bonusZoneWidth - bridge.X;
    }
}