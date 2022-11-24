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


    /// <summary>
    /// Still needs a graphic object!!
    /// </summary>
    /// <param name="forceHorizontal"></param>
    /// <param name="forceVertical"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public GameObject(int id)
    {
        Id = id;
    }
}
