using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(WrangledTimer))]
public class AnimalBase : MonoBehaviour, IWrangleable
{
    public float MoveAfterSecondsBase = 3;
    public float SpeedBase = 1;
    public float WrangleSpeed = 4;

    private Rigidbody2D _rigidbody;
    private WrangledTimer _wrangledTimer;
    private float _moveAfterSeconds;
    private float _nextMoveIn;
    private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _wrangledTimer = GetComponent<WrangledTimer>();
        _moveAfterSeconds = MoveAfterSecondsBase + Random.Range(-1f, 1f);
        _nextMoveIn = _moveAfterSeconds;
        _speed = SpeedBase + Random.Range(-0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        _nextMoveIn -= Time.deltaTime;
        if (_nextMoveIn <= 0 && !_wrangledTimer.IsWrangled)
        {
            SquakAround();
            _nextMoveIn = _moveAfterSeconds;
        }

        SpecialAnimalUpdate();
    }

    protected virtual void SpecialAnimalUpdate()
    {

    }

    private void SquakAround()
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
        _wrangledTimer.StartWranglinCountdown();
    }

    public bool IsGettinWrangled()
    {
        return _wrangledTimer.IsWrangled;
    }
}
