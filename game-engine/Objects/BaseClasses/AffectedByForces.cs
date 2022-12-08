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
    public const int FORCE_TO_INCREASE_HORIZONTAL_VELOCITY = 500;
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
        List<int[]> pathTaken = GameObject.CalculatePath(X, Y, X + x, Y + y);
        X += x;
        Y += y;
        if (GameObjCollision(pathTaken, out AffectedByForces? collidedObj, out int[]? collisionCoordinates))
        {
            if (collidedObj is null && collisionCoordinates is not null) //Collision with a solid object
            {
                if (Math.Abs(XForce) > FORCE_TO_INCREASE_HORIZONTAL_VELOCITY) 
                    X = XForce < 0 ? collisionCoordinates[0] + 1 : collisionCoordinates[0] - 1;
                if (Math.Abs(YForce) > FORCE_TO_INCREASE_VERTICAL_VELOCITY)
                    Y = YForce < 0 ? collisionCoordinates[1] - 1 : collisionCoordinates[1] + 1;
                
                XForce = ReverseForce(XForce) / 2;
                YForce = ReverseForce(YForce) / 2;
                return;
            }
            else if (collidedObj is not null)
            {
                XForce += collidedObj.XForce < 0 ? collidedObj.XForce - FORCE_TO_INCREASE_HORIZONTAL_VELOCITY : collidedObj.XForce + FORCE_TO_INCREASE_HORIZONTAL_VELOCITY;
                YForce += collidedObj.YForce < 0 ? collidedObj.YForce + FORCE_TO_INCREASE_VERTICAL_VELOCITY : collidedObj.YForce - FORCE_TO_INCREASE_VERTICAL_VELOCITY;
                collidedObj.XForce /= 2;
                collidedObj.YForce /= 2;
            }  
        }
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
    public bool GameObjCollision(List<int[]> path, out AffectedByForces? collisionObj, out int[]? collisionCoordinates)
    {
        collisionObj = null;
        collisionCoordinates = null;
        var mapWithoutMe = GameState.GameObjectsOnMap.Where(arr => arr[^1] != Id);
        if (!mapWithoutMe.Any())
            return false;

        var objectCollision = mapWithoutMe.FirstOrDefault(objectsCoordinates => path.Any(pathCoordinates => objectsCoordinates[0] == pathCoordinates[0] && objectsCoordinates[1] == pathCoordinates[1]));
        if (objectCollision is null)
            return false;

        collisionCoordinates = objectCollision.Take(2).ToArray();
        collisionObj = GameState.FindGameObj(objectCollision[^1]) as AffectedByForces; //Last item of array is objects id

        return true;
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
