using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    private List<FighterStats> fighterStats; // List to store fighter stats

    private GameObject battleMenu; // Reference to the battle menu UI

    // Text objects for displaying battle messages
    [Header("BattleScene")]
    public TMP_Text battleText;
    public TMP_Text victoryText;
    public TMP_Text playerDefeatedText;
    public TMP_Text enemyNoMagicText;
    public GameObject waterMagic;

    // Initialize references
    private void Awake()
    {
        battleMenu = GameObject.Find("ActionMenu");
    }

    // Initialize fighter stats and start the battle
    void Start()
    { 

        fighterStats = new List<FighterStats>();

        // Get player and enemy stats
        GameObject hero = GameObject.FindGameObjectWithTag("Player");
        FighterStats currentFighterStats = hero.GetComponent<FighterStats>();
        currentFighterStats.CalculateNextTurn(0);
        fighterStats.Add(currentFighterStats);

        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        FighterStats currentEnemyStats = enemy.GetComponent<FighterStats>();
        currentEnemyStats.CalculateNextTurn(0);
        fighterStats.Add(currentEnemyStats);

        // Sort fighters based on turn order
        fighterStats.Sort();

        // Start the battle
        NextTurn();
    }

    // Proceed to the next turn
    public void NextTurn()
    {
        // Hide battle messages
        battleText.gameObject.SetActive(false);
        victoryText.gameObject.SetActive(false);
        playerDefeatedText.gameObject.SetActive(false);
        enemyNoMagicText.gameObject.SetActive(false);
        waterMagic.gameObject.SetActive(false);

        // Get the next fighter in turn order
        FighterStats currentFighterStats = fighterStats[0];
        fighterStats.Remove(currentFighterStats);

        // Check if the fighter is not dead
        if (!currentFighterStats.GetDead())
        {
            // Update fighter's turn and re-add to the list
            GameObject currentUnit = currentFighterStats.gameObject;
            currentFighterStats.CalculateNextTurn(currentFighterStats.nextTurn);
            fighterStats.Add(currentFighterStats);
            fighterStats.Sort();

            // Activate battle menu for player's turn, otherwise select a random attack for enemy
            if (currentUnit.tag == "Player")
            {
                battleMenu.SetActive(true);
            }
            else
            {
                battleMenu.SetActive(false);
                string attackType = Random.Range(0, 2) == 1 ? "range" : "attack";
                currentUnit.GetComponent<EnemyAction>().SelectAttack(attackType);
            }
        }
        else
        {
            // Skip turn if fighter is dead
            NextTurn();
        }
    }
}
