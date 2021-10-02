using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Cowpoke : MonoBehaviour
{
    private CharacterController _controller;
    private SpriteRenderer _renderer;

    [SerializeField]
    private InputActionReference _moveActionReference;

    private float _speed = 6f;
    private float _moveThreshold = 0.2f;
    private Vector2 _direction = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var direction = _moveActionReference.action.ReadValue<Vector2>();
        DoMovement(direction);
    }

    public void DoMovement(Vector2 direction)
    {
        Debug.Log($"got {direction} from input stuff");
        if (direction.magnitude < _moveThreshold) { return; }

        _renderer.flipX = direction.x < 0;

        _controller.Move(direction * _speed * Time.deltaTime);
    }
}
