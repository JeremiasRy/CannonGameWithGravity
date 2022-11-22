using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace GameEngine.GameUtility;

public static class KeyboardControl
{
    [DllImport("user32.dll")]
    static extern bool GetKeyboardState(byte[] lpKeyState);
    [DllImport("user32.dll")]
    static extern short GetKeyState();

    static readonly byte[] _array = new byte[256];
    public static bool KeyboardKeyDown(out List<ConsoleKey> keys)
    {
        keys = new();
        GetKeyState();
        GetKeyboardState(_array);
        if (!_array.Any(key => key != 0 && key != 1))
        {
            return false;
        } else
        {
            for (int i = 0; i < _array.Length; i++)
            {
                if (_array[i] != 0 && _array[i] != 1)
                {
                    keys.Add((ConsoleKey)i);
                }
            }
            return true;
        }   
    }
}
