

namespace GameEngine.Objects;

public class CannonShot : AffectedByForces
{
    readonly int _birthday;
    readonly int _forceToReleaseX;
    readonly int _forceToReleaseY;
    readonly List<GameObject> _shadow = new();
    public bool Released { get; set; } = false;

    public void ShotTick()
    {
        if (GameState.Tick - _birthday > 1 && !Released)
        {
            XForce = _forceToReleaseX;
            YForce = _forceToReleaseY;
            Released = true;
        } else
        {
            for (int i = 0; i < _shadow.Count; i++)
            {
                _shadow.ElementAt(i).Move(X - XVelo * i, Y - YVelo * i);
            }
        }

    }

    public CannonShot(int id, string graphic,int x, int y, int xForce, int yForce) : base(id, graphic)
    {
        X = x;
        Y = y;
        _forceToReleaseX = xForce;
        _forceToReleaseY = yForce;
        _birthday = GameState.Tick;
        for (int i = 0; i < Graphics.Shadow.Length; i++)
        {
            _shadow.Add(new GameObject(_birthday + XForce + YForce, Graphics.Shadow[i]) { X = x, Y = y });
            GameState.AddGameObj(_shadow.Last());
        }
    }
}
