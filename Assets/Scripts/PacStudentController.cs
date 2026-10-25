using UnityEngine;
using UnityEngine.InputSystem;

public class PacStudentController : MonoBehaviour
{
    private Animator animator;

    public LevelGenerator levelGenerator;
    public HUDController hudController;
    public AudioSource moveAudio;
    public AudioSource pelletAudio;
    public AudioSource powerPelletAudio;
    public AudioSource cherryAudio;
    public AudioSource wallCollsionAudio;
    public ParticleSystem movementParticles;
    public ParticleSystem wallHitParticles;
    public float speed = 5f;
    private Vector3 lastInput;
    private Vector3 currentInput;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveTime;
    private float moveDuration;
    private bool isMoving;
    private bool wallHitPlayed;

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
            wallHitPlayed = false;
        
        if (Input.GetKeyDown(KeyCode.S))
            lastInput = Vector3.down;
            wallHitPlayed = false;
        
        if (Input.GetKeyDown(KeyCode.A))
            lastInput = Vector3.left;
            wallHitPlayed = false;

        if (Input.GetKeyDown(KeyCode.D))
            lastInput = Vector3.right;
            wallHitPlayed = false;
        
        if (isMoving == false)
        {
            Vector3 nextPosition = transform.position + lastInput;

            if (levelGenerator.IsWalkable(nextPosition))
            {
                currentInput = lastInput;
            }
            
            CheckTunnel();
            nextPosition = transform.position + currentInput;

            if (currentInput != Vector3.zero && levelGenerator.IsWalkable(nextPosition))
            {
                startPosition = transform.position;
                targetPosition = nextPosition;
                moveTime = 0f;
                isMoving = true;

                SetAnimationDirection();
                animator.speed = 1f;

                PlayMovementAudio();
                PlayMovementParticles();
            }
            else
            {
                animator.speed = 0f;
                StopMovementAudio();
                StopMovementParticles();
                Vector3 hitDirection = currentInput;

                if (hitDirection == Vector3.zero)
                {
                    hitDirection = lastInput;
                }

                if (hitDirection != Vector3.zero && wallHitPlayed == false)
                {
                    PlayWallHit(hitDirection);
                    wallHitPlayed = true;
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

    bool HasPellet(Vector3 position)
    {
        GameObject[] pellets = GameObject.FindGameObjectsWithTag("Pellet");

        for (int i = 0; i < pellets.Length; i++)
        {
            float distance = Vector3.Distance(pellets[i].transform.position, position);

            if (distance < 0.1f)
            {
                return true;
            }
        }

        GameObject[] powerPellets = GameObject.FindGameObjectsWithTag("PowerPellet");

        for (int i = 0; i < powerPellets.Length; i++)
        {
            float distance = Vector3.Distance(powerPellets[i].transform.position, position);

            if (distance < 0.1f)
            {
                return true;
            }
        }

        return false;
    }

    void PlayMovementAudio()
    {
        if (HasPellet(targetPosition) == true)
        {
            if (moveAudio.isPlaying == true)
            {
                moveAudio.Stop();
            }

            if (pelletAudio.isPlaying == false)
            {
                pelletAudio.Play();
            } 
        }
        else
        {
            if (pelletAudio.isPlaying == true)
            {
                pelletAudio.Stop();
            }

            if (moveAudio.isPlaying == false)
            {
                moveAudio.Play();
            }
        }
    }

    void StopMovementAudio()
    {
        if (moveAudio.isPlaying == true)
        {
            moveAudio.Stop();
        }

        if (pelletAudio.isPlaying == true)
        {
            pelletAudio.Stop();
        }
    }

    void PlayMovementParticles()
    {
        if (movementParticles.isPlaying == false)
        {
            movementParticles.Play();
        }
    }

    void StopMovementParticles()
    {
        if (movementParticles.isPlaying == true)
        {
            movementParticles.Stop();
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Pellet"))
        {
            Destroy(otherCollider.gameObject);
            hudController.AddScore(10);
        }
        else if (otherCollider.CompareTag("PowerPellet"))
        {
            Destroy(otherCollider.gameObject);
            hudController.AddScore(50);

            if (powerPelletAudio.isPlaying == false)
            {
                powerPelletAudio.Play();
            }
        }
        else if (otherCollider.CompareTag("Cherry"))
        {
            Destroy(otherCollider.gameObject);
            hudController.AddScore(100);

            if (cherryAudio.isPlaying == false)
            {
                cherryAudio.Play();
            }
        }
    }

    void PlayWallHit(Vector3 hitDirection)
    {
        wallHitParticles.transform.position = transform.position + hitDirection * 0.5f;
        wallHitParticles.Stop();
        wallHitParticles.Play();

        if (wallCollsionAudio.isPlaying == true)
        {
            wallCollsionAudio.Stop();
        }

        wallCollsionAudio.Play();
    }

    void CheckTunnel()
    {
        int currentY = Mathf.RoundToInt(transform.position.y);

        if (currentY == -14)
        {
            if (transform.position.x <= 0f && currentInput == Vector3.left)
            {
                transform.position = new Vector3(27f, -14f, transform.position.z);
                startPosition = transform.position;
                targetPosition = transform.position;
            }
            else if (transform.position.x >= 27f && currentInput == Vector3.right)
            {
                transform.position = new Vector3(0f, -14f, transform.position.z);
                startPosition = transform.position;
                targetPosition = transform.position;
            }
        }
    }
}
