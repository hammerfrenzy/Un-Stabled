using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpitterOuter : MonoBehaviour
{
    public GameObject[] AnimalTemplates;

    private float _spawnTimerBase = 6f;
    private float _spawnTimerFlux = 2.5f;
    private float _nextSpawnTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _nextSpawnTime = GetNextSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        _nextSpawnTime -= Time.deltaTime;
        if (_nextSpawnTime <= 0)
        {
            YeetAnimalOutOfBarn();
            _nextSpawnTime = GetNextSpawnTime();
        }
    }

    private void YeetAnimalOutOfBarn()
    {
        var index = Random.Range(0, AnimalTemplates.Length);
        var template = AnimalTemplates[index];

        var animalObject = GameObject.Instantiate(template, transform.position, Quaternion.identity);
        var wrangleable = animalObject.GetComponent<IWrangleable>();
        wrangleable.UnStable();
    }

    private float GetNextSpawnTime()
    {
        return _spawnTimerBase + Random.Range(-_spawnTimerFlux, _spawnTimerFlux);
    }
}
