using UnityEngine;
using System.Collections;

// Cartoon FX  - (c) 2013, Jean Moreno

// Automatically destructs an object when it has stopped emitting particles and when they have all disappeared from the screen.
// Check is performed every 0.5 seconds to not query the particle system's state every frame.
// (only deactivates the object if the OnlyDeactivate flag is set, automatically used with CFX Spawn System)

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	// If true, deactivate the object instead of destroying it
	public bool OnlyDeactivate;
    public bool Rocket = false;
    GameObject Player;

    private void Start()
    {
        //if (Rocket)
        //{
        //    Player = GameObject.Find("players");
        //    PlayerScript = Player.transform.GetChild(0).GetComponent<player>();
        //   // StartCoroutine(StartFunction());
        //}
       

    }
    void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
       
    }
   
	//IEnumerator StartFunction()
 //   {
 //       PlayerScript.StopControls = true;
 //       yield return new WaitForSeconds(1.5f);
 //       PlayerScript.StopControls = false;

 //   }
	IEnumerator CheckIfAlive ()
	{
		while(true)
		{
			yield return new WaitForSeconds(0.5f);
			if(!this.GetComponent<ParticleSystem>().IsAlive(true))
			{
				if(OnlyDeactivate)
				{
					#if UNITY_3_5
						this.gameObject.SetActiveRecursively(false);
					#else
						this.gameObject.SetActive(false);
					#endif
				}
				else
					GameObject.Destroy(this.gameObject);
				break;
			}
		}
	}
}
