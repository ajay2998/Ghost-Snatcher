using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class helps the camera object to follow the character!

public class CameraFollow : MonoBehaviour
{

    //The player object
    [SerializeField]
    Transform player;

    //the distance between the player and the camera
    Vector3 distance;

    //lerp speed to make the movement smooth
    [SerializeField]
    float lerp;


    // Start is called before the first frame update
    void Start()
    {
        //calculates the distance between the player and the camera
        distance = player.position - transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //this method updates every physcis frame
    void FixedUpdate()
    {
        //The current position of the camera
        Vector3 currentPos = transform.position;

        //the new position that the camera has to go to follow the player
        Vector3 newPos = player.position - distance;

        //changing the position of the camera from the old postition to the new position by the lerp speed to make the mvement smooth!
        transform.position = Vector3.Lerp(currentPos, newPos, lerp);


    }
}
