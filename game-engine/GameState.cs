using GameEngine.Objects;
using GameEngine.GameUtility;

namespace GameEngine;

public static class GameState
{
    static Random random = new Random();
    public static int Tick { get; private set; }
    public static bool Running { get; private set; }
    public static int ConsecutiveKeyPresses { get; set; } = 0;

    static List<GameObject> _gameObjects = new();
    static readonly UserControlled _player = new(0, Graphics.Tank);
    public static void GameTick() //Call screen buffer here!
    {
        Tick++;
        List<GameObject> _markForDelete = new();
        foreach (GameObject gameObject in _gameObjects)
        {
            if (gameObject is AffectedByForces gravObj)
            {
                gravObj.Move(gravObj.XVelocity(), gravObj.YVelocity());
            }
            if (gameObject is CannonShot shot)
            {
                shot.ShotTick();
            }
            if (!gameObject.OffScreenTop && !gameObject.OffScreenSide)
            {
                for (int i = 0; i < gameObject.Height; i++)
                {
                    ScreenBuffer.DrawText(gameObject.Y + i, gameObject.X, gameObject.Draw[i]);
                }
                
            }
        }
        ScreenBuffer.DrawText(0, 0, $"Current tick {Tick}");
        ScreenBuffer.DrawText(1, 0, $"User X: {_player.X} Y: {_player.Y} XForce: {_player.XForce} YForce: {_player.YForce}");
        ScreenBuffer.DrawScreen();
    }
    public static void ShootCannon()
    {
        var shot = new CannonShot(Tick, Graphics.Particle, _player.X + _player.Width / 2, _player.Y, 5000 - random.Next(10000), -5000 - random.Next(2000));
        _gameObjects.Add(shot);
    }
    public static void AddTank()
    {
        _gameObjects.Add(_player);
    }

    public static void AddGameObj(GameObject obj)
    {
        _gameObjects.Add(obj);
    }
    public static void MoveTank(UserAction act)
    {
        switch (act)
        {
            case UserAction.Up:
                {
                    _player.UserMovement(-800, true);
                } break;
            case UserAction.Left:
                {
                    _player.UserMovement(-800, false);
                } break;
            case UserAction.Right:
                {
                    _player.UserMovement(800, false);
                } break;
            case UserAction.Shoot:
                {
                    ShootCannon();
                } break;
        }
    }


    public static void StartGame() => Running = true;
    public static void PauseGame() => Running = false;

    public enum UserAction
    {
        Left,
        Right,
        Up,
        Shoot
    }
}
