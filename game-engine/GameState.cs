﻿using GameEngine.Objects;
using GameEngine.GameUtility;

namespace GameEngine;

public static class GameState
{
    public static readonly Random random = new Random();
    public static int Tick { get; private set; }
    public static bool Running { get; private set; }
    public static int ConsecutiveKeyPresses { get; set; } = 0;

    static List<GameObject> _gameObjects = new();
    static readonly UserControlled _player = new(0, Graphics.Tank);
    public static void GameTick() //Call screen buffer here!
    {
        Tick++;
        List<GameObject> _markForDelete = new();
        List<PositionRef> _positions = new();

        _gameObjects.Where(gameObj => gameObj.IsSolid).ToList().ForEach(gameObj => _positions.AddRange(gameObj.Positions));
        HandleCollisions(_positions.GroupBy(pos => pos.XY, new ArrayComparer()));
        _positions.Clear();

        foreach (GameObject gameObject in _gameObjects)
        {
            if (gameObject is AffectedByForces gravObj)
            {
                gravObj.Move(gravObj.XVelocity(), gravObj.YVelocity());

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
                for (int i = 0; i < gameObject.Height; i++)
                {
                    ScreenBuffer.DrawText(gameObject.Y + i, gameObject.X, gameObject.Draw[i]);
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
        ScreenBuffer.DrawText(1, 0, $"User X: {_player.X} Y: {_player.Y} XForce: {_player.XForce} YForce: {_player.YForce}");
        ScreenBuffer.DrawScreen();
    }
    static void HandleCollisions(IEnumerable<IGrouping<int[], PositionRef>> elements)
    {
        foreach (var positionGroupKey in elements)
        {
            if (positionGroupKey.Count() == 2)
            {
                var firstObjCollided = _gameObjects.Find(obj => obj.Id == positionGroupKey.ElementAt(0).Id);
                var secondObjCollided = _gameObjects.Find(obj => obj.Id == positionGroupKey.ElementAt(1).Id);
                if (firstObjCollided is not null && secondObjCollided is not null)
                {
                    GameObject.GameObjCollision(firstObjCollided, secondObjCollided);
                }
                
            }
        };
    }

    public static void ShootCannon()
    {
        _gameObjects.Add(new CannonShot(Tick, Graphics.Shot, _player.X + _player.Width / 2, _player.Y, 2500 - random.Next(5000), -250 * ConsecutiveKeyPresses));
    }
    public static void AddTank()
    {
        _gameObjects.Add(_player);
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
