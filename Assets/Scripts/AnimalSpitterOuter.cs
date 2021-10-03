using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpitterOuter : MonoBehaviour
{
    public GameObject[] AnimalTemplates;
    public GameStart GameStarter;

    private bool _waitForGameStart = true;
    private float _spawnTimerBase = 5f;
    private float _spawnTimerFlux = 2f;
    private float _nextSpawnTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _nextSpawnTime = GetNextSpawnTime();

        GameStarter.GameStarted += GameStarted;
    }

    void GameStarted(object sender, System.EventArgs e)
    {
        _waitForGameStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_waitForGameStart) return;

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
