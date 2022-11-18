using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Objects;

public class GameObject
{
    public int X { get; set; }
    public int Y { get; set; }

    public int XForce { get; private set; }
    public int YForce { get; private set; }

    public char Draw()
    {
        return '\u2588';
    }
}
