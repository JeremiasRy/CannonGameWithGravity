using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Objects;

public class GameObject
{
    // All this is for gravity
    // Negative forces go up and left positive down and right,
    const int MAX_VELOCITY = 20;
    const int GRAVITY_FORCE = 700;
    const int FRICTION_FORCE = 12;
    const int FORCE_TO_INCREASE_VELOCITY = 1400;
    const int FRICTION_FORCE_GROUND = 600;
    //Gravity ends here
    public int Id { get; set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool OffScreenTop { get; private set; }
    public bool OffScreenSide { get; private set; }

    public int XForce { get; private set; } = 0;
    public int YForce { get; private set; } = 0;

    int XVelocity => CalculateSpeed(XForce);
    int YVelocity => CalculateSpeed(YForce);

    public char Draw()
    {
        return '\u2588';
    }

    public void Move()
    {
        if (XForce != 0)
            ApplyHorizontalForces((YVelocity == 0 && GroundCollision(Y)) ? FRICTION_FORCE + FRICTION_FORCE_GROUND : FRICTION_FORCE);

        ApplyVerticalForces(GRAVITY_FORCE + FRICTION_FORCE);
        X += XVelocity;
        Y = Y + YVelocity > Console.WindowHeight - 1 ? Console.WindowHeight - 1 : Y + YVelocity;
        OffScreenTop = Y < 0;
        OffScreenSide = X > Console.WindowWidth - 1 || X < 0;

        if (GroundCollision(Y) && YForce > FORCE_TO_INCREASE_VELOCITY)
        {
            YForce = 0 - YForce + 500;
        }
    }
    void ApplyHorizontalForces(int frictionAmount)
    {
        if (XForce < 0)
            XForce = XForce + frictionAmount >= 0 ? 0 : XForce + frictionAmount;
        else
            XForce = XForce - frictionAmount <= 0 ? 0 : XForce - frictionAmount;
    }
    void ApplyVerticalForces(int frictionAmount) => YForce += frictionAmount;

    static int CalculateSpeed(int force)
    {
        var speed = Math.Abs(force) / FORCE_TO_INCREASE_VELOCITY;
        speed = speed > MAX_VELOCITY ? MAX_VELOCITY : speed;
        return force < 0 ? 0 - speed : speed;
    }
    static bool GroundCollision(int yPos) => yPos >= Console.WindowHeight - 1;

    /// <summary>
    /// Still needs a graphic object!!
    /// </summary>
    /// <param name="forceHorizontal"></param>
    /// <param name="forceVertical"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public GameObject(int id, int forceHorizontal, int forceVertical, int x, int y)
    {
        Id = id;
        X = x;
        Y = y;
        XForce = forceHorizontal;
        YForce = forceVertical;
    }
}
