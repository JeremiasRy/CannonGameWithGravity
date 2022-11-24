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
    const int MAX_VELOCITY = 4;
    const int GRAVITY_FORCE = 150;
    const int FRICTION_FORCE = 12;
    const int FORCE_TO_INCREASE_VELOCITY = 150;
    const int FRICTION_FORCE_GROUND = 75;
    //Gravity ends here
    public int XForce { get; set; } = 0;
    public int YForce { get; set; } = 0;

    public int XVelocity()
    {
        if (XForce != 0)
            ApplyHorizontalForces((YForce < FORCE_TO_INCREASE_VELOCITY && GroundCollision(Y)) ? FRICTION_FORCE + FRICTION_FORCE_GROUND : FRICTION_FORCE);

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
        Y = Y + y > Console.WindowHeight - 1 ? Console.WindowHeight - 1 : Y + y;
        OffScreenTop = Y < 0;
        OffScreenSide = X > Console.WindowWidth - 1 || X < 0;

        if (GroundCollision(Y) && YForce > FORCE_TO_INCREASE_VELOCITY)
        {
            YForce = 0 - YForce / 2;
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
    static bool WallCollision(int x) => x >= Console.WindowWidth || x <= 0; 

    public AffectedByForces(int id) : base(id)
    {
        
    }
}
