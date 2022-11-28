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
                        positions.Add(new PositionRef(X + ix, Y + iy, Id));
                }
            }
            return positions;
        }
    }
    public GameObject(int id, string graphic)
    {
        Id = id;
        Draw = graphic.Split("|");
    }
}
