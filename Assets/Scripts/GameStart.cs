using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameStart : MonoBehaviour
{
    public Text TitleText;
    public Text ControlsText;

    private Camera _camera;
    private bool _gameStarted = false;


    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (_gameStarted) return;

        var keyboardPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
        var gamepadPressed = Gamepad.current.buttonSouth.wasPressedThisFrame;

        if (keyboardPressed || gamepadPressed)
        {
            _gameStarted = true;
            StartCoroutine(MoveCameraOver());
        }
    }

    IEnumerator MoveCameraOver()
    {
        var totalTime = 2f;
        var timeSpent = 0f;
        var startTime = Time.time;

        var startPos = _camera.gameObject.transform.position;
        var endPos = new Vector3(4.5f, 0, startPos.z);

        TitleText.CrossFadeAlpha(0, totalTime, false);
        ControlsText.CrossFadeAlpha(0, totalTime, false);

        while (timeSpent < totalTime)
        {
            var percent = (float)timeSpent / totalTime;
            var newCameraPosition = Vector3.Lerp(startPos, endPos, percent);

            _camera.gameObject.transform.position = newCameraPosition;



            timeSpent += Time.deltaTime;
            yield return null;
        }


        _camera.gameObject.transform.position = endPos;
    }
}
