using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    public float followSpeed;
    private Vector3 targetPos;
    private bool isFollowing;
    private Animator animator;
    private Vector2 lastDirection;
    private SpriteRenderer meryllRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        meryllRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isFollowing)
        {
            if ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, followSpeed * Time.deltaTime);
                Vector3 direction = (targetPos - transform.position).normalized;

                animator.SetFloat("moveX", direction.x);
                animator.SetFloat("moveY", direction.y);
                animator.SetBool("isMoving", true);
            }
            else
            {
                transform.position = targetPos;
                isFollowing = false;
                animator.SetBool("isMoving", false);

                // Set the correct idle animation direction based on the last direction
                animator.SetFloat("moveX", lastDirection.x);
                animator.SetFloat("moveY", lastDirection.y);
            }
        }
    }

    public void FollowTarget(Vector3 elaraPosition, Vector2 direction)
    {
        // Set the target position to Elara's previous position
        targetPos = elaraPosition;
        isFollowing = true;
        lastDirection = direction; // Store the direction to set the idle animation correctly
    }
}