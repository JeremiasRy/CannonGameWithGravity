using GameEngine.GameUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Objects;

public class GameObject
{
    public int Id { get; set; }
    /// <summary>
    /// The left position of obj
    /// </summary>
    public int X { get; set; }
    /// <summary>
    /// The top position of obj
    /// </summary>
    public int Y { get; set; }
    public bool OffScreenTop => Y < 0 || Y > Console.WindowHeight - 1;
    public bool OffScreenSide => X < 0 || X + Width > Console.WindowWidth - 1;
    public bool Hidden { get; set; } = false;
    public bool IsSolid { get; set; } = false;

    public int Height => Draw.Length;
    public int Width => Draw[0].Length;

    public string[] Draw { get; private set; }

    public virtual void Move(int x, int y)
    {
        X = x;
        Y = y;
    }
    public static bool SideCollision(int x) => x <= 0 || x >= Console.WindowWidth - 1;
    public static bool GroundCollision(int y) => y >= Console.WindowHeight - 1;

    public List<PositionRef> Positions
    {
        get
        {
            var positions = new List<PositionRef>();
            for (int iy = 0; iy < Height; iy++)
            {
                for (int ix = 0; ix < Width; ix++)
                {
                    var line = Draw[iy].ToCharArray();
                    if (line[ix] != ' ')
                        positions.Add(new PositionRef(new int[] { X + ix, Y + iy}, Id));
                }
            }
            return positions;
        }
    }

    public static void GameObjCollision(GameObject obj1, GameObject obj2)
    {
        if (obj1 is AffectedByForces gravObj1 && obj2 is AffectedByForces gravObj2)
        {
            if (Math.Abs(gravObj1.XForce) > Math.Abs(gravObj2.XForce))
            {
                gravObj2.X += gravObj1.X < gravObj2.X ? 1 : -1;
                gravObj2.XForce = gravObj1.XForce;
                gravObj1.XForce /= 2;
            } else
            {

            }
            if (Math.Abs(gravObj1.YForce) > Math.Abs(gravObj2.YForce) && gravObj1.YForce > AffectedByForces.FORCE_TO_INCREASE_VELOCITY && gravObj2.YForce > AffectedByForces.FORCE_TO_INCREASE_VELOCITY)
            {
                gravObj1.Y = gravObj2.Y - 1;
                gravObj1.X = gravObj2.X + gravObj2.Width / 2;
                gravObj1.YForce = gravObj2.YForce / 2 + AffectedByForces.ReverseForce(gravObj1.YForce / 2);
            } else if (gravObj1.YForce > AffectedByForces.FORCE_TO_INCREASE_VELOCITY && gravObj2.YForce > AffectedByForces.FORCE_TO_INCREASE_VELOCITY)
            {
                gravObj2.Y = gravObj1.Y - 1;
                gravObj2.X = gravObj1.X + gravObj1.Width / 2;
                gravObj2.YForce = gravObj1.YForce / 2 + AffectedByForces.ReverseForce(gravObj2.YForce / 2);
            }
               
        }
    }
    public GameObject(int id, string graphic)
    {
        Id = id;
        Draw = graphic.Split("|");
    }
}
