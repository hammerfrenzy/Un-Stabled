using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreDisplay : MonoBehaviour
{
    public GameObject ChimkinAnimation;
    public GameObject PogAnimation;
    public GameObject MooAnimation;

    public Text ChimkinText;
    public Text PogText;
    public Text MooText;
    public Text TotalText;
    public Text HighScoreText;
    public Text ThanksText;

    private int _chimkinCount = 0;
    private int _pogCount = 0;
    private int _mooCount = 0;
    private int _chimkinMultiplier = 1;
    private int _pogMultiplier = 3;
    private int _mooMultiplier = 5;
    private int _score = 0;
    private int _previousHighScore = 0;

    private float _displayAfterSeconds = 1f;
    private float _displayTimer;
    private float _fadeDuration = 0.25f;

    private bool _hasShownChimkins = false;
    private bool _hasShownPogs = false;
    private bool _hasShownMoos = false;
    private bool _hasShownTotal = false;
    private bool _hasShownHighScore = false;
    private bool _hasShownThanks = false;

    // Start is called before the first frame update
    void Start()
    {
        _displayTimer = _displayAfterSeconds;

        _chimkinCount = PlayerPrefs.GetInt("chimkins", 0);
        _pogCount = PlayerPrefs.GetInt("pogs", 0);
        _mooCount = PlayerPrefs.GetInt("moos", 0);
        _previousHighScore = PlayerPrefs.GetInt("highScore", 0);
        _score = (_chimkinCount * _chimkinMultiplier) +
                 (_pogCount * _pogMultiplier) +
                 (_mooCount * _mooMultiplier);

        if (_score > _previousHighScore)
        {
            PlayerPrefs.SetInt("highScore", _score);
        }

        // Hide everything
        ChimkinAnimation.SetActive(false);
        PogAnimation.SetActive(false);
        MooAnimation.SetActive(false);

        ChimkinText.CrossFadeAlpha(0, 0, true);
        PogText.CrossFadeAlpha(0, 0, true);
        MooText.CrossFadeAlpha(0, 0, true);
        TotalText.CrossFadeAlpha(0, 0, true);
        HighScoreText.CrossFadeAlpha(0, 0, true);
        ThanksText.CrossFadeAlpha(0, 0, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_hasShownThanks)
        {
            var keyboardPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
            var gamepadPressed = Gamepad.current.buttonSouth.wasPressedThisFrame;

            if (keyboardPressed || gamepadPressed)
            {
                SceneManager.LoadScene("WrangleScene", LoadSceneMode.Single);
            }
        }
        else
        {
            _displayTimer -= Time.deltaTime;
            if (_displayTimer <= 0)
            {
                ShowNext();
                _displayTimer = _displayAfterSeconds;
            }
        }
    }

    void ShowNext()
    {
        // Check in reverse order to only choose the most recent
        if (_hasShownHighScore)
        {
            ShowThanks();
        }
        else if (_hasShownTotal)
        {
            ShowHighScore();
        }
        else if (_hasShownMoos)
        {
            ShowTotal();
        }
        else if (_hasShownPogs)
        {
            ShowMoos();
        }
        else if (_hasShownChimkins)
        {
            ShowPogs();
        }
        else
        {
            ShowChimkins();
        }
    }

    private void ShowChimkins()
    {
        ChimkinAnimation.SetActive(true);
        var chimkinPoints = _chimkinCount * _chimkinMultiplier;
        ChimkinText.text = $"chickens: {_chimkinCount}\nx{_chimkinMultiplier} = {chimkinPoints}";
        ChimkinText.CrossFadeAlpha(1, _fadeDuration, false);
        _hasShownChimkins = true;
    }

    private void ShowPogs()
    {
        PogAnimation.SetActive(true);
        var pogPoints = _pogCount * _pogMultiplier;
        PogText.text = $"pigs: {_pogCount}\nx{_pogMultiplier} = {pogPoints}";
        PogText.CrossFadeAlpha(1, _fadeDuration, false);
        _hasShownPogs = true;
    }

    private void ShowMoos()
    {
        MooAnimation.SetActive(true);
        var mooPoints = _mooCount * _mooMultiplier;
        MooText.text = $"cows: {_mooCount}\nx{_mooMultiplier} = {mooPoints}";
        MooText.CrossFadeAlpha(1, _fadeDuration, false);
        _hasShownMoos = true;
    }

    private void ShowTotal()
    {
        TotalText.text = $"total points: {_score}";
        TotalText.CrossFadeAlpha(1, _fadeDuration, false);
        _hasShownTotal = true;
    }

    private void ShowHighScore()
    {
        var isNew = _score > _previousHighScore;
        var modifier = isNew ? "NEW " : "";
        var highScore = isNew ? _score : _previousHighScore;
        HighScoreText.text = $"{modifier}high score: {highScore}";
        HighScoreText.CrossFadeAlpha(1, _fadeDuration, false);
        _hasShownHighScore = true;
    }

    private void ShowThanks()
    {
        ThanksText.CrossFadeAlpha(1, _fadeDuration, false);
        _hasShownThanks = true;
    }
}
