using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingZone : MonoBehaviour
{
    public EnemyData buildingEnemy;

    [Header("Punto donde aparecerá al volver")]
    public Transform exitPoint;

    private bool alreadyEntered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyEntered) return;

        if (other.CompareTag("Player"))
        {
            alreadyEntered = true;

            BattleRequest.selectedEnemy = buildingEnemy;

            if (exitPoint != null)
            {
                BattleRequest.returnPosition = exitPoint.position;
            }
            else
            {
                BattleRequest.returnPosition = other.transform.position;
            }

            BattleRequest.hasReturnPosition = true;

            Debug.Log("Entraste a: " + gameObject.name +
                      " | Enemy: " + buildingEnemy.enemyName);

            SceneManager.LoadScene(0);
        }
    }
}