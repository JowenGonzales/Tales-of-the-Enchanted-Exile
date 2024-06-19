using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionDetection : MonoBehaviour
{
    public TransitionScript transition;
    private CharMovementScript charMovementScript;
    private GameObject collidedKnight; // To store the knight we collided with

    private void Start()
    {
        // Find the CharMovementScript instance in the scene
        charMovementScript = FindObjectOfType<CharMovementScript>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Knight") || collision.gameObject.CompareTag("Knight1") || collision.gameObject.CompareTag("Knight2") ||
            collision.gameObject.CompareTag("Knight3") || collision.gameObject.CompareTag("KnightElite1") || collision.gameObject.CompareTag("KnightElite2"))
        {
            collidedKnight = collision.gameObject; // Store the knight we collided with

            // Save positions slightly away from the collision point
            if (charMovementScript != null)
            {
                SavePositionsAwayFromKnight();
            }

            transition.endingSceneTransition.SetActive(true);
            StartCoroutine(goToBattleScene());
        }
        Debug.Log("Collision detected with: " + collision.gameObject.name);
    }

    private IEnumerator goToBattleScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(4);
    }

    private void SavePositionsAwayFromKnight()
    {
        // Save Elara's position slightly away from the collision point
        Vector3 elaraPosition = transform.position;
        elaraPosition += new Vector3(2f, 0, 0); // Move slightly to the right
        PlayerPrefs.SetFloat("ElaraPosX", elaraPosition.x);
        PlayerPrefs.SetFloat("ElaraPosY", elaraPosition.y);

        // Save Meryll's position based on Elara's new position
        Vector3 meryllPosition = transform.position;
        meryllPosition += new Vector3(2f, 0, 0); // Move slightly to the right
        PlayerPrefs.SetFloat("MeryllPosX", meryllPosition.x);
        PlayerPrefs.SetFloat("MeryllPosY", meryllPosition.y);

        PlayerPrefs.Save();
    }

    public void HandleBattleOutcome(bool isDefeated)
    {
        if (isDefeated)
        {
            // Destroy the knight in the overworld scene
            Destroy(collidedKnight);
        }
        else
        {
            // Save positions slightly away from the knight
            SavePositionsAwayFromKnight();
        }
    }
}
