using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class DistanceMeterpr : MonoBehaviour
    {
        public Text playerNamepr;
        public Text playerPospr;
        public string[] playerNames;
        public float distancepr;
        public int positionInRacepr = 1;
    
        void Start()
        {
            if (tag == "Enemy")
            {
                playerNamepr.text = playerNames[Random.Range(0, playerNames.Length)];
            }
        }
    
        void Update()
        {
            if (positionInRacepr < 4)

                switch (positionInRacepr)
                {
                    case 1:
                    {
                        playerPospr.text = "1st";
                        break;
                    }
                    case 2:
                    {
                        playerPospr.text = "2nd";
                        break;
                    }
                    case 3:
                    {
                        playerPospr.text = "3rd";
                        break;
                    }
                }
            else
                playerPospr.text = positionInRacepr + "th";
        }
    }
}
