using UnityEngine;

public class TelecomSpawner : MonoBehaviour
{
    public GameObject signalWavePrefab;

    public float spawnRate = 1f;
    public float signalSpeed = 8f;

    private float timer;

    public bool canSpawn = false;
    public bool blueSoulPattern = false;

    void Update()
    {
        if (!canSpawn)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnSignal();
            timer = 0;
        }
    }

    public void StartSpawning()
    {
        canSpawn = true;
        timer = 0;
    }

    public void StopSpawning()
    {
        canSpawn = false;
    }

    public void SetDifficulty(float newSpawnRate, float newSignalSpeed)
    {
        spawnRate = newSpawnRate;
        signalSpeed = newSignalSpeed;
    }

    public void SetBlueSoulPattern(bool active)
    {
        blueSoulPattern = active;
    }

    void SpawnSignal()
    {
        float spawnY;

        if (blueSoulPattern)
        {
            spawnY = -2f; // señal baja para saltar
        }
        else
        {
            spawnY = Random.Range(-2f, 2f);
        }

        Vector2 spawnPos = new Vector2(5f, spawnY);

        GameObject wave = Instantiate(
            signalWavePrefab,
            spawnPos,
            Quaternion.identity
        );

        SignalWave signal = wave.GetComponent<SignalWave>();

        if (signal != null)
        {
            signal.speed = signalSpeed;
            signal.SetDirection(Vector2.left);
        }
    }
}