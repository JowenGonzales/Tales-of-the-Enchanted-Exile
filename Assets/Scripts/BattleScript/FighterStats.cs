using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class FighterStats : MonoBehaviour, IComparable
{

    [SerializeField]
    private Animator animatorMeryll;

    [SerializeField]
    private Animator animatorElara;

    [SerializeField]
    private Animator animatorEnemy;

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

    public TransitionScript transition;

    private CollisionDetection collisionDetection;

    private void Start()
    {
        collisionDetection = FindObjectOfType<CollisionDetection>();

    }

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

    public void UpdateMagicFill(float cost)
    {
        if (cost > 0)
        {
            magic -= cost;
            xNewMagicScale = magicScale.x * (magic / startMagic);
            magicFill.transform.localScale = new Vector2(xNewMagicScale, magicScale.y);
        }
    }

    public bool GetDead()
    {
        return dead;
    }

    private void ContinueGame()
    {
        Debug.Log("ContinueGame called");
        GameControllerObj.GetComponent<GameController>().NextTurn();
    }

    public void CalculateNextTurn(int currentTurn)
    {
        nextTurn = currentTurn + Mathf.CeilToInt(100f / speed);
    }

    public int CompareTo(object otherStats)
    {
        return nextTurn.CompareTo(((FighterStats)otherStats).nextTurn);
    }

    private void HandleDeath()
    {
        dead = true;
        gameObject.tag = "Dead";
        Destroy(healthFill);

        if (hero.tag == "Player")
        {
            audioManager.PlaySFX(audioManager.victory);
            GameControllerObj.GetComponent<GameController>().victoryText.gameObject.SetActive(true);
            if (collisionDetection != null)
            {
                collisionDetection.HandleBattleOutcome(true);
            }
            StartCoroutine(BattleVictory());
        }
        else if (enemy.tag == "Enemy")
        {
            audioManager.PlaySFX(audioManager.death);
            GameControllerObj.GetComponent<GameController>().playerDefeatedText.gameObject.SetActive(true);
            StartCoroutine(BattleLose());
        }
    }

    private IEnumerator BattleVictory()
    {
        yield return new WaitForSeconds(3);
        transition.endingSceneTransition.SetActive(true);
        SceneManager.LoadScene(3);
    }

    private IEnumerator BattleLose()
    {
        yield return new WaitForSeconds(3);
        transition.endingSceneTransition.SetActive(true);
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }

    private void UpdateHealthFill()
    {
        xNewHealthScale = healthScale.x * (health / startHealth);
        healthFill.transform.localScale = new Vector2(xNewHealthScale, healthScale.y);
    }

    private void DisplayDamage(float damage)
    {
        var battleText = GameControllerObj.GetComponent<GameController>().battleText;
        battleText.gameObject.SetActive(true);
        battleText.text = damage.ToString();
    }

    public void ReceiveDamage(float damage)
    {
        health -= damage;
        if (tag == "Player")
        {
            animatorMeryll.Play("takeDamageMeryll");
            animatorElara.Play("takeDamageElara");
        } else if (tag == "Enemy")
        {
            animatorEnemy.Play("takeDamageEnemy");
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
