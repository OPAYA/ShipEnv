using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_follow : MonoBehaviour
{
    public GameObject Main;
    public Transform playerBoat;
    public float cameraDistance = 30.0f;
    void Awake()
    {
        GetComponent<UnityEngine.Camera>().orthographicSize = ((Screen.height / 2) / cameraDistance);
    }
    void FixedUpdate()
    {
        transform.position = new Vector3(playerBoat.position.x, playerBoat.position.y, playerBoat.position.z - 10);
    }
}
