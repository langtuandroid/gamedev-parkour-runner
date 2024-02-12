using System.Collections;
using System.Collections.Generic;
using MainControllers;
using UnityEngine;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour
{
    public PlayerScriptpr player;
    public Text debugText;
    private void Start()
    {
        player = FindObjectOfType<PlayerScriptpr>();
        debugText = GetComponent<Text>();
    }
    private void Update()
    {
        debugText.text = player.playerBodypr.name +" "+ player.playerAnimatorpr.name + " " + player.playerRigidbodypr.name;
    }
}
