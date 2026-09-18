using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{   
    private Animator animator;
    public AudioSource moveAudio;

    public float speed = 2f;

    private Vector3[] spots =
    {
        new Vector3(1.5f, -0.5f, 0f),
        new Vector3(6.5f, -0.5f, 0f),
        new Vector3(6.5f, -4.5f, 0f),
        new Vector3(1.5f, -4.5f, 0f)
    };

    private int currentSpot = 0;
    private float time = 0f;
    private float duration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = spots [0];
        animator = GetComponent<Animator>();
        animator.SetInteger("Direction", 3);
        moveAudio.Play();
        duration = Vector3.Distance(spots [0], spots [1]) / speed;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        float t = time / duration;
        transform.position = Vector3.Lerp(spots [currentSpot], spots [(currentSpot + 1) % spots.Length], t);

        if (t >= 1f)
        {
            currentSpot = (currentSpot + 1) % spots.Length;
            Vector3 direction = spots [(currentSpot + 1) % spots.Length] - spots[currentSpot];
            if (direction.x > 0)
                animator.SetInteger("Direction", 3);
            else if (direction.x < 0)
                animator.SetInteger("Direction", 2);
            else if (direction.y > 0)
                animator.SetInteger("Direction", 0);
            else if (direction.y < 0)
                animator.SetInteger("Direction", 1);
            time = 0f;
            duration = Vector3.Distance(spots[currentSpot], spots[(currentSpot + 1) % spots.Length]) / speed;
        }
    }
}
