using UnityEngine;

public class TelecomSpawner : BaseAttackSpawner
{
    public GameObject signalWavePrefab;
    public GameObject interferenceZonePrefab;
    public GameObject dataPulsePrefab;

    public float spawnRate = 1f;
    public float signalSpeed = 8f;

    private float timer;

    public override void StartSpawning()
    {
        canSpawn = true;
        timer = 0;
    }

    public override void StopSpawning()
    {
        canSpawn = false;
    }

    public override void SetDifficulty(float newSpawnRate, float newSignalSpeed)
    {
        spawnRate = newSpawnRate;
        signalSpeed = newSignalSpeed;
    }
    void Update()
    {
        if (!canSpawn)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnAttack();
            timer = 0;
        }
    }

    void SpawnAttack()
    {
        if (phase == 1)
        {
            SpawnSignal();
        }
        else if (phase == 2)
        {
            int randomAttack = Random.Range(0, 2);

            if (randomAttack == 0)
                SpawnSignal();
            else
                SpawnInterferenceZone();
        }
        else if (phase == 3)
        {
            int randomAttack =
            Random.Range(0, 3);

            if (randomAttack == 0)
                SpawnSignal();

            else if (randomAttack == 1)
                SpawnInterferenceZone();

            else
                SpawnDataPulse();
        }
    }

    public override void SetBlueSoulPattern(bool active)
    {
        blueSoulPattern = active;
    }

    public override void SetPhase(int newPhase)
    {
        phase = newPhase;
    }

    void SpawnSignal()
    {
        float spawnY;

        if (blueSoulPattern)
        {
            spawnY = -2f;
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

    void SpawnInterferenceZone()
    {
        if (interferenceZonePrefab == null)
        {
            Debug.LogError("Falta asignar Interference Zone Prefab en TelecomSpawner");
            return;
        }

        Vector2[] corners =
        {
            new Vector2(-3f, 2f),
            new Vector2(3f, 2f),
            new Vector2(-3f, -2f),
            new Vector2(3f, -2f)
        };

        int randomCorner = Random.Range(0, corners.Length);

        Instantiate(
            interferenceZonePrefab,
            corners[randomCorner],
            Quaternion.identity
        );

        Debug.Log("Interferencia generada");
    }

    void SpawnDataPulse()
    {
        Vector2 spawnPos;
        Vector2 direction;

        int side =
        Random.Range(0, 4);

        switch (side)
        {
            case 0:

                spawnPos =
                new Vector2(
                6,
                Random.Range(-2f, 2f));

                direction =
                Vector2.left;

                break;

            case 1:

                spawnPos =
                new Vector2(
                -6,
                Random.Range(-2f, 2f));

                direction =
                Vector2.right;

                break;

            case 2:

                spawnPos =
                new Vector2(
                Random.Range(-3f, 3f),
                4);

                direction =
                Vector2.down;

                break;

            default:

                spawnPos =
                new Vector2(
                Random.Range(-3f, 3f),
                -4);

                direction =
                Vector2.up;

                break;
        }

        GameObject pulse =
        Instantiate(
        dataPulsePrefab,
        spawnPos,
        Quaternion.identity);

        DataPulse data =
        pulse.GetComponent<DataPulse>();

        data.SetDirection(direction);
    }
}