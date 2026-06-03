using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int maxHealth = 5;
    public int health = 5;

    public TextMeshProUGUI hpText;
    public Slider hpBar;

    public GameObject gameOverText;

    public PlayerMovement playerMovement;
    public PlayerHealth playerHealth;
    public TelecomSpawner telecomSpawner;
    public GameObject buttonsPanel, retryButton;

    public BattleManager battleManager;

    private bool isGameOver = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth;

        hpBar.maxValue = maxHealth;
        hpBar.value = health;

        UpdateUI();

        if (gameOverText != null)
        {
            gameOverText.SetActive(false);
        }
        if (retryButton != null)
        {
            retryButton.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isGameOver)
            return;

        health -= damage;

        if (health < 0)
        {
            health = 0;
        }

        UpdateUI();

        if (health <= 0)
        {
            GameOver();
        }
    }

    public void Heal(int amount)
    {
        if (isGameOver)
            return;

        health += amount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        hpText.text = "HP: " + health;
        hpBar.value = health;
    }

    void GameOver()
    {
        isGameOver = true;

        if (gameOverText != null)
            gameOverText.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (playerHealth != null)
            playerHealth.DisableDamage();

        if (telecomSpawner != null)
            telecomSpawner.StopSpawning();

        // Avisar al BattleManager
        if (battleManager != null)
            battleManager.ForceLoseBattle();

        Debug.Log("GAME OVER");
    }

    public void RetryBattle()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}