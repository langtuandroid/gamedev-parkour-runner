using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour
{
    public PlayerScript player;
    public Text debugText;
    private void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        debugText = GetComponent<Text>();
    }
    private void Update()
    {
        debugText.text = player.playerBody.name +" "+ player.playerAnimator.name + " " + player.playerRigidbody.name;
    }
}
