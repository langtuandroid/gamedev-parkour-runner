using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSettings : MonoBehaviour
{
    void Start()
    {
        GetComponent<Light>().intensity = GameManager.Instance.GetLightAmount();
    }

}
