using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Chimkin : MonoBehaviour
{
    [Range(2, 10)]
    public float MoveAfterSecondsBase = 3;

    [Range(15, 50)]
    public float Speed = 20;

    private Rigidbody2D _rigidbody;
    private float _moveAfterSeconds;
    private float _nextMoveIn;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _moveAfterSeconds = MoveAfterSecondsBase + Random.Range(-1f, 1f);
        _nextMoveIn = _moveAfterSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        _nextMoveIn -= Time.deltaTime;
        if (_nextMoveIn <= 0)
        {
            SquakAround();
            _nextMoveIn = _moveAfterSeconds;
        }
    }

    private void SquakAround()
    {
        var dx = Random.Range(-1f, 1f);
        var dy = Random.Range(-1f, 1f);
        var force = new Vector2(dx, dy) * Speed;

        _rigidbody.AddForce(force);
    }
}
