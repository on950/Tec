using UnityEngine;

public class AutomataSpawner : BaseAttackSpawner
{
    [Header("Prefabs")]
    public GameObject stateNodePrefab;
    public GameObject transitionArrowPrefab;
    public GameObject trapStatePrefab;

    [Header("Dificultad")]
    public float spawnRate = 1f;
    public float nodeSpeed = 7f;

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

    private float timer;
    private int stateCounter = 0;

    //------------------------------------

    public override void StartSpawning()
    {
        canSpawn = true;
        timer = 0;
    }

    public override void StopSpawning()
    {
        canSpawn = false;
    }

    public override void SetDifficulty(
        float newSpawnRate,
        float newNodeSpeed)
    {
        spawnRate = newSpawnRate;
        nodeSpeed = newNodeSpeed;
    }

    public override void SetBlueSoulPattern(bool active)
    {
        blueSoulPattern = active;
    }

    public override void SetPhase(int newPhase)
    {
        phase = newPhase;

        Debug.Log(
        "AutomataSpawner recibió fase: "
        + phase);
    }

    //------------------------------------

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

    //------------------------------------

    void SpawnAttack()
    {
        if (phase == 1)
        {
            SpawnStateNode();
        }
        else if (phase == 2)
        {
            int randomAttack =
                Random.Range(0, 2);

            if (randomAttack == 0)
            {
                SpawnStateNode();
            }
            else
            {
                SpawnTransitionArrow();
            }
        }
        else
        {
            int randomAttack =
                Random.Range(0, 5);

            if (randomAttack == 0 ||
                randomAttack == 1)
            {
                SpawnStateNode();
            }
            else if (randomAttack == 2 ||
                     randomAttack == 3)
            {
                SpawnTransitionArrow();
            }
            else
            {
                SpawnTrapState();
            }
        }
    }

    //------------------------------------
    // ATAQUE 1
    // StateNode
    //------------------------------------

    void SpawnStateNode()
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

        GameObject node = Instantiate(
            stateNodePrefab,
            spawnPos,
            Quaternion.identity
        );

        StateNode stateNode =
            node.GetComponent<StateNode>();

        if (stateNode != null)
        {
            stateNode.speed = nodeSpeed;
            stateNode.SetDirection(direction);
            stateNode.SetStateNumber(stateCounter);
            stateCounter++;
        }
    }

    //------------------------------------
    // ATAQUE 2
    // TransitionArrow
    //------------------------------------

    void SpawnTransitionArrow()
    {
        float randomX = Random.Range(
            minX + margin,
            maxX - margin
        );

        Vector2 spawnPos = new Vector2(
            randomX,
            maxY
        );

        GameObject arrow = Instantiate(
            transitionArrowPrefab,
            spawnPos,
            Quaternion.Euler(0, 0, -90)
        );

        TransitionArrow transition =
            arrow.GetComponent<TransitionArrow>();

        if (transition != null)
        {
            transition.SetDirection(Vector2.down);
        }
    }


    void SpawnTrapState()
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
            trapStatePrefab,
            spawnPos,
            Quaternion.identity
        );
    }
}