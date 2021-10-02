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
    }

    // Update is called once per frame
    void Update()
    {
        var wrangleKeyboardPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
        var wrangleGamepadPressed = Gamepad.current.buttonSouth.wasPressedThisFrame;

        if (wrangleKeyboardPressed || wrangleGamepadPressed)
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

        var wrangleLeft = _renderer.flipX;
        // var size = wrangleLeft ? new Vector2(-1.5f, 1.25f) : new Vector2(1.5f, 1.25f);
        // Debug.Log($"Wrangle left? {wrangleLeft}");
        // DrawDebugBox(size);
        var hits = Physics2D.OverlapCircleAll(
            transform.position,
            1.25f,
            LayerMask.GetMask("Animal")
        );

        Debug.Log($"Hit {hits.Length} animals.");

        foreach (var hit in hits)
        {
            var animal = hit.GetComponent<IWrangleable>();
            if (animal == null) continue;
            animal.Wrangle(transform.position);
        }
    }

    /// <summary>
    /// Called by an AnimationEvent at the end of the wrangle animation
    /// </summary>
    private void DoneWranglin()
    {
        _canMove = true;
        _animator.SetBool("IsWrangling", false);
    }

    private void DrawDebugBox(Vector2 size)
    {
        var wrangleLeft = _renderer.flipX;
        var halfHeight = size.y / 2;
        var x = transform.position.x;
        var y = transform.position.y;

        Debug.DrawLine(new Vector3(x, y + halfHeight, 0), new Vector3(x + size.x, y + halfHeight), Color.yellow, 2);
        Debug.DrawLine(new Vector3(x, y - halfHeight, 0), new Vector3(x + size.x, y - halfHeight), Color.yellow, 2);
        Debug.DrawLine(new Vector3(x, y + halfHeight, 0), new Vector3(x, y - halfHeight), Color.yellow, 2);
        Debug.DrawLine(new Vector3(x + size.x, y + halfHeight, 0), new Vector3(x + size.x, y - halfHeight), Color.yellow, 2);
    }
}
