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

    private int currentPlayer;
    private GameObject currentPlayerGO;
    private PlayerController currentPlayerController;

    private bool hasToSetStartState;

    private float timeAbleToShoot;
    private bool waitingToShoot;

    private bool waitingForBullet;

    private string message;
    
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

        Shuffle(playerTanks);

        hasToSetStartState = true;
        currentPlayer = 0;
        currentPlayerGO = null;
        currentPlayerController = null;
    }

    void Update()
    {
        // Wait for tanks to set their state
        if (hasToSetStartState && Time.frameCount > 1)
        {
            for (int i=0; i<playerTanks.Count; i++)
            {
                playerTanks[i].transform.Find("PlayerTank").GetComponent<PlayerController>().enabled = false;
            }

            for (int i=0; i<computerTanks.Count; i++)
            {
                computerTanks[i].transform.Find("PlayerTank").GetComponent<PlayerController>().enabled = false;
            }

            hasToSetStartState = false;
        }

        // Activate current player in turn
        if (!hasToSetStartState && currentPlayerGO == null)
        {
            playerTanks[currentPlayer].transform.Find("Back Camera").gameObject.SetActive(true);

            currentPlayerGO = playerTanks[currentPlayer].transform.Find("PlayerTank").gameObject;
            currentPlayerController = currentPlayerGO.GetComponent<PlayerController>();
            currentPlayerController.enabled = true;

            timeAbleToShoot = Time.time + 3;
            waitingToShoot = true;
            waitingForBullet = true;
        }

        // Wait to be able to shoot
        if (Time.time > timeAbleToShoot && waitingToShoot)
        {
            currentPlayerController.ableToShoot = true;
            waitingToShoot = false;
        }

        // When player shoots and bullets crashes deactivate player and go to next one
        if (
            currentPlayerGO != null && 
            !waitingToShoot &&
            !currentPlayerController.ableToShoot &&
            !waitingForBullet)
        {
            Debug.Log(message);

            playerTanks[currentPlayer].transform.Find("Back Camera").gameObject.SetActive(false);
            currentPlayerController.enabled = false;

            currentPlayerGO = null;
            currentPlayerController = null;

            currentPlayer = (currentPlayer + 1) % playerTanks.Count;
        }
    }

    void InstantiateTanks(List<GameObject> tanksList, int numOfElements, bool isEnemy)
    {
        for (int i=0; i<numOfElements; i++)
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

            tanksList[i].name = isEnemy ? $"Enemy {i+1}" : $"PlayerKit {i+1}";

            tanksList[i].transform.Find("Back Camera").gameObject.SetActive(false);

            GameObject playerTank = tanksList[i].transform.Find("PlayerTank").gameObject;

            PlayerController playerController = playerTank.GetComponent<PlayerController>();
            playerController.TankRotAY = Random.Range(0, 359);

            string[] partStrArr = new string[]{"Body", "Tower", "Canon"};
            for (int j=0; j<partStrArr.Length; j++)
            {
                MeshRenderer meshRenderer = playerTank.transform.Find(partStrArr[j]).GetComponent<MeshRenderer>();
                meshRenderer.materials = new Material[] {materials[isEnemy ? 4 : i]};
            }
        }
    }

    void Shuffle(List<GameObject> array)
    {
        for (int i = 0; i < array.Count; i++)
        {
            int random = Random.Range(0, array.Count);
            GameObject temp = array[random];
            array[random] = array[i];
            array[i] = temp;
        }
    }
}
