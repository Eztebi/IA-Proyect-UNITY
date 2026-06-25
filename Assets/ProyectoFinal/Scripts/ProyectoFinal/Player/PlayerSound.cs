using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private PlayerMovement playerRef;
    [SerializeField] public GameObject noiseGO;

    private void Awake()
    {
        playerRef = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if(playerRef.GetNoise() <= 0)
        {
            noiseGO.SetActive(false);
        }
        else
        {
            noiseGO.SetActive(true);
            noiseGO.transform.localScale = new Vector3(playerRef.GetNoise() * playerRef.GetNoiseRaidus(), noiseGO.transform.localScale.y, playerRef.GetNoise() * playerRef.GetNoiseRaidus());
        }
    }
}
