using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Stable : MonoBehaviour
{
    private AudioSource _audio;

    private int _chimkins = 0;
    private int _pogs = 0;
    private int _moos = 0;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var type = other.gameObject.tag;

        if (type == "Chimkin")
        {
            _chimkins++;
        }
        else if (type == "Pog")
        {
            _pogs++;
        }
        else if (type == "Moo")
        {
            _moos++;
        }

        Destroy(other.gameObject, 2f);
        _audio.Play();
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("chimkins", _chimkins);
        PlayerPrefs.SetInt("pogs", _pogs);
        PlayerPrefs.SetInt("moos", _moos);
    }
}
