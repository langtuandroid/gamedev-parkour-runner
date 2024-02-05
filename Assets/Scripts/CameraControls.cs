using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Vector3 offset;
    public Vector3 playerPosition;
    public PlayerScript player;
    Vector3 velocity = Vector3.zero;
    public float cameraSmoothing = 0.2f;
    private void Start()
    {
        offset = transform.position - player.transform.position;
    }
    void FixedUpdate()
    {
        
        playerPosition = player.transform.position;
        //transform.position = Vector3.Lerp(transform.position, playerPosition + offset, cameraSmoothing);
        transform.position = Vector3.SmoothDamp(transform.position, playerPosition + offset, ref velocity, cameraSmoothing);
        //tragameObject.nsform.position = new Vector3(Mathf.Lerp(transform.position.x, playerPosition.x,cameraSmoothing) + offset.x, playerPosition.y + offset.y, playerPosition.z + offset.z);
        //gameObject.GetComponent<Rigidbody>().velocity = velocity;


    }
}
