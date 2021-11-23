using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Material[] materials;
    
    public GameObject playerTankPrefab;
    public GameObject spawnPointsGO;

    private List<GameObject> spawnPoints;
    
    private List<GameObject> playerTanks;
    private List<GameObject> computerTanks;

    public ProjectileController projectileController;
    
    void Start()
    {
        spawnPoints = new List<GameObject>();
        for (int i=0; i<spawnPointsGO.transform.childCount; i++)
        {
            spawnPoints.Add(spawnPointsGO.transform.GetChild(i).gameObject);
        }

        playerTanks = new List<GameObject>();
        InstantiateTanks(playerTanks, GeneralInfo.numOfPlayers, false);

        computerTanks = new List<GameObject>();
        InstantiateTanks(computerTanks, GeneralInfo.numOfIAs, true);

        projectileController = null;
    }

    void Update()
    {
        if(projectileController)
        {
            foreach (var tank in playerTanks)
            {
                GameObject playerTank = tank.transform.Find("PlayerTank").gameObject;
                if ( projectileController.CheckCollision(playerTank.GetComponent<PlayerController>()) )
                {
                    playerTank.GetComponent<PlayerHealth>().DecreaseHealth(projectileController.damage);
                    projectileController = null;
                    break;
                }
            }
        }
    }

    void InstantiateTanks(List<GameObject> tanksList, int numOfTanks, bool isEnemy)
    {
        for (int i=0; i<numOfTanks; i++)
        {
            int randomIndex = Random.Range(0, spawnPoints.Count - 1);
            Vector3 spawnPointsPos = spawnPoints[randomIndex].transform.position;
            spawnPoints.RemoveAt(randomIndex);

            tanksList.Add(
                Instantiate(
                    playerTankPrefab, 
                    new Vector3(
                        spawnPointsPos.x, 
                        -2.247196f, 
                        spawnPointsPos.z
                    ), 
                    transform.rotation
                )
            );

            tanksList[i].name = isEnemy ? $"PlayerKit {i+1}" : $"PlayerKit {i+1}";

            GameObject playerTank = tanksList[i].transform.Find("PlayerTank").gameObject;
            playerTank.GetComponent<PlayerController>().TankRotAY = Random.Range(0, 359);

            string[] partStrArr = new string[]{"Body", "Tower", "Canon"};
            for (int j=0; j<partStrArr.Length; j++)
            {
                MeshRenderer meshRenderer = playerTank.transform.Find(partStrArr[j]).GetComponent<MeshRenderer>();
                meshRenderer.materials = new Material[] {materials[isEnemy ? 4 : i]};
            }
        }
    }
}
