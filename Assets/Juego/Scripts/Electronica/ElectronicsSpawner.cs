using UnityEngine;

public class ElectronicsSpawner : BaseAttackSpawner
{
    [Header("Prefabs")]
    public GameObject electricSparkPrefab;
    public GameObject voltageRayPrefab;
    public GameObject circuitOverloadPrefab;

    [Header("Dificultad")]
    public float spawnRate = 1f;
    public float sparkSpeed = 9f;

    [Header("Battle Box")]
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -2.2f;
    public float maxY = 2.2f;
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

    public override void SetDifficulty(float newSpawnRate, float newSparkSpeed)
    {
        spawnRate = newSpawnRate;
        sparkSpeed = newSparkSpeed;
    }

    void Update()
    {
        if (!canSpawn) return;

        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnAttack();
            timer = 0;
        }
    }

    public override void SetBlueSoulPattern(bool active)
    {
        blueSoulPattern = active;
    }

    public override void SetPhase(int newPhase)
    {
        phase = newPhase;
        Debug.Log("ElectronicsSpawner recibió fase: " + phase);
    }

    void SpawnAttack()
    {
        if (phase == 1)
        {
            SpawnElectricSpark();
        }
        else if (phase == 2)
        {
            int randomAttack = Random.Range(0, 2);

            if (randomAttack == 0)
                SpawnElectricSpark();
            else
                SpawnVoltageRay();
        }
        else
        {
            int randomAttack = Random.Range(0, 3);

            if (randomAttack == 0)
                SpawnElectricSpark();
            else if (randomAttack == 1)
                SpawnVoltageRay();
            else
                SpawnCircuitOverload();
        }
    }

    void SpawnElectricSpark()
    {
        int side = Random.Range(0, 2);

        Vector2 spawnPos;
        Vector2 direction;

        float spawnY;

        if (blueSoulPattern)
        {
            spawnY = Random.Range(-2f, -0.4f);
        }
        else
        {
            spawnY = Random.Range(-2f, 2f);
        }

        if (side == 0)
        {
            spawnPos = new Vector2(5f, spawnY);
            direction = Vector2.left;
        }
        else
        {
            spawnPos = new Vector2(-5f, spawnY);
            direction = Vector2.right;
        }

        GameObject spark = Instantiate(
            electricSparkPrefab,
            spawnPos,
            Quaternion.identity
        );

        ElectricSpark electricSpark =
            spark.GetComponent<ElectricSpark>();

        if (electricSpark != null)
        {
            electricSpark.speed = sparkSpeed;
            electricSpark.SetDirection(direction);
        }
    }

    void SpawnVoltageRay()
    {
        float randomX = Random.Range(minX + 1f, maxX - 1f);

        Vector2 spawnPos = new Vector2(
            randomX,
            maxY
        );

        Instantiate(
            voltageRayPrefab,
            spawnPos,
            Quaternion.Euler(0, 0, 90)
        );
    }

    void SpawnCircuitOverload()
    {
        float randomX = Random.Range(-3.5f, 3.5f);

        Vector2 spawnPos = new Vector2(
            randomX,
            -2.15f
        );

        Instantiate(
            circuitOverloadPrefab,
            spawnPos,
            Quaternion.identity
        );
    }
}