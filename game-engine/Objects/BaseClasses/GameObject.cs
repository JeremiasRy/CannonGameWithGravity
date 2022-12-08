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
    public bool OffScreenTop => Y < 0 || Y + Height -1 > Console.WindowHeight - 1;
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
    public static List<int[]> CalculatePath(int startX, int startY, int endX, int endY)
    {
        List<int[]> path = new();
        bool left = startX > endX;
        bool up = startY > endY;
        float xDifference = left ? startX - endX : endX - startX;
        int yDifference = up ? startY - endY : endY - startY;
        if (xDifference == 0)
        {
            for (int i = 0; i < yDifference; i++)
            {
                path.Add(new int[] { startX, startY + i });
            }
            return path;
        } else if (yDifference == 0)
        {
            for (int i = 0; i < xDifference; i++)
            {
                path.Add(new int[] { startX + i, startY});
            }
            return path;
        } else if (xDifference > yDifference || xDifference == yDifference)
        {
            for (float x = 0; x < xDifference; x++)
            {
                float percent = x / xDifference;
                int y = (int)Math.Round(up ? startY - yDifference * percent : startY + yDifference * percent);
                path.Add(new int[] { (int)(left ? startX - x : startX + x), y });
            }
            return path;
        } else
        {
            for (float y = 0; y < yDifference; y++)
            {
                float percent = y / yDifference;
                int x = (int)Math.Round(left ? startX - xDifference * percent : startX + xDifference * percent);
                path.Add(new int[] { x, (int)(up ? startY - y : startY + y) });
            }
            return path;
        }
    }

    public GameObject(int id, string graphic)
    {
        Id = id;
        Draw = graphic.Split("|");
    }
}
