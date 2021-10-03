using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(WrangledTimer))]
public class AnimalBase : MonoBehaviour, IWrangleable
{
    public float MoveAfterSecondsBase = 3;
    public float SpeedBase = 1;
    public float WrangleSpeed = 4;

    public GameStart GameStarter;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;
    private WrangledTimer _wrangledTimer;
    private float _moveAfterSeconds;
    private float _nextMoveIn;
    private float _speed;
    private bool _waitForGameStart;

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _wrangledTimer = GetComponent<WrangledTimer>();
        _moveAfterSeconds = MoveAfterSecondsBase + Random.Range(-1f, 1f);
        _nextMoveIn = _moveAfterSeconds;
        _speed = SpeedBase + Random.Range(-0.5f, 0.5f);

        if (GameStarter != null)
        {
            _waitForGameStart = true;
            GameStarter.GameStarted += GameStarted;
        }
    }

    void GameStarted(object sender, System.EventArgs e)
    {
        _waitForGameStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_waitForGameStart) return;

        _renderer.flipX = _rigidbody.velocity.x < 0;

        _nextMoveIn -= Time.deltaTime;
        if (_nextMoveIn <= 0 && !_wrangledTimer.IsWrangled)
        {
            MoveAround();
            _nextMoveIn = _moveAfterSeconds;
        }

        SpecialAnimalUpdate();
    }

    protected virtual void SpecialAnimalUpdate()
    {
        // truffles / cow pies? (time permitting)
    }

    private void MoveAround()
    {
        var dx = Random.Range(-1f, 1f);
        var dy = Random.Range(-1f, 1f);
        var force = new Vector2(dx, dy) * _speed;

        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    public void Wrangle(Vector2 cowpokePosition)
    {
        // Stop whatever the animal is doing at the moment
        _rigidbody.velocity = Vector2.zero;

        // Scare them away from you
        var direction = ((Vector2)transform.position - cowpokePosition).normalized;
        _rigidbody.AddForce(direction * WrangleSpeed, ForceMode2D.Impulse);

        // Set the 'wrangled' status for some amount 
        // of time so they can move to the barn
        _wrangledTimer.StartWranglinCountdown(true);
    }

    public void UnStable()
    {
        // away from barn with some y variance
        var dy = Random.Range(-0.8f, 0.8f);
        var direction = new Vector2(1, dy).normalized;
        var force = GetEscapeVelocity();

        _rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
        _wrangledTimer.StartWranglinCountdown(false);
    }

    public virtual float GetEscapeVelocity()
    {
        return 4;
    }

    public bool IsGettinWrangled()
    {
        return _wrangledTimer.IsWrangled;
    }
}
