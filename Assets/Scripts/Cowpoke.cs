using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(SpriteRenderer))]
public class Cowpoke : MonoBehaviour
{
    private Animator _animator;
    private CharacterController _controller;
    private SpriteRenderer _renderer;

    [SerializeField]
    private InputActionReference _moveActionReference;
    private InputAction _moveAction;

    [SerializeField]
    private InputActionReference _wrangleActionReference;
    private InputAction _wrangleAction;

    private float _speed = 6f;
    private float _moveThreshold = 0.2f;
    private Vector2 _direction = Vector2.zero;
    private bool _canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _renderer = GetComponent<SpriteRenderer>();

        _moveAction = _moveActionReference.action;
        _wrangleAction = _wrangleActionReference.action;
    }

    // Update is called once per frame
    void Update()
    {
        var shouldWrangle = _wrangleAction.triggered;
        if (shouldWrangle)
        {
            StartWranglin();
        }

        if (!_canMove) return;
        var direction = _moveAction.ReadValue<Vector2>();
        DoMovement(direction);
    }

    public void DoMovement(Vector2 direction)
    {
        if (direction.magnitude < _moveThreshold)
        {
            _animator.SetBool("IsMoving", false);
            return;
        }

        _renderer.flipX = direction.x < 0;
        _controller.Move(direction * _speed * Time.deltaTime);
        _animator.SetBool("IsMoving", true);
    }

    private void StartWranglin()
    {
        _canMove = false;
        _animator.SetBool("IsWrangling", true);
        // Throw a overlap box to see if we hit something
    }

    /// <summary>
    /// Called by an AnimationEvent at the end of the wrangle animation
    /// </summary>
    private void DoneWranglin()
    {
        _canMove = true;
        _animator.SetBool("IsWrangling", false);
    }
}
