using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;   
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


//The most important script which controlls the player and all important aspects of the game.

public class PlayerController : MonoBehaviour
{
    //The instance of the player so it can be called from other classes
    public static PlayerController instance;
    
    //The rigid body of the player
    public Rigidbody rb;

    //Animator that will add animation to the player
    public Animator anim;

    //The speed of the player
    float speed;

    //The jump force of the player
    [SerializeField]
    float jumpForce;

    //bool to check if the game is over or not
    public bool gameOver = false;

    //bool to check if the player on the ground or not
    bool onGround;

    //bool to check if the player is jumping
    bool isJumping = false;

    //bool to check if the player is falling
    bool falling = false; 

    //Vector that will store the movement of the player
    Vector3 move;

    //The number of hits the player hits and obstacles will be stored here
    int hitTimes = 0;

    //A powerup object "health kit" will be refrenced by this gameobject
    public GameObject healthKit;

    //check if an instance of the scirpt is null or not
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        //getting the component of the rigid body
        rb = GetComponent<Rigidbody>();
        //getting the combonent of the animator 
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //once the game starts the score counter starts
        ScoreManager.instance.StartScore();
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the game is over
        if(gameOver)
        {
            //stopping the score
            ScoreManager.instance.StopScore();
            //invoking the game over method
            Invoke("GameOver", 0.5f);
        }

        //checking when the player taps on the screen to jump.
        //checks if the player is on the ground to be able to jump again
        //checking if the game is over or not to not allow the player to jump
        if (Input.GetMouseButtonDown(0) && onGround == true && gameOver == false)
        {
            //playing the jumping sound from sound manager class
            SoundManager.PlaySound("jumping");
            //changing the animation to the jump animation
            anim.SetTrigger("jump");
            //adding an up force to allow the character  to jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //making on ground equal to false as the player is not on ground
            onGround = false;
            //making the is jumping variable equal to true as he is jumping
            isJumping = true;
        }
    }


    //updates every physcis frame
    void FixedUpdate()
    {
        //checking if the game is over or not to not allow movement
        if(!gameOver)
        {
            //allowing the x axis of the player to be changed by the accerlomater
            move.x = (Input.acceleration.x * 5);
            //y is equal to 0
            move.y = 0;
            //z is equal to the speed variable.
            move.z = speed;
            //changing the velocity of the player by the move variable
            rb.velocity = move;
            //checks if the number of platforms is less than 20 to give a certain speed to the player
            if (GroundManager.instance.numberOfPlatforms < 20)
            {
                speed = 8;
                move.z = speed;
                rb.velocity = move;
            }
            //checks if the number of platform is between 20 and 40 to change the player velocity to 9
            else if (GroundManager.instance.numberOfPlatforms >= 20 && GroundManager.instance.numberOfPlatforms < 40)
            {
                speed = 9f;
                move.z = speed;
                rb.velocity = move;
            }
            //if number of platforms is more than or equal to 40 the velocity will change to 10 to make the game harder!
            else if (GroundManager.instance.numberOfPlatforms >= 40)
            {
                speed = 10f;
                move.z = speed;
                rb.velocity = move;
            }
        }

        //checks if the game over is true and the player is falling to change the y axis velocity to -9.81
        else if(gameOver && falling)
        {
            move.z = 0;
            move.x = 0;
            //changing the y velocity
            move.y = -9.81f;
            rb.velocity = move;
            //changing animation to fall animation
            anim.SetTrigger("fall");
            //making the game over = true
            gameOver = true;
            //plays the falling sound
            SoundManager.PlaySound("falling");
            //stops the player from falling froever so we disable the gravity till the user clicks on the restart button
            if (transform.position.y < (-30))
            {
                this.rb.useGravity = false;
            }
        }
        //for any other case the player velocity will be 0
        else
        {
            rb.velocity = Vector3.zero;
        }
        
    }

    //checks if the player had a collision with any object
    void OnCollisionEnter(Collision cl)
    {
        //if the player hits the ground we make the on ground variable true to allow jumping
        if (cl.gameObject.CompareTag("ground") && onGround == false)
        {
            onGround = true;
        }
        //if the player hits the ghost the ghost will die and we get a power up that will increase the health of the player!
        if (cl.gameObject.tag == "ghost")
        {
            //vector for the powerup's position
            Vector3 healthPos = new Vector3(0, 1.5f, cl.transform.position.z + 30);
            //destroying the ghost
            Destroy(cl.gameObject);
            //instantiating the health kit
            Instantiate(healthKit, healthPos, Quaternion.Euler(-90.0f,0,0));
        }
        //if the player hits the "Fall cube" mentioned in the ground manager class, the player has to fall
        //the fall cube is an invisible cube that is between grounds with gaps to check if the player was late to jump and therfore the player loses and fall down
        if (cl.gameObject.CompareTag("fall"))
        {
            //makes the falling equal to true
            falling = true;
            //makes the game over equal to true
            gameOver = true;
            //destroying the fallcube
            Destroy(cl.gameObject);
        }

        //if the player hits an obstacle 
        if (cl.gameObject.CompareTag("obstacle"))
        {
            //changing the number of platforms so the speed of the player decreseas
            GroundManager.instance.numberOfPlatforms = 0;
            //increasing the hit times by 1
            hitTimes++;
            //if the player hits 3 times then he dies!
            if(hitTimes == 3)
            {
                //changing animation to death animation
                anim.SetTrigger("die");
                //making game over equal to true
                gameOver = true;
                //playing the death sound!
                SoundManager.PlaySound("die");
            }
            //playing the hit sound
            SoundManager.PlaySound("hit");
            //destroying the obstacle
            Destroy(cl.gameObject);
        }
        //if the player hits the fence this code will run
        if (cl.gameObject.CompareTag("fence"))
        {
            //changing the number of platforms so the speed of the player decreseas
            GroundManager.instance.numberOfPlatforms = 0;
            //increasing the hit times by 1
            hitTimes++;
            //changing the position of the player back to the middle of the ground
            transform.position = new Vector3(0,transform.position.y,transform.position.z);
            //if the player hits 3 times then he dies!
            if (hitTimes == 3)
            {
                //changing animation to death animation
                anim.SetTrigger("die");
                //making game over equal to true
                gameOver = true;
                //playing the death sound!
                SoundManager.PlaySound("die");
            }
            //playing the hit sound
            SoundManager.PlaySound("hit");
        }
        //if the player hits a skull this code will run
        if (cl.gameObject.CompareTag("skull"))
        {
            //changing the number of platforms so the speed of the player decreseas
            GroundManager.instance.numberOfPlatforms = 0;
            //increasing the hit times by 1
            hitTimes++;
            //if the player hits 3 times then he dies!
            if (hitTimes == 3)
            {
                //changing animation to death animation
                anim.SetTrigger("die");
                //making game over equal to true
                gameOver = true;
                //playing the death sound!
                SoundManager.PlaySound("die");
            }
            //playing the hit sound
            SoundManager.PlaySound("hit");
            //destroying the obstacle
            Destroy(cl.gameObject);
        }
        //if the player hits the health kit this code will run
        if(cl.gameObject.CompareTag("health"))
        {
            //decrease the hit times by one
            hitTimes--;
            //destroying the health kit
            Destroy(cl.gameObject);
        }
    }
    //a function that will run when the game is over
    public void GameOver()
    {
        //set the game over panel to true 
        UIManager.instance.gameOverPanel.SetActive(true);
        //getting the final score
        UIManager.instance.finalScore.text = "Your Final score is: " + PlayerPrefs.GetInt("score");
    }

}