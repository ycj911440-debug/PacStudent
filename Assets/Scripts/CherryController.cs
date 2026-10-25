using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab;
    public float moveSpeed = 4f;
    private GameObject currentCherry;
    private Vector3 levelCenter;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float spawnTimer = 5f;
    private float moveTime;
    private float moveDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelCenter = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCherry == null)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0f)
            {
                SpawnCherry();
            }
        }
        else
        {
            MoveCherry();
        }
    }

    void SpawnCherry()
    {
        int side = Random.Range(0, 4);
        Vector3 offset = Vector3.zero;

        if (side == 0)
        {
            offset = new Vector3(-15.5f, Random.Range(-14f, 14f), 0f);
        }
        else if (side == 1)
        {
            offset = new Vector3(15.5f, Random.Range(-14f, 14f), 0f);
        }
        else if (side == 2)
        {
            offset = new Vector3(Random.Range(-13.5f, 13.5f), 16f, 0f);
        }
        else if (side == 3)
        {
            offset = new Vector3(Random.Range(-13.5f, 13.5f), -16f, 0f);
        }

        startPosition = levelCenter - offset;
        currentCherry = Instantiate(cherryPrefab, startPosition, Quaternion.identity);
        spawnTimer = 5f;
        float distance = Vector3.Distance(startPosition, endPosition);
        moveDuration = distance / moveSpeed;
        moveTime = 0f;
    }

    void MoveCherry()
    {
        moveTime += Time.deltaTime;
        float t = moveTime / moveDuration;
        currentCherry.transform.position = Vector3.Lerp(startPosition, endPosition, t);

        if (t >= 1f)
        {
            currentCherry.transform.position = endPosition;
            Destroy(currentCherry);
            currentCherry = null;
            spawnTimer = 5f;
        }
    }
}
