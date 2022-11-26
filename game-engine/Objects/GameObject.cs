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
    public bool OffScreenTop { get; set; }
    public bool OffScreenSide { get; set; }

    public int Height => Draw.Length;
    public int Width => Draw[0].Length;

    public string[] Draw { get; private set; }

    public virtual void Move(int x, int y)
    {
        X += x;
        Y += y;
    }
    public static bool SideCollision(int x) => x <= 0 || x >= Console.WindowWidth - 1;
    public static bool GroundCollision(int y) => y >= Console.WindowHeight - 1;

    public bool Collision(int x, int y)
    {
        for (int ix = X; ix < X + Width; x++)
        {
            for (int iy = Y; iy < Y + Height; y++)
            {
                if (x == ix && y == iy)
                    return true;
            }
        }
        return false;
    }
    public GameObject(int id, string graphic)
    {
        Id = id;
        Draw = graphic.Split("|");
    }
}
