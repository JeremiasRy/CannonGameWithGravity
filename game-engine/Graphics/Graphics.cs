using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine;

public static class Graphics
{
    public static string Tank { get; set; } = "  \u2584  | \u2588\u2588\u2588 |\u2588\u2588 \u2588\u2588";
    public static string Shot { get; set; } = "\u2588";
    public static string Particle { get {
            string[] _particles = new[] { "#", "&", "^", "*", };
            return _particles[GameState.random.Next(0, _particles.Length - 1)];
        } }
    public static string[] Shadow { get; set; } = new[] { "\u2592", "\u2591" };
    public static string AimCursor { get; set; } = "  \u2588  |-- --|  \u2588  ";
}
