using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleZone : MonoBehaviour
{
    public EnemyData enemyData;
    public string battleSceneName = "BattleScene";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BattleRequest.selectedEnemy = enemyData;
            SceneManager.LoadScene(battleSceneName);
        }
    }
}