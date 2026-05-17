using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    int actCount = 0;

    bool mercyAvailable = false;
    public enum BattleState
    {
        PLAYERTURN,
        BUSY,
        ENEMYTURN,
        WON,
        LOST
    }

    public BattleState currentState;

    public int enemyMaxHealth = 20;
    public int enemyHealth = 20;

    private int currentPhase = 1;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI enemyNameText;

    public Slider enemyHPBar;

    public GameObject buttonsPanel;

    public TelecomSpawner telecomSpawner;
    public PlayerMovement playerMovement;

    public GameObject continueButton;

    public EnemyData enemyData;

    public string enemyName = "Profesor de Telecomunicaciones";

    [TextArea]
    public string[] actDialogues;

    [TextArea]
    public string[] enemyTurnDialogues;

    void Start()
    {
        if (enemyData != null)
        {
            enemyName = enemyData.enemyName;
            enemyMaxHealth = enemyData.maxHealth;
            enemyHealth = enemyMaxHealth;

            dialogueText.text = enemyData.introText;
        }
        else
        {
            enemyHealth = enemyMaxHealth;
        }

        if (enemyHPBar != null)
        {
            enemyHPBar.maxValue = enemyMaxHealth;
            enemyHPBar.value = enemyHealth;
        }

        if (enemyNameText != null)
        {
            enemyNameText.text = enemyName;
        }

        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        currentState = BattleState.PLAYERTURN;

        dialogueText.text = "* ¿Qué harás?";

        buttonsPanel.SetActive(true);

        telecomSpawner.StopSpawning();
        telecomSpawner.SetBlueSoulPattern(false);

        playerMovement.ActivateRedSoul();
    }

    void StartEnemyTurn()
    {
        currentState = BattleState.ENEMYTURN;

        buttonsPanel.SetActive(false);

        string enemyText = GetRandomText(enemyTurnDialogues);

        int randomMode = Random.Range(0, 2);

        if (randomMode == 0)
        {
            playerMovement.ActivateRedSoul();

            telecomSpawner.SetBlueSoulPattern(false);

            dialogueText.text = "* El profesor transmite una señal.";
        }
        else
        {
            playerMovement.ActivateBlueSoul();

            telecomSpawner.SetBlueSoulPattern(true);

            dialogueText.text = "* La señal altera la gravedad.";
        }

        telecomSpawner.StartSpawning();

        Invoke(nameof(EndEnemyTurn), 5f);
    }

    void EndEnemyTurn()
    {
        telecomSpawner.StopSpawning();

        StartCoroutine(WaitForAttacks());
    }

    IEnumerator WaitForAttacks()
    {
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            yield return null;
        }

        telecomSpawner.SetBlueSoulPattern(false);

        playerMovement.ActivateRedSoul();

        StartPlayerTurn();
    }

    public void Fight()
    {
        if (currentState != BattleState.PLAYERTURN)
            return;

        currentState = BattleState.BUSY;
        buttonsPanel.SetActive(false);

        int damage = Random.Range(3, 8);

        enemyHealth -= damage;

        if (enemyHealth < 0)
        {
            enemyHealth = 0;
        }

        enemyHPBar.value = enemyHealth;

        CheckPhase();

        dialogueText.text = "Hiciste " + damage + " de daño.";

        if (enemyHealth <= 0)
        {
            WinBattle();
            return;
        }

        Invoke(nameof(StartEnemyTurn), 2f);
    }

    public void Act()
    {
        if (currentState != BattleState.PLAYERTURN)
            return;

        currentState = BattleState.BUSY;

        buttonsPanel.SetActive(false);

        actCount++;

        if (actCount >= 3)
        {
            mercyAvailable = true;

            dialogueText.text =
            "* El profesor parece más tranquilo.";
        }
        else
        {
            dialogueText.text =
            "* Analizas la frecuencia.";
        }

        Invoke(nameof(StartEnemyTurn), 2f);
    }

    public void Item()
    {
        if (currentState != BattleState.PLAYERTURN)
            return;

        currentState = BattleState.BUSY;
        buttonsPanel.SetActive(false);

        GameManager.instance.Heal(2);

        dialogueText.text = "* Usaste una batería de respaldo.";

        Invoke(nameof(StartEnemyTurn), 2f);
    }

    public void Mercy()
    {
        if (currentState != BattleState.PLAYERTURN)
            return;

        currentState = BattleState.BUSY;

        buttonsPanel.SetActive(false);

        if (mercyAvailable)
        {
            dialogueText.text =
            "* Perdonaste al profesor.";

            WinBattle();
        }
        else
        {
            dialogueText.text =
            "* Aún no puedes perdonarlo.";

            Invoke(nameof(StartEnemyTurn), 2f);
        }
    }

    void CheckPhase()
    {
        if (enemyHealth <= 6 && currentPhase != 3)
        {
            currentPhase = 3;

            dialogueText.text = "* La señal entra en modo crítico.";

            telecomSpawner.SetDifficulty(0.4f, 12f);
        }
        else if (enemyHealth <= 13 && currentPhase != 2)
        {
            currentPhase = 2;

            dialogueText.text = "* La interferencia aumenta.";

            telecomSpawner.SetDifficulty(0.7f, 10f);
        }
    }

    void WinBattle()
    {
        currentState = BattleState.WON;

        dialogueText.text = "* GANASTE.";

        telecomSpawner.StopSpawning();
        telecomSpawner.SetBlueSoulPattern(false);

        playerMovement.ActivateRedSoul();

        buttonsPanel.SetActive(false);

        if (continueButton != null)
        {
            continueButton.SetActive(true);
        }

        Debug.Log("VICTORIA");
    }

    public void ContinueAfterBattle()
    {
        Debug.Log("Continuar después de la batalla.");

        // Aquí después se conectará con el sistema de tu equipo:
        // volver al pasillo, cargar siguiente escena o activar evento.
    }

    string GetRandomText(string[] texts)
    {
        if (texts == null || texts.Length == 0)
        {
            return "";
        }

        int index = Random.Range(0, texts.Length);

        return texts[index];
    }
}