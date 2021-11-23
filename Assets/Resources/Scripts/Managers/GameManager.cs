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

    private int currentPlayer;
    private GameObject currentPlayerGO;
    private PlayerController currentPlayerController;

    private bool hasToSetStartState;

    private float timeAbleToShoot;
    private bool waitingToShoot;
    private bool waitingForBullet;

    public bool WaitingForBullet
    {
        set { waitingForBullet = value; }
    }
    
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

        hasToSetStartState = true;
        currentPlayer = 0;
        currentPlayerGO = null;
        currentPlayerController = null;
    }

    void Update()
    {
        if (Time.frameCount > 1 && hasToSetStartState)
        {
            for (int i=0; i<playerTanks.Length; i++)
            {
                playerTanks[i].transform.Find("PlayerTank").GetComponent<PlayerController>().enabled = false;
            }

            for (int i=0; i<computerTanks.Length; i++)
            {
                computerTanks[i].transform.Find("PlayerTank").GetComponent<PlayerController>().enabled = false;
            }

            hasToSetStartState = false;
        }

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

        if (Time.time > timeAbleToShoot && waitingToShoot)
        {
            currentPlayerController.ableToShoot = true;
            waitingToShoot = false;
        }

        if (
            currentPlayerGO != null && 
            !currentPlayerController.ableToShoot &&
            !waitingToShoot &&
            !waitingForBullet)
        {
            playerTanks[currentPlayer].transform.Find("Back Camera").gameObject.SetActive(false);
            currentPlayerController.enabled = false;

            currentPlayerGO = null;
            currentPlayerController = null;

            currentPlayer = (currentPlayer + 1) % playerTanks.Length;
        }
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

    void Shuffle(GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int random = Random.Range(0, array.Length);
            GameObject temp = array[random];
            array[random] = array[i];
            array[i] = temp;
        }
    }
}
