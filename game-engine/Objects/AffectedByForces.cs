using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Objects;

public class AffectedByForces : GameObject
{
    // All this is for gravity
    // Negative forces go up and left positive down and right,
    const int MAX_VELOCITY = 12;
    const int GRAVITY_FORCE = 600;
    const int FRICTION_FORCE = 12;
    const int FORCE_TO_INCREASE_VELOCITY = 800;
    const int FRICTION_FORCE_GROUND = 550;
    //Gravity ends here
    public int XForce { get; set; } = 0;
    public int YForce { get; set; } = 0;

    public int XVelocity()
    {
        if (XForce != 0)
            ApplyHorizontalForces(GroundCollision(Y) ? FRICTION_FORCE + FRICTION_FORCE_GROUND : FRICTION_FORCE);
        
        return CalculateSpeed(XForce);
    }
    public int YVelocity()
    {
        if (GroundCollision(Y) && YForce > FORCE_TO_INCREASE_VELOCITY)
            YForce = ReverseForce(YForce) / 2;
        ApplyVerticalForces(GRAVITY_FORCE + FRICTION_FORCE);
        return CalculateSpeed(YForce);
    }

    public override void Move(int x, int y)
    {
        if (X + x > Console.WindowWidth - 1)
        {
            var spillOver = X + x - (Console.WindowWidth - 1);
            X = Console.WindowWidth - 1;
            XForce = ReverseForce(XForce);
        } else if (X + x < 0)
        {
            X = 0;
            XForce = ReverseForce(XForce);
        } else
        {
            X += x;
        }
        Y = Y + y > Console.WindowHeight - 1 ? Console.WindowHeight - 1 : Y + y;
        OffScreenTop = Y < 0;
        OffScreenSide = X < 0 || X > Console.WindowWidth - 1;
    }
    void ApplyHorizontalForces(int frictionAmount)
    {
        if (XForce < 0)
            XForce = XForce + frictionAmount > 0 ? 0 : XForce + frictionAmount;
        else
            XForce = XForce - frictionAmount < 0 ? 0 : XForce - frictionAmount;
    }
    void ApplyVerticalForces(int frictionAmount)
    {
        if (YForce < 0 && XForce > YForce)
        {
            YForce = YForce + frictionAmount - Math.Abs(XForce) / 40;
        } else
        {
            YForce += frictionAmount;
        }
        
    } 

    static int CalculateSpeed(int force)
    {
        var speed = Math.Abs(force) / FORCE_TO_INCREASE_VELOCITY;
        speed = speed > MAX_VELOCITY ? MAX_VELOCITY : speed;
        return force < 0 ? 0 - speed : speed;
    }

    static int ReverseForce(int force) => force < 0 ? Math.Abs(force) : 0 - force;

    public AffectedByForces(int id) : base(id)
    {
        
    }
}
