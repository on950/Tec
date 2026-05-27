using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    int actCount = 0;

    int selectedButton = 0;
    float inputCooldown = 0.25f;
    float inputTimer = 0f;

    private bool usingController;
    private bool mouseIsSelecting = false;

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

    private BaseAttackSpawner attackSpawner;
    public PlayerMovement playerMovement;

    public GameObject continueButton;
    public GameObject retryButton;

    public EnemyData enemyData;

    public Transform spawnerPoint;

    public string enemyName = "Profesor de Telecomunicaciones";

    [TextArea]
    public string[] actDialogues;

    [TextArea]
    public string[] enemyTurnDialogues;

    public Image dialogueBoxImage;
    public Image hpBarFill;
    public Image enemyHPBarFill;
    public Image enemyImage;

    public TextMeshProUGUI fightText;
    public TextMeshProUGUI actText;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI mercyText;

    void Start()
    {
        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(true);
        }
        if (enemyData != null)
        {
            Debug.Log("BattleManager cargó enemigo: " + enemyData.enemyName);
            enemyName = enemyData.enemyName;
            enemyMaxHealth = enemyData.maxHealth;
            enemyHealth = enemyMaxHealth;

            dialogueText.text = enemyData.introText;

            if (enemyImage != null && enemyData.enemySprite != null)
            {
                enemyImage.sprite = enemyData.enemySprite;
                enemyImage.SetNativeSize();
            }

            if (enemyNameText != null)
            {
                enemyNameText.text = enemyName;
            }

            if (enemyData.attackSpawnerPrefabs != null &&
                enemyData.attackSpawnerPrefabs.Length > 0 &&
                enemyData.attackSpawnerPrefabs[0] != null)
            {
                GameObject spawnerObject = Instantiate(
                    enemyData.attackSpawnerPrefabs[0],
                    spawnerPoint.position,
                    Quaternion.identity
                );

                attackSpawner =
                    spawnerObject.GetComponent<BaseAttackSpawner>();

                attackSpawner.SetPhase(1);
            }
            else
            {
                Debug.LogWarning("Este EnemyData no tiene AttackSpawner asignado.");
            }
        }
        else
        {
            Debug.LogError("No hay EnemyData asignado al BattleManager");
            return;
        }

        if (enemyHPBar != null)
        {
            enemyHPBar.maxValue = enemyMaxHealth;
            enemyHPBar.value = enemyHealth;
        }

        ApplyEnemyColors();

        currentPhase = 1;

        StartPlayerTurn();
    }

    void Update()
    {
        usingController = Input.GetJoystickNames().Length > 0 &&
                  !string.IsNullOrEmpty(Input.GetJoystickNames()[0]);
        if (currentState == BattleState.WON)
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                ContinueAfterBattle();
            }

            return;
        }

        if (currentState == BattleState.LOST)
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                GameManager.instance.RetryBattle();
            }

            return;
        }
        if (currentState != BattleState.PLAYERTURN)
            return;

        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            mouseIsSelecting = false;

            selectedButton--;
            if (selectedButton < 0)
                selectedButton = 3;

            UpdateButtonSelection();

            Debug.Log("Seleccionado: " + selectedButton);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            mouseIsSelecting = false;

            selectedButton++;
            if (selectedButton > 3)
                selectedButton = 0;

            UpdateButtonSelection();

            Debug.Log("Seleccionado: " + selectedButton);
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            ExecuteSelectedButton();
        }
    }

    public void RetryBattle()
    {
        Debug.Log("Reintentando batalla.");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ExecuteSelectedButton()
    {
        switch (selectedButton)
        {
            case 0:
                Fight();
                break;

            case 1:
                Act();
                break;

            case 2:
                Item();
                break;

            case 3:
                Mercy();
                break;
        }
    }

    void StartPlayerTurn()
    {
        if (currentState == BattleState.LOST)
            return;

        currentState = BattleState.PLAYERTURN;

        dialogueText.text = "* ¿Qué harás?";

        buttonsPanel.SetActive(true);

        mouseIsSelecting = false;
        selectedButton = 0;
        UpdateButtonSelection();

        if (attackSpawner != null)
        {
            attackSpawner.StopSpawning();
            attackSpawner.SetBlueSoulPattern(false);
        }

        playerMovement.ActivateRedSoul();
    }

    public void ForceLoseBattle()
    {
        currentState = BattleState.LOST;

        CancelInvoke();
        StopAllCoroutines();

        if (attackSpawner != null)
        {
            attackSpawner.StopSpawning();
            attackSpawner.SetBlueSoulPattern(false);
        }

        if (buttonsPanel != null)
            buttonsPanel.SetActive(false);

        if (continueButton != null)
            continueButton.SetActive(false);

        // NUEVO
        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);

        Debug.Log("BattleManager cambió a LOST");
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

            attackSpawner.SetBlueSoulPattern(false);

            dialogueText.text = "* El profesor transmite una señal.";
        }
        else
        {
            playerMovement.ActivateBlueSoul();

            attackSpawner.SetBlueSoulPattern(true);

            dialogueText.text = "* La señal altera la gravedad.";
        }

        attackSpawner.StartSpawning();

        Invoke(nameof(EndEnemyTurn), 5f);
    }

    void EndEnemyTurn()
    {
        attackSpawner.StopSpawning();

        StartCoroutine(WaitForAttacks());
    }

    IEnumerator WaitForAttacks()
    {
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            yield return null;
        }

        attackSpawner.SetBlueSoulPattern(false);

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

            attackSpawner.SetDifficulty(0.4f, 12f);
            attackSpawner.SetPhase(3);

            Debug.Log("FASE 3 ACTIVADA");
        }
        else if (enemyHealth <= 13 && currentPhase != 2)
        {
            currentPhase = 2;

            dialogueText.text = "* La interferencia aumenta.";

            attackSpawner.SetDifficulty(0.7f, 10f);
            attackSpawner.SetPhase(2);

            Debug.Log("FASE 2 ACTIVADA");
        }
    }

    void WinBattle()
    {
        currentState = BattleState.WON;

        dialogueText.text = "* GANASTE.";

        attackSpawner.StopSpawning();
        attackSpawner.SetBlueSoulPattern(false);

        playerMovement.ActivateRedSoul();

        buttonsPanel.SetActive(false);

        if (continueButton != null)
        {
            continueButton.SetActive(true);
        }

        Debug.Log("VICTORIA");
    }

    void LoseBattle()
    {
        currentState = BattleState.LOST;

        dialogueText.text = "* Has sido derrotado.";

        if (attackSpawner != null)
        {
            attackSpawner.StopSpawning();
            attackSpawner.SetBlueSoulPattern(false);
        }

        playerMovement.ActivateRedSoul();

        // Desactivar menú de acciones
        buttonsPanel.SetActive(false);

        // Ocultar continuar por seguridad
        if (continueButton != null)
            continueButton.SetActive(false);

        // Mostrar retry
        if (retryButton != null)
            retryButton.SetActive(true);

        Debug.Log("DERROTA");
    }

    public void ContinueAfterBattle()
    {
        Debug.Log("Continuar después de la batalla.");

        if (!string.IsNullOrEmpty(BattleRequest.returnScene))
        {
            SceneManager.LoadScene(
                BattleRequest.returnScene
            );
        }
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

    void ApplyEnemyColors()
    {
        if (enemyData == null)
            return;

        if (enemyNameText != null)
            enemyNameText.color = enemyData.primaryColor;

        if (dialogueBoxImage != null)
            dialogueBoxImage.color = Color.black;

        if (hpBarFill != null)
            hpBarFill.color = enemyData.secondaryColor;

        if (enemyHPBarFill != null)
            enemyHPBarFill.color = enemyData.primaryColor;

        if (fightText != null)
            fightText.color = enemyData.primaryColor;

        if (actText != null)
            actText.color = enemyData.primaryColor;

        if (itemText != null)
            itemText.color = enemyData.primaryColor;

        if (mercyText != null)
            mercyText.color = enemyData.primaryColor;

        UpdateButtonSelection();
    }

    void UpdateButtonSelection()
    {
        Color normal = Color.white;
        Color selected = Color.yellow;

        if (enemyData != null)
        {
            normal = enemyData.primaryColor;
            selected = enemyData.secondaryColor;
        }

        fightText.color = normal;
        actText.color = normal;
        itemText.color = normal;
        mercyText.color = normal;

        if (selectedButton == 0)
            fightText.color = selected;

        if (selectedButton == 1)
            actText.color = selected;

        if (selectedButton == 2)
            itemText.color = selected;

        if (selectedButton == 3)
            mercyText.color = selected;
    }

    public void HoverButton(int index)
    {
        mouseIsSelecting = true;
        selectedButton = index;
        UpdateButtonSelection();
    }

    public void ChangeEnemy(EnemyData newEnemy)
    {
        if (newEnemy == null)
            return;

        if (attackSpawner != null)
        {
            attackSpawner.StopSpawning();
            Destroy(attackSpawner.gameObject);
        }

        enemyData = newEnemy;

        enemyName = enemyData.enemyName;
        enemyMaxHealth = enemyData.maxHealth;
        enemyHealth = enemyMaxHealth;

        dialogueText.text = enemyData.introText;
        enemyNameText.text = enemyName;

        enemyHPBar.maxValue = enemyMaxHealth;
        enemyHPBar.value = enemyHealth;

        if (enemyImage != null && enemyData.enemySprite != null)
        {
            enemyImage.sprite = enemyData.enemySprite;
            enemyImage.SetNativeSize();
        }

        GameObject spawnerObject = Instantiate(
            enemyData.attackSpawnerPrefabs[0],
            spawnerPoint.position,
            Quaternion.identity
        );

        attackSpawner =
            spawnerObject.GetComponent<BaseAttackSpawner>();

        attackSpawner.SetPhase(1);

        ApplyEnemyColors();

        currentPhase = 1;

        StartPlayerTurn();

        Debug.Log("BattleManager cambió a enemigo: " + enemyData.enemyName);
    }
}