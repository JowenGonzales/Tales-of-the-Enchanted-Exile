using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class FighterStats : MonoBehaviour, IComparable
{
    // Animator component for handling animations
    [SerializeField]
    private Animator animatorMeryll;

    // Animator component for handling animations
    [SerializeField]
    private Animator animatorElara;

    // UI elements for health and magic fill bars
    [SerializeField]
    private GameObject healthFill;

    [SerializeField]
    private GameObject magicFill;

    [Header("Stats")]
    public float health;
    public float magic;
    public float attack;
    public float magicRange;
    public float defense;
    public float speed;

    private float startHealth;
    private float startMagic;

    [HideInInspector]
    public int nextTurn;

    public bool dead = false;

    private Transform healthTransform;
    private Transform magicTransform;

    private Vector2 healthScale;
    private Vector2 magicScale;

    private float xNewHealthScale;
    private float xNewMagicScale;

    private GameObject GameControllerObj;
    private GameObject hero;
    private GameObject enemy;

    private AudioManager audioManager;

    // Initialize references and set initial values
    private void Awake()
    {
        healthTransform = healthFill.GetComponent<RectTransform>();
        healthScale = healthFill.transform.localScale;

        magicTransform = magicFill.GetComponent<RectTransform>();
        magicScale = magicFill.transform.localScale;

        startHealth = health;
        startMagic = magic;

        GameControllerObj = GameObject.Find("GameControllerObject");

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        hero = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Handle updates to magic fill
    public void UpdateMagicFill(float cost)
    {
        if (cost > 0)
        {
            magic -= cost;
            xNewMagicScale = magicScale.x * (magic / startMagic);
            magicFill.transform.localScale = new Vector2(xNewMagicScale, magicScale.y);
        }
    }

    // Check if the fighter is dead
    public bool GetDead()
    {
        return dead;
    }

    // Continue the game after a delay
    private void ContinueGame()
    {
        Debug.Log("ContinueGame called");
        GameControllerObj.GetComponent<GameController>().NextTurn();
    }

    // Calculate the next turn for the fighter
    public void CalculateNextTurn(int currentTurn)
    {
        nextTurn = currentTurn + Mathf.CeilToInt(100f / speed);
    }

    // Compare to another fighter to determine turn order
    public int CompareTo(object otherStats)
    {
        return nextTurn.CompareTo(((FighterStats)otherStats).nextTurn);
    }

    // Handle the death of the fighter
    private void HandleDeath()
    {
        dead = true;
        gameObject.tag = "Dead";
        Destroy(healthFill);

        if (hero.tag == "Player")
        {
            audioManager.PlaySFX(audioManager.victory);
            GameControllerObj.GetComponent<GameController>().victoryText.gameObject.SetActive(true);
            StartCoroutine(EndBattle());
        }
        else if (enemy.tag == "Enemy")
        {
            audioManager.PlaySFX(audioManager.death);
            GameControllerObj.GetComponent<GameController>().playerDefeatedText.gameObject.SetActive(true);
        }
    }

    // Coroutine to end the battle and load the next scene
    private IEnumerator EndBattle()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene(2);
    }


    // Update the health fill bar
    private void UpdateHealthFill()
    {
        xNewHealthScale = healthScale.x * (health / startHealth);
        healthFill.transform.localScale = new Vector2(xNewHealthScale, healthScale.y);
    }

    // Display the damage dealt
    private void DisplayDamage(float damage)
    {
        var battleText = GameControllerObj.GetComponent<GameController>().battleText;
        battleText.gameObject.SetActive(true);
        battleText.text = damage.ToString();
    }

    // Handle receiving damage
    public void ReceiveDamage(float damage)
    {
        health -= damage;
        if (tag == "Player")
        {
            animatorMeryll.Play("takeDamage");
            animatorElara.Play("takeDamageElara");
        }

        if (health <= 0)
        {
            HandleDeath();
        }
        else if (damage > 0)
        {
            UpdateHealthFill();
            DisplayDamage(damage);
        }

        Invoke(nameof(ContinueGame), 2);
    }

}
