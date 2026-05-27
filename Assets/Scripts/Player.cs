using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 1.5f; // Speed of the player movement   
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Vector2 movement; // Store the player's movement input
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (BattleRequest.hasReturnPosition)
        {
            transform.position = BattleRequest.returnPosition;

            Debug.Log("Regresando a: " + BattleRequest.returnPosition);

            BattleRequest.hasReturnPosition = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from the player
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement=movement.normalized; // Normalize the movement vector to prevent faster diagonal movement
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.magnitude);
    }


    private void FixedUpdate()
    {
        // Move the player
        rb.linearVelocity = movement * speed;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Inicio":
                SceneManager.LoadScene(1);
                break;
            case "BibliotecaInicio":
                SceneManager.LoadScene(2);
                break;
            case "SalidaJMNS":
                SceneManager.LoadScene(3);
                break;
            case "SalidaE":
                SceneManager.LoadScene(4);
                break;
            case "Cooperativa":
                SceneManager.LoadScene(5);
                break;
            case "AcrBiblioteca":
                SceneManager.LoadScene(6);
                break;
            case "SalidaC":
                SceneManager.LoadScene(7);
                break;
            case "Servicios":
                SceneManager.LoadScene(8);
                break;
            case "SalidaCope":
                SceneManager.LoadScene(9);
                break;
            case "SalidaK":
                SceneManager.LoadScene(10);
                break;
            case "SalidaKOPQ":
                SceneManager.LoadScene(11);
                break;
            case "SalidaVOPQ":
                SceneManager.LoadScene(12);
                break;
            case "InicioACR":
                SceneManager.LoadScene(13);
                break;
            case "BibliotecaACR":
                SceneManager.LoadScene(14);
                break;
            case "Biblioteca":
                SceneManager.LoadScene(15);
                break;
            case "Oxxito":
                SceneManager.LoadScene(16);
                break;
            case "OxxitoOPQ":
                SceneManager.LoadScene(17);
                break;
            case "SalidaQ1OPQ":
                SceneManager.LoadScene(18);
                break;
            case "SalidaQ2OPQ":
                SceneManager.LoadScene(19);
                break;
            case "SalidaQXW":
                SceneManager.LoadScene(20);
                break;
            case "SalidaO":
                SceneManager.LoadScene(21);
                break;
            case "SalidaOMNS":
                SceneManager.LoadScene(22);
                break;
            case "SalidaQ1MNS":
                SceneManager.LoadScene(23);
                break;
            case "SalidaQ2MNS":
                SceneManager.LoadScene(24);
                break;
            case "SalidaP":
                SceneManager.LoadScene(25);
                break;
            case "SalidaXMNS":
                SceneManager.LoadScene(26);
                break;
            case "SalidaXY":
                SceneManager.LoadScene(27);
                break;
            case "SalidaY":
                SceneManager.LoadScene(28);
                break;
            case "X":
                SceneManager.LoadScene(29);
                break;
            case "SalidaXPB":
                SceneManager.LoadScene(32);
                break;
            case "PAX":
                SceneManager.LoadScene(30);
                break;
            case "PBX":
                SceneManager.LoadScene(31);
                break;
            case "Q":
                SceneManager.LoadScene(57);
                break;
            case "PAQ":
                SceneManager.LoadScene(56);
                break;
            case "PBQE":
                SceneManager.LoadScene(58);
                break;
            case "SalidaQPB":
                SceneManager.LoadScene(60);
                break;
            case "SalidaSal":
                SceneManager.LoadScene(61);
                break;
            case "C":
                SceneManager.LoadScene(59);
                break;
            case "D":
                SceneManager.LoadScene(62);
                break;
            case "SallidaSalD":
                SceneManager.LoadScene(63);
                break;
            case "Audio":
                SceneManager.LoadScene(64);
                break;
            case "SalidaAudio":
                SceneManager.LoadScene(65);
                break;
            case "J":
                SceneManager.LoadScene(66); 
                break;
            case "SalidaSalJ":
                SceneManager.LoadScene(67);
                break;
            case "F":
                SceneManager.LoadScene(68);
                break; 
            case "SalidaSalE":
                SceneManager.LoadScene(69);
                break;
            case "K":
                SceneManager.LoadScene(70);
                break;
            case "SalidaSalK":
                SceneManager.LoadScene(71);
                break;
            case "G":
                SceneManager.LoadScene(72);
                break;
            case "SalidaSalG":
                SceneManager.LoadScene(73);
                break;
        }
    }
}