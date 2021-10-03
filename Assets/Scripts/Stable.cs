using UnityEngine;
using UnityEngine.SceneManagement;

public class Stable : MonoBehaviour
{
    private int _chimkins = 0;
    private int _pogs = 0;
    private int _moos = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        var type = other.gameObject.tag;
        Debug.Log(LayerMask.LayerToName(other.gameObject.layer));
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
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("chimkins", _chimkins);
        PlayerPrefs.SetInt("pogs", _pogs);
        PlayerPrefs.SetInt("moos", _moos);
    }
}
