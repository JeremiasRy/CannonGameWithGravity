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
        foreach (GameObject gameObject in _gameObjects)
        {
            gameObject.Move();
            ScreenBuffer.Draw(gameObject.Y, gameObject.X, gameObject.Draw());
        }
        ScreenBuffer.DrawText(0, 0, $"Current tick {Tick}");
        ScreenBuffer.DrawScreen();
    }
    public static void AddGameObject()
    {
        _gameObjects.Add(new GameObject(500, -300, 0, Console.WindowHeight - 1));
    }

    public static void StartGame() => Running = true;
    public static void PauseGame() => Running = false;
}
