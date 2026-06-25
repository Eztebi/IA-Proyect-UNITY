using UnityEngine;

public enum FloorType
{
    low,
    medium,
    high
}
public class FloorCell : MonoBehaviour
{

    
    Renderer rend;
    public FloorType type;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void SetState(FloorType floor)
    {
        type = floor;
        switch (floor)
        {
            case FloorType.low:
                rend.material.color = Color.blue;
                break;
            case FloorType.medium:
                rend.material.color = Color.yellow;
                break;
            case FloorType.high:
                rend.material.color = Color.orange;
                break;
        }
    }
}
