using GameEngine.Objects;
using GameEngine.GameUtility;

namespace GameEngine;

public static class GameState
{
    const int GRAVITY_FORCE = 15;
    const int FRICTION_FORCE = 5;
    const int FORCE_TO_MOVE = 7;
    public static int Tick { get; private set; }
    public static bool Running { get; private set; }

    static List<GameObject> _gameObjects = new();
    public static void GameTick() //Call screen buffer here!
    {
        Tick++;
        ScreenBuffer.DrawText(0, 0, $"Current tick {Tick}");
        ScreenBuffer.DrawScreen();
    }
    public static void AddGameObject()
    {
        _gameObjects.Add(new GameObject() { X = 99, Y = 0 });
    }

    public static void StartGame() => Running = true;
    public static void PauseGame() => Running = false;
}
