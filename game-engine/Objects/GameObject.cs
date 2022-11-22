using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Objects;

public class GameObject
{
    // All this is for gravity
    const int MAX_VELOCITY = 5;
    const int GRAVITY_FORCE = 15;
    const int FRICTION_FORCE = 1;
    const int FORCE_TO_INCREASE_VELOCITY = 50;
    //Gravity ends here
    public int X { get; private set; }
    public int Y { get; private set; }

    int XForce { get; set; } = 0;
    int YForce { get; set; } = 0;

    int XVelocity => CalculateSpeed(XForce);
    int YVelocity => CalculateSpeed(YForce);

    public char Draw()
    {
        return '\u2588';
    }

    public void Move()
    {
        if (XVelocity != 0)
            ApplyHorizontalForces();

        YForce += GRAVITY_FORCE + FRICTION_FORCE;
        X += XVelocity;
        Y += YVelocity;
    }
    void ApplyHorizontalForces()
    {
        if (XVelocity < 0)
            XForce = XForce + FRICTION_FORCE > 0 ? 0 : XForce + FRICTION_FORCE;
        else
            XForce = XForce - FRICTION_FORCE < 0 ? 0 : XForce - FRICTION_FORCE;
    }

    static int CalculateSpeed(int force)
    {
        var speed = Math.Abs(force) / FORCE_TO_INCREASE_VELOCITY;
        speed = speed > MAX_VELOCITY ? MAX_VELOCITY : speed;
        return force < 0 ? 0 - speed : speed;
    }

    /// <summary>
    /// Still needs a graphic object!!
    /// </summary>
    /// <param name="forceHorizontal"></param>
    /// <param name="forceVertical"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public GameObject(int forceHorizontal, int forceVertical, int x, int y)
    {
        X = x;
        Y = y;
        XForce = forceHorizontal;
        YForce = forceVertical;
    }
}
