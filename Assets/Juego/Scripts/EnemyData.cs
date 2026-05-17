using UnityEngine;

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
    public GameObject attackSpawnerPrefab;

    [TextArea]
    public string[] actDialogues;

    [TextArea]
    public string[] enemyTurnDialogues;
}