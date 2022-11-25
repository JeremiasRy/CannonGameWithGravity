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
        if (GroundCollision(Y + Height))
            ApplyHorizontalForces(FRICTION_FORCE + FRICTION_FORCE_GROUND);
        else
            ApplyHorizontalForces(FRICTION_FORCE);

        return CalculateSpeed(XForce);
    }
    public int YVelocity()
    {
        ApplyVerticalForces(GRAVITY_FORCE + FRICTION_FORCE);
        return CalculateSpeed(YForce);
    }

    public override void Move(int x, int y)
    {
        X += x;
        Y += y;

        if ((SideCollision(X) || SideCollision(X + Width)))
        {
            X = X < Console.WindowWidth / 2 ? 0 : Console.WindowWidth - 1 - Width;
            XForce = Math.Abs(XForce) > FORCE_TO_INCREASE_VELOCITY ? ReverseForce(XForce) / 2 : XForce;
        }
        if (GroundCollision(Y + Height) && !(YForce < 0))
        {
            Y = Console.WindowHeight - 1 - Height;
            YForce = ReverseForce(YForce) / 2;
        } 
        OffScreenTop = Y < 0;
        OffScreenSide = X < 0 || X + Width > Console.WindowWidth - 1;
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

    public AffectedByForces(int id, string graphics) : base(id, graphics)
    {
        
    }
}
