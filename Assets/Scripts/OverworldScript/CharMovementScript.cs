using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovementScript : MonoBehaviour
{
    public float moveSpeed;
    public Transform meryllTransform; // Reference to Meryll's Transform
    public FollowScript meryllFollowScript; // Reference to Meryll's follow script

    private bool isMoving;
    private Vector2 input;
    private Animator animator;
    public LayerMask solidObjectsLayer;
    private Vector2 lastDirection;
    private SpriteRenderer elaraRenderer;
    private SpriteRenderer meryllRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        elaraRenderer = GetComponent<SpriteRenderer>();
        meryllRenderer = meryllTransform.GetComponent<SpriteRenderer>();
        LoadPositions(); // Load positions on awake
    }

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0; // Prevent diagonal movement

            if (input != Vector2.zero)
            {
                lastDirection = input; // Store the last direction of movement
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                Vector3 targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (isWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        // Notify Meryll to start following Elara
        meryllFollowScript.FollowTarget(transform.position, lastDirection);

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        // Update sorting order based on movement direction
        UpdateSortingOrder();

        isMoving = false;
    }

    private bool isWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null)
        {
            return false;
        }

        return true;
    }

    private void UpdateSortingOrder()
    {
        if (lastDirection.y > 0) // Moving up
        {
            elaraRenderer.sortingOrder = 0;
            meryllRenderer.sortingOrder = 1;
        }
        else if (lastDirection.y < 0) // Moving down
        {
            elaraRenderer.sortingOrder = 1;
            meryllRenderer.sortingOrder = 0;
        }
    }

    public void SavePositions()
    {
        PlayerPrefs.SetFloat("ElaraPosX", transform.position.x);
        PlayerPrefs.SetFloat("ElaraPosY", transform.position.y);
        PlayerPrefs.SetFloat("MeryllPosX", meryllTransform.position.x);
        PlayerPrefs.SetFloat("MeryllPosY", meryllTransform.position.y);
        PlayerPrefs.Save();
    }

    public void LoadPositions()
    {
        if (PlayerPrefs.HasKey("ElaraPosX") && PlayerPrefs.HasKey("ElaraPosY") &&
            PlayerPrefs.HasKey("MeryllPosX") && PlayerPrefs.HasKey("MeryllPosY"))
        {
            Vector3 elaraPosition = new Vector3(PlayerPrefs.GetFloat("ElaraPosX"), PlayerPrefs.GetFloat("ElaraPosY"), transform.position.z);
            Vector3 meryllPosition = new Vector3(PlayerPrefs.GetFloat("MeryllPosX"), PlayerPrefs.GetFloat("MeryllPosY"), meryllTransform.position.z);
            transform.position = elaraPosition;
            meryllTransform.position = meryllPosition;
        }
    }
}
