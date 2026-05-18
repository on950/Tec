using UnityEngine;

public abstract class BaseAttackSpawner : MonoBehaviour
{
    public bool canSpawn = false;
    public bool blueSoulPattern = false;
    public int phase = 1;

    public abstract void StartSpawning();

    public abstract void StopSpawning();

    public abstract void SetDifficulty(float spawnRate, float speed);

    public virtual void SetBlueSoulPattern(bool active)
    {
        blueSoulPattern = active;
    }

    public virtual void SetPhase(int newPhase)
    {
        phase = newPhase;
    }
}