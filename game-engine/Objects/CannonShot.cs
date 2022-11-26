

namespace GameEngine.Objects;

public class CannonShot : AffectedByForces
{
    readonly int _birthday;
    readonly int _forceToReleaseX;
    readonly int _forceToReleaseY;
    public readonly List<GameObject> Shadow = new();
    public bool Released { get; set; } = false;
    public bool Explode => GameState.Tick - _birthday > 60;

    public void MoveShadow()
    {
        if (GameState.Tick - _birthday > 1 && !Released)
        {
            XForce = _forceToReleaseX;
            YForce = _forceToReleaseY;
            Released = true;
        } else
        {
            for (int i = 0; i < Shadow.Count; i++)
            {
                Shadow.ElementAt(i).Move(X - XVelo * i, Y - YVelo * i);
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
            Shadow.Add(new GameObject(_birthday + XForce + YForce, Graphics.Shadow[i]) { X = x, Y = y });
            GameState.AddGameObj(Shadow.Last());
        }
    }
}
