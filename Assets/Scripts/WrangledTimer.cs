using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrangledTimer : MonoBehaviour
{
    public float MaxWrangledTime = 2.5f;
    public bool IsWrangled { get; private set; } = false;

    private float _wrangleTimeRemaining = 0;

    public void Update()
    {
        if (!IsWrangled) return;

        _wrangleTimeRemaining -= Time.deltaTime;
        if (_wrangleTimeRemaining <= 0)
        {
            IsWrangled = false;
            gameObject.layer = LayerMask.NameToLayer("Animal");
        }
    }

    public void StartWranglinCountdown(bool changeLayer)
    {
        _wrangleTimeRemaining = MaxWrangledTime;
        IsWrangled = true;

        // if they just spawned we don't want to put them on wrangled layer
        if (changeLayer)
        {
            gameObject.layer = LayerMask.NameToLayer("Wrangled");
        }
    }
}
