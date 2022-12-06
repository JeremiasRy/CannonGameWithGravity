using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Objects;

public class UserControlled : AffectedByForces
{
    public readonly AimCursor AimCursorRef;
    public void UserMovement(int amount, bool jump)
    {
        AimCursorRef.DirectionLeft = amount < 0;
        if (jump)
        {
            YForce += GroundCollision(Y + Height) ? amount : 0;
            return;
        } 
        XForce += amount;
    }
    public void MoveAimCursor()
    {
        AimCursorRef.Move(X, Y);
    }

    public UserControlled(int id, string graphics, AimCursor aimCursorRef) : base(id, graphics, true)
    {
        AimCursorRef = aimCursorRef;
    }
}
