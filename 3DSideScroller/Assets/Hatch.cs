using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour {

    public PlayerManager playermanager;

    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject combatArea;

    List<Slicer> enemies = new List<Slicer>();
    public int maxNumEnemiesKilled;

    private int playerCount = 0;

    private enum HATCHSTATES {LEFTDOOROPEN, LEFTDOORCLOSED, ALLENEMIESDEAD, HATCHCLOSED };
    HATCHSTATES currentHatchState = HATCHSTATES.LEFTDOOROPEN;

    public GameObject slicerPrefab;
    public Transform spawnPos;

    bool isSpawning = false;
    bool waveSpawned = false;

    //public Image rightArrow;

     void Update()
    {

        switch (currentHatchState)
        {
            case HATCHSTATES.LEFTDOOROPEN:

                //


                break;
            case HATCHSTATES.LEFTDOORCLOSED:
                leftDoor.SetActive(true);
                
                if (!isSpawning && !waveSpawned )
                {

                    StartCoroutine(SpawnSlicers(5));
                }

                //print(enemies.Count);

                if (!isSpawning && waveSpawned)
                {
                    CheckForEnemiesKilled();
                }
              

                break;
            case HATCHSTATES.ALLENEMIESDEAD:

                //display the sign to close the hatch
                HasHatchBeenClosed();


                break;
            case HATCHSTATES.HATCHCLOSED:
                //turn off right door
                rightDoor.SetActive(false);
                //play the move right sign
                break;

        }
        


    }

    void CheckForPlayersInArea()
    {
        if (playerCount == playermanager.players.Count)
        {
            //all players are in area
           // print("gotem");
            currentHatchState = HATCHSTATES.LEFTDOORCLOSED;
        }

    }

    void CheckForEnemiesKilled()
    {
        //if players have killed x number of enemies
        foreach (Slicer slicer in enemies)
        {
            if (slicer.currState != Slicer.SlicerStates.DEAD)
            {
                return ;
            }
        }

        currentHatchState = HATCHSTATES.ALLENEMIESDEAD;
    }

    void HasHatchBeenClosed()
    {


        currentHatchState = HATCHSTATES.HATCHCLOSED;
    }


    public void ChangePlayerCount(int i)
    {
       
        playerCount += i;
       // print("NewHatchCount: " + playerCount);
        CheckForPlayersInArea();
    }

    IEnumerator SpawnSlicers(int numslicers)
    {
        isSpawning = true;
       
        int currentNumSlicers = 0;

        while (currentNumSlicers != numslicers)
        {
            currentNumSlicers++;
            GameObject newSlicer = Instantiate(slicerPrefab, spawnPos.position, Quaternion.identity);
            enemies.Add(newSlicer.GetComponent<Slicer>());
            yield return new WaitForSeconds(1.5f);
            print(currentNumSlicers);
        }

        waveSpawned = true;
        isSpawning = false;
         yield return null;
    }

}
