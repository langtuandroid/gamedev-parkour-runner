using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    PlayerScript player;
    public Transform[] correspondingPlatform;
    private void Start()
    {
        player = FindObjectOfType<PlayerScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player.lastCheckpoint = transform;
            
        }

        else if(other.tag =="Enemy" && correspondingPlatform.Length != 0)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.nearestPlatform = correspondingPlatform[Random.Range(0, correspondingPlatform.Length)];
            enemy.lastCheckpoint = transform;
        }
    }
}
