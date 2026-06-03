using System.Collections;
using UnityEngine;

public class BosqueCelular : MonoBehaviour
{
    public int height = 30;
    public int width = 30;

    public GameObject celulaPrefab;

    private CelulaBosque[,] grid;
    private EstadoCelula[,] nextState;

    public float spreadChance = 0.4f;
    public float spontaneousFireChance = 0.001f;
    
    private void Start()
    {
        grid = new CelulaBosque[width, height];
        nextState = new EstadoCelula[width, height];

        GenerateGrid();
        StartCoroutine(SimulationLoop());
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject obj = Instantiate(celulaPrefab);
                obj.transform.position = new Vector3(x, y, 0);
                obj.transform.parent = this.transform;

                CelulaBosque cel = obj.GetComponent<CelulaBosque>();
                if(Random.value > 0.3)
                {
                    cel.SetState(EstadoCelula.Tree);
                }
                else
                {
                    cel.SetState(EstadoCelula.Empty);
                }
                grid[x, y] = cel;
            }
        }
    }

    int CountNeighbors(int x, int y,EstadoCelula state)
    {
        int count = 0;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (grid[nx, ny].currentState == state) count++;
                }
            }
        }
        return count;

    }
    void Simulate()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CelulaBosque celula = grid[x, y];

                switch (celula.currentState)
                {
                    case EstadoCelula.Empty:
                        nextState[x,y] = EstadoCelula.Empty;
                        break;
                    case EstadoCelula.Tree:
                        int burningNeigh = CountNeighbors(x, y, EstadoCelula.Burning);

                        if(burningNeigh > 0 && Random.value< spreadChance)
                        {
                            nextState[x, y] = EstadoCelula.Burning;
                        }
                        else if(Random.value < spontaneousFireChance)
                        {
                            nextState[x, y] = EstadoCelula.Burning;
                        }
                        else
                        {
                            nextState[x, y] = EstadoCelula.Tree;
                        }
                        break;
                    case EstadoCelula.Burning:
                        nextState[x, y] = EstadoCelula.Ash;
                        break;
                    case EstadoCelula.Ash:
                        int treeNeigh= CountNeighbors(x, y,EstadoCelula.Tree);
                        if(treeNeigh >= 3)
                        {
                            nextState[x, y] = EstadoCelula.Tree;
                        }
                        else
                        {
                            nextState[x, y] = EstadoCelula.Ash;
                        }
                        break;
                }
            }
        }
        ApplyNextState();
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
    IEnumerator SimulationLoop()
    {
        while (true)
        {
            Simulate();
            yield return new WaitForSeconds(.3f);
        }
    }
}

