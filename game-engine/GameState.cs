using GameEngine.Objects;
using GameEngine.GameUtility;

namespace GameEngine;

public static class GameState
{
    const int GRAVITY_FORCE = 15;
    const int FRICTION_FORCE = 5;
    const int FORCE_TO_MOVE = 7;
    public static int Tick { get; private set; }

    static List<GameObject> _gameObjects = new();
    public static void GameTick()
    {
        Tick++;
        foreach (var gameObject in _gameObjects)
        {
            ScreenBuffer.Draw(gameObject.Y, gameObject.X, gameObject.Draw());
        }
        ScreenBuffer.DrawScreen();
    }
    public static void AddGameObject()
    {
        _gameObjects.Add(new GameObject() { X = 99, Y = 0 });
    }
}
