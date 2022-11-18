using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameUtility;

public static class ScreenBuffer
{
    static char[][] _screenArray = new char[Console.WindowHeight][];
    static readonly char _whiteSpace = ' ';

    static public void Initialize()
    {
        for (int i = 0; i < _screenArray.Length; i++)
        {
            _screenArray[i] = new char[Console.WindowWidth];
            for(int j = 0; j < _screenArray[i].Length; j++)
            {
                _screenArray[i][j] = _whiteSpace;
            }
        }
    }
    static void Clear()
    {
        for (int i = 0; i < _screenArray.Length; i++)
        {
            for (int j = 0; j < _screenArray[i].Length; j++)
            {
                _screenArray[i][j] = _whiteSpace;
            }
        }
    }
    public static void Draw(int y, int x, char block) //use zero index! even thought the window is set with 50, 200 real max values are 49,199
    {
        _screenArray[y][x] = block;
    }

    public static void DrawText(int y, int x, string text)
    {
        char[] _charArr = text.ToCharArray();
        for (int i = 0; i < _charArr.Length; i++)
        {
            Draw(y, x + i, _charArr[i]);
        }
    }

    static public void DrawScreen()
    {
        Console.SetCursorPosition(0, 0);
        for (int i = 0; i < _screenArray.Length; i++)
        {
            Console.WriteLine(_screenArray[i]);
        }
        Clear();
        Console.SetCursorPosition(0, 0);
    }
}
