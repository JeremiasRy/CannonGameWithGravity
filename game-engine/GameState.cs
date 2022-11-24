using GameEngine.Objects;
using GameEngine.GameUtility;

namespace GameEngine;

public static class GameState
{
    public static int Tick { get; private set; }
    public static bool Running { get; private set; }
    public static int ConsecutiveKeyPresses { get; set; } = 0;

    static List<GameObject> _gameObjects = new();
    public static void GameTick() //Call screen buffer here!
    {
        Tick++;
        List<GameObject> _markForDelete = new();
        foreach (GameObject gameObject in _gameObjects)
        {
            if (gameObject is AffectedByForces gravObj)
            {
                gravObj.Move(gravObj.XVelocity(), gravObj.YVelocity());
                ScreenBuffer.DrawText(1, 0, $"GameObj {gameObject.Id}, Yforce: {gravObj.YForce}, Xforce {gravObj.XForce}");
            }
            if (!gameObject.OffScreenTop && !gameObject.OffScreenSide)
            {
                ScreenBuffer.Draw(gameObject.Y, gameObject.X, gameObject.Draw());
            }
            
            ScreenBuffer.DrawText(2, 0, $"X { gameObject.X}, Y { gameObject.Y}");
            ScreenBuffer.DrawText(3, 0, $"Consecutive key presses {ConsecutiveKeyPresses}");
        }
        ScreenBuffer.DrawText(0, 0, $"Current tick {Tick}");
        ScreenBuffer.DrawScreen();
    }
    public static void AddGameObject()
    {
        _gameObjects.Add(new AffectedByForces(1) { X = Console.WindowWidth / 2, Y = Console.WindowHeight - 1, XForce = -12000, YForce = -6000 });
    }
    public static void ApplyUserInput(bool figureitout)
    {
        
    }

    public static void StartGame() => Running = true;
    public static void PauseGame() => Running = false;
}
