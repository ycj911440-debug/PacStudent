using UnityEngine;
using UnityEngine.InputSystem;

public class PacStudentController : MonoBehaviour
{
    private Animator animator;

    public LevelGenerator levelGenerator;
    public AudioSource moveAudio;
    public float speed = 5f;
    private Vector3 lastInput;
    private Vector3 currentInput;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveTime;
    private float moveDuration;
    private bool isMoving;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        targetPosition = transform.position;
        moveDuration = 1f / speed;
        animator.speed = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            lastInput = Vector3.up;
        
        if (Input.GetKeyDown(KeyCode.S))
            lastInput = Vector3.down;
        
        if (Input.GetKeyDown(KeyCode.A))
            lastInput = Vector3.left;

        if (Input.GetKeyDown(KeyCode.D))
            lastInput = Vector3.right;
        
        if (isMoving == false)
        {
            Vector3 nextPosition = transform.position + lastInput;

            if (levelGenerator.IsWalkable(nextPosition))
            {
                currentInput = lastInput;
            }

            nextPosition = transform.position + currentInput;

            if (currentInput != Vector3.zero && levelGenerator.IsWalkable(nextPosition))
            {
                startPosition = transform.position;
                targetPosition = nextPosition;
                moveTime = 0f;
                isMoving = true;

                SetAnimationDirection();
                animator.speed = 1f;

                if (moveAudio.isPlaying == false)
                {
                    moveAudio.Play();
                }
            }
            else
            {
                animator.speed = 0f;

                if (moveAudio.isPlaying == true)
                {
                    moveAudio.Stop();
                }
            }
        }
        
        if (isMoving == true)
        {    
            Move();
        }
    }

    void Move()
    {
        moveTime += Time.deltaTime;
        float t = moveTime / moveDuration;
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);

        if (t >= 1f)
        {
            transform.position = targetPosition;
            isMoving = false;
        }
    }

    void SetAnimationDirection()
    {
        if (currentInput == Vector3.up)
        {
            animator.SetInteger("Direction", 0);
        }
        else if (currentInput == Vector3.down)
        {
            animator.SetInteger("Direction", 1);
        }
        else if (currentInput == Vector3.left)
        {
            animator.SetInteger("Direction", 2);
        }
        else if (currentInput == Vector3.right)
        {
            animator.SetInteger("Direction", 3);
        }
    }
}
