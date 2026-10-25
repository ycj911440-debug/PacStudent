using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private const int MapWidth = 28;
    private const int MapHeight = 29;

    private int[,] levelMap = new int[MapHeight, MapWidth];
    
    private void Awake()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        for (int i = 0; i < walls.Length; i++)
        {
            GameObject wall = walls[i];
            int column = Mathf.RoundToInt(wall.transform.position.x);
            int row = Mathf.RoundToInt(-wall.transform.position.y);

            if (row >= 0 && row < MapHeight && column >=0 && column < MapWidth)
            {
                levelMap[row, column] = 1;
            }
        }
    }
    
    public bool IsWalkable(Vector3 position)
    {
        int column = Mathf.RoundToInt(position.x);
        int row = Mathf.RoundToInt(-position.y);

        if (row < 0 || row >= MapHeight || column < 0 || column >= MapWidth)
        {
            return false;
        }

        return levelMap[row, column] == 0;
    }

    
}
