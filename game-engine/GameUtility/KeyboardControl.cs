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
    static extern bool GetKeyboardState(byte lpKeyState);
    [DllImport("user32.dll")]
    static extern bool GetKeyState();

    public static bool KeyboardKeyDown()
    {
        return false;
    }
    public static bool KeyPressed(ConsoleKey key)
    {
        return false;
    }
}
