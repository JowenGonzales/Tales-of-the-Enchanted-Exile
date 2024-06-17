using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    // Reference to the owner of the attack
    public GameObject owner;

    // Name of the animation to play during the attack
    [SerializeField] private string animationName;

    // Boolean flag indicating if it's a magic attack
    [SerializeField] private bool magicAttack;

    // Cost of magic for the attack
    [SerializeField] private float magicCost;

    // Multipliers for calculating damage
    [SerializeField] private float minAttackMultiplier;
    [SerializeField] private float maxAttackMultiplier;
    [SerializeField] private float minDefenseMultiplier;
    [SerializeField] private float maxDefenseMultiplier;

    private FighterStats attackerStats;
    private FighterStats targetStats;
    private float damage = 0.0f;

    private GameObject GameControllerObj;
    private AudioManager audioManager;

    // Initialize references
    private void Awake()
    {
        GameControllerObj = GameObject.Find("GameControllerObject");
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Perform the attack
    public void Attack(GameObject victim)
    {
        // Get stats of attacker and target
        attackerStats = owner.GetComponent<FighterStats>();
        targetStats = victim.GetComponent<FighterStats>();

        // Check if attacker has enough magic for the attack
        if (attackerStats.magic >= magicCost)
        {
            // Calculate damage based on multipliers
            float multiplier = Random.Range(minAttackMultiplier, maxAttackMultiplier);
            damage = multiplier * (magicAttack ? attackerStats.magicRange : attackerStats.attack);

            // Apply defense multiplier to reduce damage
            float defenseMultiplier = Random.Range(minDefenseMultiplier, maxDefenseMultiplier);
            damage = Mathf.Max(0, damage - (defenseMultiplier * targetStats.defense));

            // Play attack animation
            owner.GetComponent<Animator>().Play(animationName);

            // Inflict damage on the target
            targetStats.ReceiveDamage(Mathf.CeilToInt(damage));

            // Reduce attacker's magic
            attackerStats.UpdateMagicFill(magicCost);

            // Play attack sound effect
            audioManager.PlaySFX(magicAttack ? audioManager.magicCast : audioManager.attack);
        }
        else
        {
            // Display message if attacker doesn't have enough magic
            GameControllerObj.GetComponent<GameController>().enemyNoMagicText.gameObject.SetActive(true);

            // Skip turn and continue the game after a delay
            Invoke(nameof(SkipTurnContinueGame), 2);
        }
    }

    // Skip turn and continue the game
    private void SkipTurnContinueGame()
    {
        GameControllerObj.GetComponent<GameController>().NextTurn();
    }
}
