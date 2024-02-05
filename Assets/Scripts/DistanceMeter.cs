using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceMeter : MonoBehaviour
{
    public Text playerName, playerPos;
    public string[] playerNames;
    public float distance;
    public int positionInRace = 1;
    void Start()
    {
        if (tag == "Enemy")
        {
            playerName.text = playerNames[Random.Range(0, playerNames.Length)];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (positionInRace < 4)

            switch (positionInRace)
            {
                case 1:
                    {
                        playerPos.text = "1st";
                        break;
                    }
                case 2:
                    {
                        playerPos.text = "2nd";
                        break;
                    }
                case 3:
                    {
                        playerPos.text = "3rd";
                        break;
                    }
            }
        else
            playerPos.text = positionInRace + "th";
    }
}
