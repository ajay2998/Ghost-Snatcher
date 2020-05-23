using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class helps the ghost object to move and release skulls as obstacles for the player

public class GhostController : MonoBehaviour
{
    //The skull that the ghost will release
    public GameObject skull;

    //refrence of the skull so we can destroy it after
    GameObject refrence;

    //The player
    [SerializeField]
    Transform player;

    //Distance between the player and the ghost
    Vector3 distance;

    //rigid body of the ghost object
    Rigidbody rb;

    //The speed of the ghost which will be controlled by the unity editor
    [SerializeField]
    float speed;

    void Awake()
    {
        //getting the component of the rigid body
        rb = GetComponent<Rigidbody>();
    }


    // Start is called before the first frame update
    void Start()
    {
        //Allowing the ghost to move by a certain speed
        rb.velocity = Vector3.forward * speed;

        //calling the changepos method every 3 seconds
        InvokeRepeating("ChangePos", 3f, 3f);
        
    }

    // Update is called once per frame
    void Update()
    {
        //calculating the distance betweent the ghost and the player
        distance = player.position - transform.position;

    }

    //This method will change the position of the ghost randomly and releases skulls
    void ChangePos()
    {
        //The random position of the ghost that it will move to
        Vector3 temp = new Vector3(Random.Range(-1.01f, 1.01f), transform.position.y, transform.position.z);
        //change the position of the ghost
        transform.position = Vector3.Lerp(transform.position, temp, 0.5f);

        //checks of the distance between the player and ghost is less than -10 to release a skull
        if (distance.z < -10)
        {
            Vector3 skullPlace = new Vector3(transform.position.x, 0, transform.position.z);
            refrence = Instantiate(skull, skullPlace, Quaternion.Euler(0, 180, 0));
            Destroy(refrence, 15f); 
        }

    }

    
}
