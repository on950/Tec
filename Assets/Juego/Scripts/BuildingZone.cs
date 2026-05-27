using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingZone : MonoBehaviour
{
    public string battleSceneName;
    public Transform exitPoint;

    private bool playerInside = false;
    private Transform player;

    void Update()
    {
        if (!playerInside)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (exitPoint != null)
                BattleRequest.returnPosition = exitPoint.position;
            else if (player != null)
                BattleRequest.returnPosition = player.position;

            BattleRequest.hasReturnPosition = true;

            SceneManager.LoadScene(battleSceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            player = null;
        }
    }
}