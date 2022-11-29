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
    public const int MAX_VELOCITY = 12;
    public const int GRAVITY_FORCE = 600;
    public const int FRICTION_FORCE = 12;
    public const int FORCE_TO_INCREASE_VERTICAL_VELOCITY = 800;
    public const int FORCE_TO_INCREASE_HORIZONTAL_VELOCITY = 300;
    public const int FRICTION_FORCE_GROUND = 500;
    //Gravity ends here
    public int XForce { get; set; } = 0;
    public int YForce { get; set; } = 0;
    public int XSpeed { get; private set; }
    public int YSpeed { get; private set; }

    public List<Directions> Movement { get 
        {
            var list = new List<Directions>();
            if (Math.Abs(XForce) < FORCE_TO_INCREASE_HORIZONTAL_VELOCITY && Math.Abs(YForce) < FORCE_TO_INCREASE_VERTICAL_VELOCITY)
            {
                list.Add(Directions.Stationary);
                return list;
            }
            if (Math.Abs(XForce) > FORCE_TO_INCREASE_HORIZONTAL_VELOCITY)
                if (XForce < 0)
                    list.Add(Directions.Left);
                else
                    list.Add(Directions.Right);
            if (Math.Abs(YForce) > FORCE_TO_INCREASE_VERTICAL_VELOCITY)
                if (YForce < 0)
                    list.Add(Directions.Up);
                else
                    list.Add(Directions.Down);

            return list;

        } }
    public int XVelocity()
    {
        if (GroundCollision(Y + Height))
            ApplyHorizontalForces(FRICTION_FORCE + FRICTION_FORCE_GROUND);
        else
            ApplyHorizontalForces(FRICTION_FORCE);

        XSpeed = CalculateSpeed(XForce, false);
        return XSpeed;
    }
    public int YVelocity()
    {
        ApplyVerticalForces(GRAVITY_FORCE + FRICTION_FORCE);
        YSpeed = CalculateSpeed(YForce, true);
        return YSpeed;
    }

    public override void Move(int x, int y)
    {
        X += x;
        Y += y;

        if ((SideCollision(X) || SideCollision(X + Width)))
        {
            X = X < Console.WindowWidth / 2 ? 0 : Console.WindowWidth - 1 - Width;
            XForce = Math.Abs(XForce) > FORCE_TO_INCREASE_HORIZONTAL_VELOCITY ? ReverseForce(XForce) / 2 : XForce;
        }
        if (GroundCollision(Y + Height) && !(YForce < 0))
        {
            Y = Console.WindowHeight - Height;
            YForce = ReverseForce(YForce) / 2;
        } 
        if (GameObjCollision())
        {
            var jausers = "jausers";
        }
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

    public static int CalculateSpeed(int force, bool vertical)
    {
        var speed = Math.Abs(force) / (vertical ? FORCE_TO_INCREASE_VERTICAL_VELOCITY : FORCE_TO_INCREASE_HORIZONTAL_VELOCITY);
        speed = speed > MAX_VELOCITY ? MAX_VELOCITY : speed;
        return force < 0 ? 0 - speed : speed;
    }
    public bool GameObjCollision()
    {
        var mapWithoutMe = GameState.GameMap().Where(posRef => posRef.Id != Id);
        foreach (var position in mapWithoutMe)
        {
            for (int i = 0; i < Positions.Count; i++)
            {
                var nextTickPos = Positions[i].X + XSpeed;
                if ((position.X + 1 == Positions[i].X && position.Y == Positions[i].Y) || (position.X - 1 == Positions[i].X && position.Y == Positions[i].Y))
                {
                    var gameObjCollided = GameState.FindGameObj(position.Id);
                    if (gameObjCollided != null && gameObjCollided is AffectedByForces gravObj)
                    {
                        if (Math.Abs(gravObj.XForce) > Math.Abs(XForce)) //Handle horizontal forces
                        {
                            X += X <= gravObj.X ? -1 : 1;
                            XForce = gravObj.XForce;
                            gravObj.XForce /= 2;

                        }
                        else
                        {
                            gravObj.X += gravObj.X <= X ? -1 : 1;
                            gravObj.XForce = XForce;
                            XForce /= 2;
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }

    public static int ReverseForce(int force) => force < 0 ? Math.Abs(force) : 0 - force;
    public override string ToString() => $"Id: {Id}, X: {X}, Y: {Y}, X force: {XForce}, Y force: {YForce}";


    public AffectedByForces(int id, string graphics, bool solid) : base(id, graphics)
    {
        IsSolid = solid;
    }
}

public enum Directions
{
    Up,
    Down,
    Left,
    Right,
    Stationary,
}
