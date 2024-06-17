using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyAction : MonoBehaviour
{
    // References to player and enemy game objects
    private GameObject hero;
    private GameObject enemy;

    // Prefabs for attack and range attack
    [SerializeField]
    private GameObject attackPrefab;

    [SerializeField]
    private GameObject rangePrefab;

    // Icon representing the face of the enemy (if used in UI)
    [SerializeField]
    private Sprite faceIcon;

    // Reference to the current attack object (if needed for tracking)
    private GameObject currentAttack;

    // Reference to the AudioManager for playing sound effects
    private AudioManager audioManager;

    private GameObject GameControllerObj;

    // Animator component for handling animations
    [SerializeField]
    private Animator animator;

    // Initialize references to player, enemy, and AudioManager
    private void Awake()
    {
        hero = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        GameControllerObj = GameObject.Find("GameControllerObject");
    }

    // Handle attack selection based on button input
    public void SelectAttack(string btn)
    {
        Debug.Log($"Fighter selected to {btn}");

        // Determine the target of the attack
        GameObject victim = hero;
        if (tag == "Player")
        {
            victim = enemy;
        }

        // Perform action based on button input
        switch (btn)
        {
            case "attack":
                // Execute attack action
                attackPrefab.GetComponent<AttackScript>().Attack(victim);
                audioManager.PlaySFX(audioManager.attack);
                if (tag == "Player")
                {
                    animator.Play("attackElara");
                }
                    break;

            case "range":
                // Execute range attack action
                rangePrefab.GetComponent<AttackScript>().Attack(victim);
                    if (tag == "Player")
                    {
                    GameControllerObj.GetComponent<GameController>().waterMagic.gameObject.SetActive(true);
                    animator.Play("rangeElara");   
                    }
                break;

            case "run":
                // Play run sound effect
                audioManager.PlaySFX(audioManager.run);
                SceneManager.LoadScene(2);
                break;

            default:
                Debug.LogWarning($"Unknown action: {btn}");
                break;
        }
    }
}
