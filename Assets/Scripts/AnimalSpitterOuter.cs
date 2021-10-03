using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AnimalSpitterOuter : MonoBehaviour
{
    // These arrays should correlate so we can use the same index
    public GameObject[] AnimalTemplates;
    public AudioClip[] AnimalSounds;
    public GameStart GameStarter;

    private AudioSource _audio;

    private bool _waitForGameStart = true;
    private float _spawnTimerBase = 4f;
    private float _spawnTimerFlux = 1f;
    private float _nextSpawnTime = 0f;
    private bool _letEmLoose = true;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
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
            YeetAnimalsOutOfBarn(2);
            _nextSpawnTime = GetNextSpawnTime();
        }

        if (GameStarter.TimeLeft == 15 && _letEmLoose)
        {
            YeetAnimalsOutOfBarn(10);
            _letEmLoose = false;
        }
    }

    private void YeetAnimalsOutOfBarn(int animalCount)
    {
        for (int x = 0; x < animalCount; x++)
        {
            var index = Random.Range(0, AnimalTemplates.Length);
            var template = AnimalTemplates[index];

            var animalObject = GameObject.Instantiate(template, transform.position, Quaternion.identity);
            animalObject.layer = LayerMask.NameToLayer("Escaping");
            var wrangleable = animalObject.GetComponent<IWrangleable>();
            wrangleable.UnStable();
            StartCoroutine(ReLayerAnimalAfter(animalObject, 1.5f));
        }
    }

    private IEnumerator ReLayerAnimalAfter(GameObject animal, float time)
    {
        yield return new WaitForSeconds(time);
        animal.layer = LayerMask.NameToLayer("Animal");
    }

    private float GetNextSpawnTime()
    {
        return _spawnTimerBase + Random.Range(-_spawnTimerFlux, _spawnTimerFlux);
    }
}
