using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public List<GameObject> powerUpSpawnPoints;
    private GameObject currentPowerUp;
    private int spawnIndex;
    private string powerUpNameInResourcesFolder;
    // Start is called before the first frame update
    void Start()
    {
        GenerateRandomSpawnLocation();
    }

    // Update is called once per frame
    
    public void GenerateRandomSpawnLocation()
    {
        spawnIndex = Random.Range(0, powerUpSpawnPoints.Count);
        int randPowerUp = Random.Range(0, 3);
        if(randPowerUp == 0)
        {
            powerUpNameInResourcesFolder = "SpeedBoost";
        }
        else if(randPowerUp == 1)
        {
            powerUpNameInResourcesFolder = "TimeAdd";
        }
        else
        {
            powerUpNameInResourcesFolder = "JumpBoost";
        }
        currentPowerUp = Instantiate(Resources.Load<GameObject>(powerUpNameInResourcesFolder));
        currentPowerUp.transform.position = powerUpSpawnPoints[spawnIndex].transform.position;
    }
}
