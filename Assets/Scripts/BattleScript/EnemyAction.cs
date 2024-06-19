using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAction : MonoBehaviour
{
    private GameObject hero;
    private GameObject enemy;
    private GameObject battleMessage;


    private AudioManager audioManager;
    private Animator CompanionAnimator;
    private GameObject GameControllerObj;
    private CollisionDetection collisionDetection;
    private CameraShake cameraShake;

    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private GameObject rangePrefab;
    [SerializeField] private GameObject transition;

    private void Awake()
    {
        hero = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        battleMessage= GameObject.FindGameObjectWithTag("BattleMessage");


        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        GameControllerObj = GameObject.Find("GameControllerObject");
        CompanionAnimator = GetComponent<Animator>(); // Assuming this script is attached to the same GameObject as the Animator
        cameraShake = Camera.main.GetComponent<CameraShake>(); // Assuming CameraShake is attached to the main camera

        // Initialize the transition object, ensure it's assigned in the inspector or find it in the scene
        if (transition == null)
        {
            transition = GameObject.Find("TransitionObjectName"); // Replace with the actual name of your transition object
        }
    }

    private void Start()
    {
        collisionDetection = FindObjectOfType<CollisionDetection>();
    }

    public void SelectAttack(string btn)
    {
        GameObject victim = hero;
        if (tag == "Player")
        {
            victim = enemy;
        }

        switch (btn)
        {
            case "attack":
                StartCoroutine(MoveHeroAndAttack(victim));
                break;

            case "range":
                rangePrefab.GetComponent<AttackScript>().Attack(victim);
                if (tag == "Player")
                {
                    GameControllerObj.GetComponent<GameController>().waterMagic.gameObject.SetActive(true);
                    CompanionAnimator.Play("rangeElara");
                }
                break;

            case "run":
                StartCoroutine(RunToOverworld());
                break;

            default:
                Debug.LogWarning($"Unknown action: {btn}");
                break;
        }
    }

    IEnumerator MoveHeroAndAttack(GameObject victim)
    {
        Vector3 originalPosition = hero.transform.position;
        Vector3 offset = (victim.transform.position - originalPosition).normalized * 2;
        Vector3 targetPosition = victim.transform.position - offset;

        float moveDuration = 0.5f; // Duration for fast initial movement
        yield return StartCoroutine(MoveHeroOverTime(hero.transform, targetPosition, moveDuration));

        // Set hero's sorting order higher than the enemy's
        SpriteRenderer heroRenderer = hero.GetComponent<SpriteRenderer>();
        SpriteRenderer enemyRenderer = enemy.GetComponent<SpriteRenderer>();

        if (heroRenderer != null && enemyRenderer != null)
        {
            heroRenderer.sortingOrder = enemyRenderer.sortingOrder + 1;
        }

        // Shake the camera
        if (cameraShake != null)
        {
            StartCoroutine(cameraShake.Shake(0.2f, 0.3f)); // Shake for 0.2 seconds with intensity of 0.3 units
        }

        // Play attack animation and perform attack
        CompanionAnimator.Play("attackElara");
        attackPrefab.GetComponent<AttackScript>().Attack(victim);

        // Wait for attack animation or action to finish (adjust timing as needed)
        yield return new WaitForSeconds(0.5f); // Adjust this wait time based on your animation length or attack duration

        // Move the hero back to original position
        moveDuration = 1.0f; // Duration for slower return movement
        yield return StartCoroutine(MoveHeroOverTime(hero.transform, originalPosition, moveDuration));

        // Reset hero's sorting order if needed
        if (heroRenderer != null && enemyRenderer != null)
        {
            heroRenderer.sortingOrder = enemyRenderer.sortingOrder - 1; // Reset or adjust accordingly
        }
    }

    IEnumerator MoveHeroOverTime(Transform heroTransform, Vector3 targetPosition, float duration)
    {
        float timeElapsed = 0f;
        Vector3 startingPosition = heroTransform.position;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            heroTransform.position = Vector3.Lerp(startingPosition, targetPosition, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        heroTransform.position = targetPosition;
    }

    IEnumerator RunToOverworld()
    {
        audioManager.PlaySFX(audioManager.run);

        // Activate transition if available
        if (transition != null)
        {
            transition.SetActive(true);
        }

        if (collisionDetection != null)
        {
            collisionDetection.HandleBattleOutcome(false);
        }

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(3);
    }
}
