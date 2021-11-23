using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Material[] materials;
    
    public GameObject playerTankPrefab;
    public GameObject spawnPointsGO;

    private List<GameObject> spawnPoints;
    
    private List<GameObject> playerTanks;
    private List<GameObject> computerTanks;

    public ProjectileController projectileController;
    private int currentPlayer;
    private GameObject currentPlayerGO;
    private PlayerController currentPlayerController;

    private float timeAbleToShoot;
    private bool waitingToShoot;

    public bool waitingForBullet;

    public string message;
    
    private GameObject lastCamera;
    private GameObject lastUI;

    private float timeToNextTurn;

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
        
        Shuffle(playerTanks);

        currentPlayer = 0;
        currentPlayerGO = null;
        currentPlayerController = null;
    }

    void Update()
    {
        // Activate current player in turn
        if (currentPlayerGO == null && Time.time > timeToNextTurn)
        {
            if (lastCamera) lastCamera.SetActive(false);
            if (lastUI) 
            {   
                GameObject uiMessage = lastUI.transform.Find("HitOrMiss").gameObject;
                uiMessage.SetActive(false);
                lastUI.SetActive(false);
            }

            lastCamera = playerTanks[currentPlayer].transform.Find("Back Camera").gameObject;
            lastCamera.SetActive(true);

            lastUI = playerTanks[currentPlayer].transform.Find("PlayerUI").gameObject;
            lastUI.SetActive(true);

            currentPlayerGO = playerTanks[currentPlayer].transform.Find("PlayerTank").gameObject;
            currentPlayerController = currentPlayerGO.GetComponent<PlayerController>();
            currentPlayerController.enabled = true;

            timeAbleToShoot = Time.time + 3;
            waitingToShoot = true;
            waitingForBullet = true;

            message = "Miss!";

            WindController.SetRandomWind();
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
            GameObject uiMessage = playerTanks[currentPlayer].transform.Find("PlayerUI").Find("HitOrMiss").gameObject;
            uiMessage.SetActive(true);
            uiMessage.GetComponent<Text>().text = message;

            currentPlayerController.enabled = false;

            currentPlayerGO = null;
            currentPlayerController = null;

            currentPlayer = (currentPlayer + 1) % playerTanks.Count;

            timeToNextTurn = Time.time + 2;
        }

        if (projectileController)
        {
            foreach (var tank in playerTanks)
            {
                GameObject playerTank = tank.transform.Find("PlayerTank").gameObject;
                if (projectileController.CheckCollision(playerTank.GetComponent<PlayerController>()))
                {
                    playerTank.GetComponent<PlayerHealth>().DecreaseHealth(projectileController.damage);
                    Destroy(projectileController.gameObject);
                    message = "Hit!";
                    projectileController = null;
                    break;
                }
            }
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
            tanksList[i].transform.Find("PlayerUI").gameObject.SetActive(false);

            GameObject playerTank = tanksList[i].transform.Find("PlayerTank").gameObject;

            PlayerController playerController = playerTank.GetComponent<PlayerController>();
            playerController.TankRotAY = Random.Range(0, 359);
            playerController.enabled = false;

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
