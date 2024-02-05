using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeWall : MonoBehaviour
{
    PlayerScript player;
    private void Start()
    {
        player = FindObjectOfType<PlayerScript>();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
                StartCoroutine(player.Jump());
        }
        else if(other.tag == "Enemy")
        {
            StartCoroutine(other.GetComponent<Enemy>().Jump());
        }
    }

}
