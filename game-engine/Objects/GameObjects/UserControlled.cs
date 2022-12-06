using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Objects;

public class UserControlled : AffectedByForces
{
    readonly AimCursor _aimCursorRef;
    public void UserMovement(int amount)
    {
        _aimCursorRef.DirectionLeft = amount < 0;
        XForce += amount;
    }
    public void MoveAimCursor()
    {
        _aimCursorRef.Move(X, Y);
    }

    public UserControlled(int id, string graphics, AimCursor aimCursorRef) : base(id, graphics, true)
    {
        _aimCursorRef = aimCursorRef;
    }
}
