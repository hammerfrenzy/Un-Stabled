using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(SpriteRenderer))]
public class Cowpoke : MonoBehaviour
{
    public GameStart GameStarter;
    public AudioClip[] WrangleSounds;

    private Animator _animator;
    private AudioSource _audio;
    private CharacterController _controller;
    private SpriteRenderer _renderer;

    [SerializeField]
    private InputActionReference _moveActionReference;
    private InputAction _moveAction;

    private float _speed = 6f;
    private float _moveThreshold = 0.2f;
    private Vector2 _direction = Vector2.zero;
    private bool _canMove = true;
    private bool _waitForGameStart = true;

    // Start is called before the first frame update
    void Start()
    {
        GameStarter.GameStarted += GameStarted;

        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _controller = GetComponent<CharacterController>();
        _renderer = GetComponent<SpriteRenderer>();

        _moveAction = _moveActionReference.action;
    }

    void GameStarted(object sender, System.EventArgs e)
    {
        _waitForGameStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_waitForGameStart) return;

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
        var hits = Physics2D.OverlapCircleAll(
            transform.position,
            1.25f,
            LayerMask.GetMask("Animal", "Wrangled")
        );

        foreach (var hit in hits)
        {
            var animal = hit.GetComponent<IWrangleable>();
            if (animal == null) continue;
            animal.Wrangle(transform.position);
        }

        var index = Random.Range(0, WrangleSounds.Length);
        var sfx = WrangleSounds[index];
        _audio.PlayOneShot(sfx, 0.3f);
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
