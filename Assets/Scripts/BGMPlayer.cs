using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour
{
    public AudioSource BGM;

    private Music _music;

    // Start is called before the first frame update
    void Start()
    {
        _music = Music.Instance;

        if (!_music.IsPlaying)
        {
            DontDestroyOnLoad(gameObject);
            BGM.Play();
            _music.StartedPlaying();
        }
    }
}
