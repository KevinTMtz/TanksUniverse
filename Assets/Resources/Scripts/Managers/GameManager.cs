using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Material[] materials;
    
    public GameObject playerTankPrefab;
    public GameObject spawnPointsGO;

    private List<GameObject> spawnPoints;
    
    private GameObject[] playerTanks;
    private GameObject[] computerTanks;
    
    void Start()
    {
        spawnPoints = new List<GameObject>();
        for (int i=0; i<spawnPointsGO.transform.childCount; i++)
        {
            spawnPoints.Add(spawnPointsGO.transform.GetChild(i).gameObject);
        }

        playerTanks = new GameObject[GeneralInfo.numOfPlayers];
        InstantiateTanks(playerTanks, false);

        computerTanks = new GameObject[GeneralInfo.numOfIAs];
        InstantiateTanks(computerTanks, true);

        Shuffle(playerTanks);
    }

    void InstantiateTanks(GameObject[] tanksList, bool isEnemy)
    {
        for (int i=0; i<tanksList.Length; i++)
        {
            int randomIndex = Random.Range(0, spawnPoints.Count - 1);
            Vector3 spawnPointsPos = spawnPoints[randomIndex].transform.position;
            spawnPoints.RemoveAt(randomIndex);

            tanksList[i] = Instantiate(
                playerTankPrefab, 
                new Vector3(
                    spawnPointsPos.x, 
                    -2.247196f, 
                    spawnPointsPos.z
                ), 
                transform.rotation
            );

            tanksList[i].name = isEnemy ? $"Enemy {i+1}" : $"PlayerKit {i+1}";

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

    void Shuffle(GameObject[] array)
    {
        for (int i = array.Length-1; i > 0; i--)
        {
            int r = Random.Range(0, i-1);
            GameObject temp = array[r];
            array[r] = array[i];
            array[i] = temp;
        }
    }
}
