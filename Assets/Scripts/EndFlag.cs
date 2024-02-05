using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class EndFlag : MonoBehaviour
{

    public Slider progressSlider;
    public float distanceBetweenPlayer;
    public float distanceBeweenPlayerAtStart;
    public PlayerScript player;
    public List<DistanceMeter> distanceFromtheEnd;
    public Transform playerParent;
    public int startPos = 0;

    private void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        distanceBeweenPlayerAtStart = Vector3.Distance(transform.position, player.transform.position);
        progressSlider = FindObjectOfType<Slider>();
        foreach (Transform playersPos in playerParent)
        {
            distanceFromtheEnd.Add(playersPos.GetComponent<DistanceMeter>());
        }
    }

    public float DistanceFinder(Transform player)
    {
        float distance = Vector3.Distance(player.position, transform.position);
        return distance;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            //Do something
            GameManager.Instance.EndGame();
            gameObject.SetActive(false);
        }
        else if (other.tag == "Enemy")
        {
            startPos++;
            other.gameObject.SetActive(false);
            //Do something
        }
    }

    private void Update()
    {
        distanceBetweenPlayer = Vector3.Distance(transform.position, player.transform.position);
        progressSlider.value = 100 -  (distanceBetweenPlayer / distanceBeweenPlayerAtStart) * 100 ;

        {
            for (int i = startPos; i < playerParent.childCount; i++)
            {
                distanceFromtheEnd[i].distance = DistanceFinder(distanceFromtheEnd[i].transform);

            }
            distanceFromtheEnd = distanceFromtheEnd.OrderBy(i => i.GetComponent<DistanceMeter>().distance).ToList();

            for (int i = startPos; i < distanceFromtheEnd.Count; i++)
            {
                distanceFromtheEnd[i].positionInRace = i + 1;
            }
        }
    }
}
