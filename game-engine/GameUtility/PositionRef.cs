using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameUtility;

public class PositionRef
{
    public int[] XY { get; set; }
    public int Id { get; set; }
    public PositionRef(int[] xy, int id)
    {
        XY = xy;
        Id = id;
    }
}
