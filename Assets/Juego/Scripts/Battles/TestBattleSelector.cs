using UnityEngine;

public class TestBattleSelector : MonoBehaviour
{
    public EnemyData[] enemies;

    private int currentIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            currentIndex--;

            if (currentIndex < 0)
                currentIndex = enemies.Length - 1;

            SelectEnemy(currentIndex);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            currentIndex++;

            if (currentIndex >= enemies.Length)
                currentIndex = 0;

            SelectEnemy(currentIndex);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            int randomIndex = Random.Range(0, enemies.Length);
            SelectEnemy(randomIndex);
        }
    }

    void SelectEnemy(int index)
    {
        if (enemies.Length == 0)
            return;

        if (enemies[index] == null)
            return;

        Debug.Log("Cambiando a enemigo: " + enemies[index].enemyName);

        BattleManager battleManager =
            FindFirstObjectByType<BattleManager>();

        if (battleManager != null)
        {
            battleManager.ChangeEnemy(enemies[index]);
        }
        else
        {
            Debug.LogWarning("No se encontró BattleManager en la escena.");
        }
    }
}