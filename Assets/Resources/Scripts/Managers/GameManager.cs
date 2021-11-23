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
    }

    void Update()
    {
        
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
            tanksList[i].transform.Rotate(0, Random.Range(0, 359), 0);

            tanksList[i].name = isEnemy ? $"PlayerKit {i+1}" : $"PlayerKit {i+1}";

            GameObject playerTank = tanksList[i].transform.Find("PlayerTank").gameObject;

            string[] partStrArr = new string[]{"Body", "Tower", "Canon"};
            for (int j=0; j<partStrArr.Length; j++)
            {
                MeshRenderer meshRenderer = playerTank.transform.Find(partStrArr[j]).GetComponent<MeshRenderer>();
                meshRenderer.materials = new Material[] {materials[isEnemy ? 4 : i]};
            }
        }
    }
}
