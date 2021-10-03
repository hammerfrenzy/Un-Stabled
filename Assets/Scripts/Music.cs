using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music
{
    private static Music _music;
    public static Music Instance
    {
        get
        {
            if (_music == null)
            {
                _music = new Music();
            }
            return _music;
        }
    }

    public bool IsPlaying { get { return _isPlayingAlready; } }

    public void StartedPlaying()
    {
        _isPlayingAlready = true;
    }

    private bool _isPlayingAlready = false;
}
