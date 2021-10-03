using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public Text TitleText;
    public Text ControlsText;
    public Text TimeText;

    public event EventHandler GameStarted;

    private Camera _camera;
    private bool _gameStarted = false;
    private bool _startCountdown = false;
    private float _secondsLeft = 5;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        TimeText.CrossFadeAlpha(0, 0, true);
    }

    void Update()
    {
        if (_gameStarted)
        {
            if (_startCountdown)
            {
                _secondsLeft -= Time.deltaTime;
                TimeText.text = $"Time: {(int)_secondsLeft}";
                if (_secondsLeft <= 0)
                {
                    EndGame();
                }
            }
        }
        else
        {
            CheckForGameStart();
        }
    }

    void CheckForGameStart()
    {
        var keyboardPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
        var gamepadPressed = Gamepad.current.buttonSouth.wasPressedThisFrame;

        if (keyboardPressed || gamepadPressed)
        {
            _gameStarted = true;
            StartCoroutine(MoveCameraOver());
        }
    }

    void EndGame()
    {
        SceneManager.LoadScene("ScoreScene", LoadSceneMode.Single);
    }

    IEnumerator MoveCameraOver()
    {
        var totalTime = 2f;
        var timeSpent = 0f;
        var startTime = Time.time;

        var startPos = _camera.gameObject.transform.position;
        var endPos = new Vector3(4.5f, startPos.y, startPos.z);

        // Fade Title & Controls out
        TitleText.CrossFadeAlpha(0, totalTime, false);
        ControlsText.CrossFadeAlpha(0, totalTime, false);

        // Fade Game UI in
        TimeText.CrossFadeAlpha(1, totalTime, false);

        // Lerp Camera
        while (timeSpent < totalTime)
        {
            var percent = (float)timeSpent / totalTime;
            var newCameraPosition = Vector3.Lerp(startPos, endPos, percent);

            _camera.gameObject.transform.position = newCameraPosition;

            timeSpent += Time.deltaTime;
            yield return null;
        }

        // Start the game
        _camera.gameObject.transform.position = endPos;
        _startCountdown = true;

        GameStarted?.Invoke(this, EventArgs.Empty);
    }
}
