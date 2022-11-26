using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Objects;

public class Explosion : AffectedByForces
{
    readonly int _birthday;
    readonly int _deathTime;
    public bool Disappear => GameState.Tick - _birthday > _deathTime;
    public Explosion(int id, string graphic, int x, int y) : base(id, graphic)
    {
        X = x;
        Y = y;
        _birthday = GameState.Tick;
        XForce = 5000 - GameState.random.Next(10000);
        YForce = -3000 - GameState.random.Next(2000);
        _deathTime = GameState.random.Next(30, 60);
    }
}
