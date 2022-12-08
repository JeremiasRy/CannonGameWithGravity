using GameEngine.Objects;
using GameEngine.GameUtility;

namespace GameEngine;

public static class GameState
{
    public static readonly Random random = new Random();
    public static int Tick { get; private set; }
    public static bool Running { get; private set; }
    public static int ConsecutiveKeyPresses { get; set; } = 0;

    /// <summary>
    /// list of array of ints containing in order X position, Y position, Object id
    /// </summary>
    public static List<int[]> GameObjectsOnMap { get 
        {
            List<int[]> result = new();
            foreach (var gameObj in _gameObjects)
            {
                if (!gameObj.IsSolid)
                    continue;

                for (int ix = 0; ix < gameObj.Width; ix++)
                {
                    for (int iy = 0; iy < gameObj.Height; iy++)
                    {
                        if (gameObj.Draw[iy][ix] != ' ')    
                            result.Add(new[] { gameObj.X + ix, gameObj.Y + iy, gameObj.Id });
                    }
                }
            }
            return result;
        } }

    public static GameObject? FindGameObj(int id)
    {
        GameObject? objToRtrn = _gameObjects.FirstOrDefault(obj => obj.Id == id);
        if (objToRtrn is not null)
        {
            return objToRtrn;
        }
        return null;
    }

    static List<GameObject> _gameObjects = new();
    static readonly UserControlled _player = new(0, Graphics.Tank, new AimCursor(1, Graphics.AimCursor, 0));

    public static void GameTick() //Call screen buffer here!
    {
        Tick++;
        List<GameObject> _markForDelete = new();
        int count = 1;
        _player.AimCursorRef.Move(_player.X, _player.Y);
        ScreenBuffer.DrawText(0, Console.WindowWidth / 2, _player.AimCursorRef.ToString());

        foreach (GameObject gameObject in _gameObjects)
        {
            if (gameObject is AffectedByForces gravObj)
            {
                gravObj.Move(gravObj.XVelocity(), gravObj.YVelocity());
                ScreenBuffer.DrawText(count, 0, gravObj.ToString());
                //For debugging and analyzing
                var dirCount = 1;
                foreach (Directions direction in gravObj.Movement)
                {
                    ScreenBuffer.DrawText(count + dirCount, 0, direction.ToString());
                    dirCount++;
                }
                count++;
            }
            if (gameObject is CannonShot shot)
            {
                if (shot.Explode)
                {
                    _markForDelete.Add(shot);
                    foreach (var shadowObj in shot.Shadow)
                    {
                        _markForDelete.Add(shadowObj);
                    }
                    continue;
                }
                shot.MoveShadow();
            }
            if (gameObject is Explosion expParticle)
            {
                if(expParticle.Disappear)
                {
                    _markForDelete.Add(expParticle);
                    continue;
                }
            }
            if (!gameObject.OffScreenTop && !gameObject.OffScreenSide && !gameObject.Hidden)
            {
                for (int iy = 0; iy < gameObject.Draw.Length; iy++)
                {
                    for (int ix = 0; ix < gameObject.Draw[iy].Length; ix++)
                    {
                        if (gameObject.Draw[iy][ix] != ' ')
                            ScreenBuffer.Draw(gameObject.Y + iy, gameObject.X + ix, gameObject.Draw[iy][ix]);
                    }
                    
                }
            }
        }
        if (_markForDelete.Any())
        {
            foreach (GameObject objToDel in _markForDelete)
            {
                if (objToDel is CannonShot shot)
                {
                    AddExplosion(shot.X, shot.Y, shot.Id);
                }
            }
            _gameObjects = _gameObjects.Where(x => !_markForDelete.Contains(x)).ToList();
        }

        ScreenBuffer.DrawText(0, 0, $"Current tick {Tick} ConsecutiveKeyPresses: {ConsecutiveKeyPresses}");
        ScreenBuffer.DrawScreen();
    }

    public static void ShootCannon()
    {
        int totalForce = -500 * ConsecutiveKeyPresses;
        int verticalForce = (int)Math.Round(totalForce * (_player.AimCursorRef.Angle / 90));
        int horizontalForce = (int)Math.Round(totalForce *  (1 - (_player.AimCursorRef.Angle / 90)));
        _gameObjects.Add(new CannonShot(Tick, Graphics.Shot, _player.X, Console.WindowHeight - _player.Height, _player.AimCursorRef.DirectionLeft ? horizontalForce : 0 - horizontalForce, verticalForce));
        _player.XForce += _player.AimCursorRef.DirectionLeft ? 0 - horizontalForce / 2 : horizontalForce / 2; 
        _player.YForce += AffectedByForces.ReverseForce(verticalForce);
    }
    public static void SetupGame()
    {
        _gameObjects.Add(_player);
        _gameObjects.Add(_player.AimCursorRef);
        _gameObjects.Add(new GameObject(2, Graphics.Bucket) { IsSolid = true, X = Console.WindowWidth / 5 * 4, Y = Console.WindowHeight / 3 }); 
    }

    public static void AddGameObj(GameObject obj)
    {
        _gameObjects.Add(obj);
    }
    public static void AddExplosion(int x, int y, int shotId)
    {
        int amount = random.Next(4, 10);
        for (int i = 0; i < amount; i++)
        {
            _gameObjects.Add(new Explosion(Tick * shotId + i, Graphics.Particle, x, y));
        }
    }
    public static void UserInput(UserAction act)
    {
        switch (act)
        {
            case UserAction.Up:
                {
                    _player.AimCursorRef.ChangeAngle(+2);
                } break;
            case UserAction.Down:
                {
                    _player.AimCursorRef.ChangeAngle(-2);
                } break;
            case UserAction.Left:
                {
                    _player.UserMovement(-600, false);
                } break;
            case UserAction.Right:
                {
                    _player.UserMovement(600, false);
                } break;
            case UserAction.Jump:
                {
                    _player.UserMovement(-3000, true);
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
        Down,
        Jump,
        Shoot
    }
}
