using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleZone : MonoBehaviour
{
    [Header("Escena de batalla que se va a cargar")]
    public string battleSceneName;

    [Header("Punto donde regresa el jugador")]
    public Transform exitPoint;

    private bool playerInside = false;
    private Transform player;

    void Update()
    {
        if (!playerInside)
            return;

        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            BattleRequest.returnScene =
                SceneManager.GetActiveScene().name;

            if (exitPoint != null)
                BattleRequest.returnPosition = exitPoint.position;
            else if (player != null)
                BattleRequest.returnPosition = player.position;

            BattleRequest.hasReturnPosition = true;

            Debug.Log("Cargando batalla: " + battleSceneName);

            SceneManager.LoadScene(battleSceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            player = other.transform;

            Debug.Log("Presiona ENTER para iniciar batalla.");
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