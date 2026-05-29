using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public enum BattleState
    {
        PLAYERTURN,
        BUSY,
        ENEMYTURN,
        WON,
        LOST
    }

    [Header("Estado de batalla")]
    public BattleState currentState;

    private int selectedButton = 0;
    private int actCount = 0;
    private bool mercyAvailable = false;
    private int currentPhase = 1;

    [Header("Colores Inventario")]
    public Image inventoryPanelImage;
    public Image batteryButtonImage;
    public Image formulaButtonImage;
    public Image shieldButtonImage;

    public Outline inventoryPanelOutline;
    public Outline batteryButtonOutline;
    public Outline formulaButtonOutline;
    public Outline shieldButtonOutline;

    [Header("Inventario")]
    public int batteries = 3;
    public int formulaBooks = 2;
    public int shields = 1;

    [SerializeField] private int healAmount = 2;

    private int damageBonus = 0;
    private int damageBonusTurns = 0;

    public bool shieldActive = false;
    public int shieldTurns = 0;

    [Header("Enemigo")]
    public EnemyData enemyData;
    public int enemyMaxHealth = 20;
    public int enemyHealth = 20;

    [Header("UI")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI enemyNameText;
    public Slider enemyHPBar;
    public GameObject buttonsPanel;
    public GameObject continueButton;
    public GameObject retryButton;

    [Header("Inventario UI")]
    public GameObject inventoryPanel;
    public Button batteryButton;
    public Button formulaButton;
    public Button shieldButton;

    public TextMeshProUGUI batteryButtonText;
    public TextMeshProUGUI formulaButtonText;
    public TextMeshProUGUI shieldButtonText;

    [Header("Imágenes")]
    public Image dialogueBoxImage;
    public Image hpBarFill;
    public Image enemyHPBarFill;
    public Image enemyImage;

    [Header("Texto botones")]
    public TextMeshProUGUI fightText;
    public TextMeshProUGUI actText;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI mercyText;

    [Header("Ataques")]
    private BaseAttackSpawner attackSpawner;
    public PlayerMovement playerMovement;
    public Transform spawnerPoint;

    void Start()
    {
        if (enemyData == null)
        {
            Debug.LogError("No hay EnemyData asignado al BattleManager");
            return;
        }

        LoadEnemyData();
        ApplyEnemyColors();
        UpdateItemText();
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        UpdateInventoryTexts();
        StartPlayerTurn();
    }

    void Update()
    {
        if (currentState == BattleState.WON)
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.JoystickButton0))
                ContinueAfterBattle();

            return;
        }

        if (currentState == BattleState.LOST)
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.JoystickButton0))
                RetryBattle();

            return;
        }

        if (currentState == BattleState.BUSY && dialogueText.text.Contains("INVENTARIO"))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                UseBattery();

            if (Input.GetKeyDown(KeyCode.Alpha2))
                UseFormulaBook();

            if (Input.GetKeyDown(KeyCode.Alpha3))
                UseShield();

            return;
        }

        if (currentState != BattleState.PLAYERTURN)
            return;

        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            selectedButton--;

            if (selectedButton < 0)
                selectedButton = 3;

            UpdateButtonSelection();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            selectedButton++;

            if (selectedButton > 3)
                selectedButton = 0;

            UpdateButtonSelection();
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            ExecuteSelectedButton();
        }
    }

    void LoadEnemyData()
    {
        enemyMaxHealth = enemyData.maxHealth;
        enemyHealth = enemyMaxHealth;

        dialogueText.text = enemyData.introText;

        if (enemyNameText != null)
            enemyNameText.text = enemyData.enemyName;

        if (enemyHPBar != null)
        {
            enemyHPBar.maxValue = enemyMaxHealth;
            enemyHPBar.value = enemyHealth;
        }

        if (enemyImage != null && enemyData.enemySprite != null)
        {
            enemyImage.sprite = enemyData.enemySprite;
            enemyImage.SetNativeSize();
        }

        CreateAttackSpawner();

        currentPhase = 1;
        actCount = 0;
        mercyAvailable = false;

        damageBonus = 0;
        damageBonusTurns = 0;
        shieldActive = false;
        shieldTurns = 0;
    }

    void CreateAttackSpawner()
    {
        if (enemyData.attackSpawnerPrefabs == null ||
            enemyData.attackSpawnerPrefabs.Length == 0 ||
            enemyData.attackSpawnerPrefabs[0] == null)
        {
            Debug.LogWarning("Este EnemyData no tiene AttackSpawner asignado.");
            return;
        }

        GameObject spawnerObject = Instantiate(
            enemyData.attackSpawnerPrefabs[0],
            spawnerPoint.position,
            Quaternion.identity
        );

        attackSpawner = spawnerObject.GetComponent<BaseAttackSpawner>();

        if (attackSpawner != null)
            attackSpawner.SetPhase(1);
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

        selectedButton = 0;
        UpdateButtonSelection();

        if (attackSpawner != null)
        {
            attackSpawner.StopSpawning();
            attackSpawner.SetBlueSoulPattern(false);
        }

        if (playerMovement != null)
            playerMovement.ActivateRedSoul();
    }

    void StartEnemyTurn()
    {
        currentState = BattleState.ENEMYTURN;
        buttonsPanel.SetActive(false);

        int randomMode = Random.Range(0, 2);

        if (randomMode == 0)
        {
            playerMovement.ActivateRedSoul();

            if (attackSpawner != null)
                attackSpawner.SetBlueSoulPattern(false);
        }
        else
        {
            playerMovement.ActivateBlueSoul();

            if (attackSpawner != null)
                attackSpawner.SetBlueSoulPattern(true);
        }

        dialogueText.text = "* " + GetRandomText(enemyData.enemyTurnDialogues);

        if (attackSpawner != null)
            attackSpawner.StartSpawning();

        Invoke(nameof(EndEnemyTurn), 5f);
    }

    void EndEnemyTurn()
    {
        if (attackSpawner != null)
            attackSpawner.StopSpawning();

        StartCoroutine(WaitForAttacks());
    }

    IEnumerator WaitForAttacks()
    {
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            yield return null;

        if (attackSpawner != null)
            attackSpawner.SetBlueSoulPattern(false);

        if (playerMovement != null)
            playerMovement.ActivateRedSoul();

        ReduceShieldTurn();

        StartPlayerTurn();
    }

    public void Fight()
    {
        if (currentState != BattleState.PLAYERTURN)
            return;

        currentState = BattleState.BUSY;
        buttonsPanel.SetActive(false);

        int damage = Random.Range(3, 8) + damageBonus;

        enemyHealth -= damage;

        if (enemyHealth < 0)
            enemyHealth = 0;

        if (enemyHPBar != null)
            enemyHPBar.value = enemyHealth;

        CheckPhase();

        dialogueText.text = "* Hiciste " + damage + " de daño.";

        ReduceDamageBonusTurn();

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

        dialogueText.text = "* " + GetRandomText(enemyData.actDialogues);

        if (actCount >= 3)
            mercyAvailable = true;

        Invoke(nameof(StartEnemyTurn), 2f);
    }

    public void Item()
    {
        if (currentState != BattleState.PLAYERTURN)
            return;

        currentState = BattleState.BUSY;

        buttonsPanel.SetActive(false);

        if (inventoryPanel != null)
            inventoryPanel.SetActive(true);

        dialogueText.text = "* Elige un objeto.";
    }

    public void UseBattery()
    {
        if (batteries <= 0)
        {
            dialogueText.text = "* Ya no tienes baterías.";
            CloseInventoryAndStartEnemyTurn();
            return;
        }

        batteries--;

        GameManager.instance.Heal(healAmount);

        dialogueText.text =
            "* Usaste una batería.\n" +
            "* Baterías restantes: " + batteries + ".";

        UpdateInventoryTexts();
        CloseInventoryAndStartEnemyTurn();
    }

    public void UseFormulaBook()
    {
        if (formulaBooks <= 0)
        {
            dialogueText.text = "* Ya no tienes libros de fórmulas.";
            CloseInventoryAndStartEnemyTurn();
            return;
        }

        formulaBooks--;

        damageBonus = 2;
        damageBonusTurns = 3;

        dialogueText.text =
            "* Usaste un libro de fórmulas.\n" +
            "* Tu daño aumentará durante 3 ataques.";

        UpdateInventoryTexts();
        CloseInventoryAndStartEnemyTurn();
    }

    public void UseShield()
    {
        if (shields <= 0)
        {
            dialogueText.text = "* Ya no tienes escudos.";
            CloseInventoryAndStartEnemyTurn();
            return;
        }

        shields--;

        shieldActive = true;
        shieldTurns = 2;

        dialogueText.text =
            "* Usaste un escudo.\n" +
            "* Recibirás menos daño durante 2 turnos.";

        UpdateInventoryTexts();
        CloseInventoryAndStartEnemyTurn();
    }

    void CloseInventoryAndStartEnemyTurn()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        Invoke(nameof(StartEnemyTurn), 2f);
    }

    void UpdateInventoryTexts()
    {
        if (batteryButtonText != null)
            batteryButtonText.text = "BATERÍA x" + batteries;

        if (formulaButtonText != null)
            formulaButtonText.text = "LIBRO x" + formulaBooks;

        if (shieldButtonText != null)
            shieldButtonText.text = "ESCUDO x" + shields;
    }

    void ReduceDamageBonusTurn()
    {
        if (damageBonusTurns <= 0)
            return;

        damageBonusTurns--;

        if (damageBonusTurns <= 0)
        {
            damageBonus = 0;
        }
    }

    void ReduceShieldTurn()
    {
        if (!shieldActive)
            return;

        shieldTurns--;

        if (shieldTurns <= 0)
        {
            shieldActive = false;
            shieldTurns = 0;
        }
    }

    public int ApplyShieldReduction(int damage)
    {
        if (shieldActive)
        {
            damage = Mathf.CeilToInt(damage * 0.5f);
        }

        return damage;
    }

    public void Mercy()
    {
        if (currentState != BattleState.PLAYERTURN)
            return;

        currentState = BattleState.BUSY;
        buttonsPanel.SetActive(false);

        if (mercyAvailable)
        {
            dialogueText.text = "* Perdonaste al profesor.";
            WinBattle();
        }
        else
        {
            dialogueText.text = "* Aún no puedes perdonarlo.";
            Invoke(nameof(StartEnemyTurn), 2f);
        }
    }

    void CheckPhase()
    {
        if (enemyHealth <= 6 && currentPhase != 3)
        {
            currentPhase = 3;

            dialogueText.text = "* " + enemyData.phase3Dialogue;

            if (attackSpawner != null)
            {
                attackSpawner.SetDifficulty(0.4f, 12f);
                attackSpawner.SetPhase(3);
            }
        }
        else if (enemyHealth <= 13 && currentPhase != 2)
        {
            currentPhase = 2;

            dialogueText.text = "* " + enemyData.phase2Dialogue;

            if (attackSpawner != null)
            {
                attackSpawner.SetDifficulty(0.7f, 10f);
                attackSpawner.SetPhase(2);
            }
        }
    }

    void WinBattle()
    {
        currentState = BattleState.WON;

        dialogueText.text = "* GANASTE.";

        if (attackSpawner != null)
        {
            attackSpawner.StopSpawning();
            attackSpawner.SetBlueSoulPattern(false);
        }

        if (playerMovement != null)
            playerMovement.ActivateRedSoul();

        buttonsPanel.SetActive(false);

        if (continueButton != null)
            continueButton.SetActive(true);
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

        buttonsPanel.SetActive(false);

        if (continueButton != null)
            continueButton.SetActive(false);

        if (retryButton != null)
            retryButton.SetActive(true);

        dialogueText.text = "* Has sido derrotado.";
    }

    public void RetryBattle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ContinueAfterBattle()
    {
        BattleRequest.hasReturnPosition = true;

        if (!string.IsNullOrEmpty(BattleRequest.returnScene))
            SceneManager.LoadScene(BattleRequest.returnScene);
    }

    string GetRandomText(string[] texts)
    {
        if (texts == null || texts.Length == 0)
            return "No hay diálogo asignado.";

        int index = Random.Range(0, texts.Length);
        return texts[index];
    }

    void UpdateItemText()
    {
        if (itemText != null)
            itemText.text = "ITEM";
    }

    void ApplyEnemyColors()
    {
        if (enemyNameText != null)
            enemyNameText.color = enemyData.primaryColor;

        if (dialogueBoxImage != null)
            dialogueBoxImage.color = Color.black;

        if (hpBarFill != null)
            hpBarFill.color = enemyData.secondaryColor;

        if (enemyHPBarFill != null)
            enemyHPBarFill.color = enemyData.primaryColor;

        // Colores del inventario
        if (inventoryPanelImage != null)
            inventoryPanelImage.color = Color.black;

        if (inventoryPanelOutline != null)
            inventoryPanelOutline.effectColor = enemyData.primaryColor;

        if (batteryButtonImage != null)
            batteryButtonImage.color = Color.black;

        if (formulaButtonImage != null)
            formulaButtonImage.color = Color.black;

        if (shieldButtonImage != null)
            shieldButtonImage.color = Color.black;

        if (batteryButtonOutline != null)
            batteryButtonOutline.effectColor = enemyData.primaryColor;

        if (formulaButtonOutline != null)
            formulaButtonOutline.effectColor = enemyData.primaryColor;

        if (shieldButtonOutline != null)
            shieldButtonOutline.effectColor = enemyData.primaryColor;

        if (batteryButtonText != null)
            batteryButtonText.color = enemyData.primaryColor;

        if (formulaButtonText != null)
            formulaButtonText.color = enemyData.primaryColor;

        if (shieldButtonText != null)
            shieldButtonText.color = enemyData.primaryColor;

        UpdateButtonSelection();
    }

    void UpdateButtonSelection()
    {
        Color normal = enemyData.primaryColor;
        Color selected = enemyData.secondaryColor;

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

        LoadEnemyData();
        ApplyEnemyColors();
        UpdateItemText();
        StartPlayerTurn();
    }
}