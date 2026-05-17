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

        }
    }
}