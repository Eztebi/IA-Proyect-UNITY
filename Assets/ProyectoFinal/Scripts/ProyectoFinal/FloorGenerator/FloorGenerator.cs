using System.Collections;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public int width = 100;
    public int height = 100;

    [Range(0f, 1f)]
    public float lowChance = 0.4f;

    [Range(0f, 1f)]
    public float mediumChance = 0.35f;

    public int simulationSteps = 5;
    public GameObject cellPrefab;

    private FloorCell[,] grid;
    private FloorType[,] nextState;

    void Start()
    {
        grid = new FloorCell[width, height];
        nextState = new FloorType[width, height];

        GenerateGrid();
        StartCoroutine(SimulateFloor());
    }
    void GenerateGrid()
    {
        Renderer prefabRenderer = cellPrefab.GetComponent<Renderer>();
        Vector3 cellSize = prefabRenderer.bounds.size;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 cellpos = new Vector3(x * cellSize.x,0,y * cellSize.z);
                GameObject obj = Instantiate(cellPrefab,cellpos,transform.rotation,transform);

                FloorCell cell = obj.GetComponent<FloorCell>();

                FloorType randomType = GetRandomFloorType();
                cell.SetState(randomType);

                grid[x, y] = cell;
            }
        }
    }
    FloorType GetRandomFloorType()
    {
        float rand = Random.value;

        if (rand < lowChance)
            return FloorType.low;
        else if (rand < lowChance + mediumChance)
            return FloorType.medium;
        else
            return FloorType.high;
    }
    void Simulate()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                nextState[x, y] = FloorNeighCount(x, y);
            }
        }
        ApplyNextState();
    }
    FloorType FloorNeighCount(int x, int y)
    {
        int low = 0;
        int medium = 0;
        int high = 0;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                int nx = x + dx;
                int ny = y + dy;

                if (nx < 0 || ny < 0 || nx >= width || ny >= height)
                    continue;

                switch (grid[nx, ny].type)
                {
                    case FloorType.low:
                        low++;
                        break;
                    case FloorType.medium:
                        medium++;
                        break;
                    case FloorType.high:
                        high++;
                        break;
                }
            }
        }
        if (low >= medium && low >= high)
            return FloorType.low;
        if (medium >= low && medium >= high)
            return FloorType.medium;
        return FloorType.high;
    }
    void ApplyNextState()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y].SetState(nextState[x, y]);
            }
        }
    }
    IEnumerator SimulateFloor()
    {
        for (int i = 0; i < simulationSteps; i++)
        {
            Simulate();
            yield return new WaitForSeconds(0.5f);
        }
    }
}