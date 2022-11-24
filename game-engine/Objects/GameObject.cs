using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Objects;

public class GameObject
{
    public int Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool OffScreenTop { get; set; }
    public bool OffScreenSide { get; set; }

    public char Draw()
    {
        return '\u2588';
    }

    public virtual void Move(int x, int y)
    {
 
    }
    public static bool GroundCollision(int y) => y >= Console.WindowHeight - 1;
    public static bool WallCollision(int x) => x > Console.WindowWidth - 1 || x < 0;
    /// <summary>
    /// Still needs a graphic object!!
    /// </summary>
    public GameObject(int id)
    {
        Id = id;
    }
}
