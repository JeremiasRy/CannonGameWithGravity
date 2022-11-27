using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Objects;

public class UserControlled : AffectedByForces
{
    public void UserMovement(int amount, bool vertical)
    {
        if (vertical)
        {
            YForce += amount;
        } else
        {
            XForce += amount;
        }
    }

    public UserControlled(int id, string graphics) : base(id, graphics, true)
    {

    }
}
