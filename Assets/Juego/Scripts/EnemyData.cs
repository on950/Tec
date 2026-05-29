using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Battle/Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("Información")]
    public string enemyName;

    [TextArea]
    public string introText;

    public Sprite enemySprite;

    [Header("Stats")]
    public int maxHealth = 20;

    public bool usesBlueSoul;

    [Header("UI")]
    public Color primaryColor;

    public Color secondaryColor;

    [Header("Combate")]
    public GameObject[] attackSpawnerPrefabs;

    [TextArea]
    public string[] actDialogues;

    [TextArea]
    public string[] enemyTurnDialogues;

    [TextArea]
    public string itemDialogue;

    [TextArea]
    public string phase2Dialogue;

    [TextArea]
    public string phase3Dialogue;

    [Header("Colores Inventario")]
    public Image inventoryPanelImage;
    public Image batteryButtonImage;
    public Image formulaButtonImage;
    public Image shieldButtonImage;

    public Outline inventoryPanelOutline;
    public Outline batteryButtonOutline;
    public Outline formulaButtonOutline;
    public Outline shieldButtonOutline;
}