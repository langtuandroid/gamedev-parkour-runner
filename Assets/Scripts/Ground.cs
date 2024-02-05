using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    PlayerScript player;
    Enemy enemy;
    private void Start()
    {
        player = FindObjectOfType<PlayerScript>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.transform.position = player.lastCheckpoint.position;
            player.ResetMovementValues();
        }
        else if (other.tag == "Enemy")
        {
            enemy = other.GetComponent<Enemy>();
            enemy.ResetMovementValues();
            enemy.transform.position = enemy.lastCheckpoint.position;
        }

    }
}