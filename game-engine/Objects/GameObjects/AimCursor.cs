using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Objects;

public class AimCursor : GameObject
{
    public bool DirectionLeft { get; set; }
    public double Angle { get; private set; } = 45;
    readonly int _radius = 15;
    
    public void ChangeAngle(int angle)
    {
        if (angle < 0)
            Angle = Angle + angle < 0 ? 0 : Angle + angle;
        else
            Angle = Angle + angle > 90 ? 90 : Angle + angle; 
    }
    public override void Move(int x, int y)
    {
        double radians = (DirectionLeft ? 180 + Angle : 360 - Angle) * Math.PI / 180;
        base.Move((int)Math.Round(_radius * Math.Cos(radians) + x), (int)Math.Round(_radius * Math.Sin(radians) + y));
    }
    public override string ToString() => $"Angle: {Angle}, X: {X}, Y: {Y}";
    public AimCursor(int id, string graphic) : base(id, graphic)
    {
    }
}
