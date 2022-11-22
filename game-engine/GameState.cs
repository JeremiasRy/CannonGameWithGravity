using GameEngine.Objects;
using GameEngine.GameUtility;

namespace GameEngine;

public static class GameState
{
    public static int Tick { get; private set; }
    public static bool Running { get; private set; }

    static List<GameObject> _gameObjects = new();
    public static void GameTick() //Call screen buffer here!
    {
        Tick++;
        List<GameObject> _markForDelete = new();
        foreach (GameObject gameObject in _gameObjects)
        {
            gameObject.Move();
            if (!gameObject.OffScreenSide && !gameObject.OffScreenTop)
            {
                ScreenBuffer.Draw(gameObject.Y, gameObject.X, gameObject.Draw());
                ScreenBuffer.DrawText(1, 0, $"GameObj {gameObject.Id}, Yforce: {gameObject.YForce}, Xforce {gameObject.XForce}");
            } else if (gameObject.OffScreenSide)
            {
                _markForDelete.Add(gameObject);
            }
        }
        foreach (var gameObj in _markForDelete)
        {
            _gameObjects.RemoveAt(_gameObjects.IndexOf(gameObj));
        }
        ScreenBuffer.DrawText(0, 0, $"Current tick {Tick}");
        ScreenBuffer.DrawScreen();
    }
    public static void AddGameObject()
    {
        _gameObjects.Add(new GameObject(1, 4000, -11000, 0, Console.WindowHeight - 1));
    }

    public static void StartGame() => Running = true;
    public static void PauseGame() => Running = false;
}
