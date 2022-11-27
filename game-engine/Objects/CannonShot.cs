

namespace GameEngine.Objects;

public class CannonShot : AffectedByForces
{
    readonly int _birthday;
    readonly int _forceToReleaseX;
    readonly int _forceToReleaseY;
    public readonly List<GameObject> Shadow = new();
    public bool Released { get; set; } = false;
    private bool _explode = false;
    public bool Explode { get {
            if (!_explode) return GameState.Tick - _birthday > 150; else return _explode; } set { _explode = value; } }

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
                var shdwEl = Shadow[i];
                if (XForce + YForce < FORCE_TO_INCREASE_VELOCITY * 2)
                {
                    shdwEl.Hidden = true;
                } else
                {
                    shdwEl.Hidden = false;
                    shdwEl.Move(X - XVelo * i, Y - YVelo * i);
                }                
            }
        }
    }

    public CannonShot(int id, string graphic,int x, int y, int xForce, int yForce) : base(id, graphic, true)
    {
        X = x;
        Y = y - 2;
        _forceToReleaseX = xForce;
        _forceToReleaseY = yForce;
        _birthday = GameState.Tick;
        for (int i = 0; i < Graphics.Shadow.Length; i++)
        {
            Shadow.Add(new GameObject(_birthday + xForce + yForce + i, Graphics.Shadow[i]) { X = x, Y = y });
            GameState.AddGameObj(Shadow.Last());
        }
    }
}
