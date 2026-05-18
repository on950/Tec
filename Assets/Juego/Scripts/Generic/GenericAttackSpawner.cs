using UnityEngine;

public class GenericAttackSpawner : BaseAttackSpawner
{
    [Header("Prefabs")]
    public GameObject projectilePrefab;
    public GameObject fallingAttackPrefab;
    public GameObject damageZonePrefab;

    [Header("Dificultad")]
    public float spawnRate = 1f;
    public float projectileSpeed = 7f;
    public float fallingSpeed = 8f;

    [Header("BattleBox")]
    public float minX = -4f;
    public float maxX = 4f;
    public float minY = -2.5f;
    public float maxY = 2.5f;
    public float margin = 0.3f;

    [Header("Estado")]
    public bool canSpawn = false;
    public bool blueSoulPattern = false;
    public int phase = 1;

    [Header("Probabilidades Fase 3")]
    public int projectileWeight = 3;
    public int fallingWeight = 3;
    public int zoneWeight = 1;

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

    public override void SetDifficulty(float newSpawnRate, float newSpeed)
    {
        spawnRate = newSpawnRate;
        projectileSpeed = newSpeed;
        fallingSpeed = newSpeed;
    }

    public override void SetBlueSoulPattern(bool active)
    {
        blueSoulPattern = active;
    }

    public override void SetPhase(int newPhase)
    {
        phase = newPhase;
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
            SpawnProjectile();
        }
        else if (phase == 2)
        {
            int randomAttack = Random.Range(0, 2);

            if (randomAttack == 0)
                SpawnProjectile();
            else
                SpawnFallingAttack();
        }
        else
        {
            SpawnWeightedPhase3();
        }
    }

    void SpawnWeightedPhase3()
    {
        int totalWeight =
            projectileWeight +
            fallingWeight +
            zoneWeight;

        int randomValue =
            Random.Range(0, totalWeight);

        if (randomValue < projectileWeight)
        {
            SpawnProjectile();
        }
        else if (randomValue < projectileWeight + fallingWeight)
        {
            SpawnFallingAttack();
        }
        else
        {
            SpawnDamageZone();
        }
    }

    void SpawnProjectile()
    {
        int side = Random.Range(0, 2);

        Vector2 spawnPos;
        Vector2 direction;

        float spawnY;

        if (blueSoulPattern)
        {
            spawnY = Random.Range(
                minY + 0.3f,
                minY + 1.4f
            );
        }
        else
        {
            spawnY = Random.Range(
                minY + margin,
                maxY - margin
            );
        }

        if (side == 0)
        {
            spawnPos = new Vector2(maxX, spawnY);
            direction = Vector2.left;
        }
        else
        {
            spawnPos = new Vector2(minX, spawnY);
            direction = Vector2.right;
        }

        GameObject projectile =
            Instantiate(
                projectilePrefab,
                spawnPos,
                Quaternion.identity
            );

        GenericProjectile genericProjectile =
            projectile.GetComponent<GenericProjectile>();

        if (genericProjectile != null)
        {
            genericProjectile.speed = projectileSpeed;
            genericProjectile.SetDirection(direction);
        }
    }

    void SpawnFallingAttack()
    {
        float randomX = Random.Range(
            minX + margin,
            maxX - margin
        );

        Vector2 spawnPos = new Vector2(
            randomX,
            maxY
        );

        GameObject falling =
            Instantiate(
                fallingAttackPrefab,
                spawnPos,
                Quaternion.identity
            );

        GenericFallingAttack fallingAttack =
            falling.GetComponent<GenericFallingAttack>();

        if (fallingAttack != null)
        {
            fallingAttack.speed = fallingSpeed;
        }
    }

    void SpawnDamageZone()
    {
        float randomX = Random.Range(
            minX + 0.7f,
            maxX - 0.7f
        );

        Vector2 spawnPos = new Vector2(
            randomX,
            minY
        );

        Instantiate(
            damageZonePrefab,
            spawnPos,
            Quaternion.identity
        );
    }
}