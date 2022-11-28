using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameUtility;

public class PositionRef
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Id { get; set; }
    public PositionRef(int x, int y, int id)
    {
        X = x;
        Y = y;
        Id = id;
    }
}
