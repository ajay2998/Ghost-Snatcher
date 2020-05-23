using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//This class takes care of the UI of the game!

public class UIManager : MonoBehaviour
{
    bool isMute;

    //instance of the UImanager
    public static UIManager instance;

    //Buttons and their sprites
    public Button pauseButton;
    public Button muteButton;
    Sprite pause;
    Sprite resume;
    Sprite sound;
    Sprite noSound;

    //The score and final score texts
    public Text score;
    public Text finalScore;

    //The game over panel
    public GameObject gameOverPanel;
    


    //make sure that the instance is not null so it can be accessed by other scripts
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //loading the button images from the resources folder from UNITY
        pause = Resources.Load<Sprite>("PauseButton");
        resume = Resources.Load<Sprite>("PlayButton");
        sound = Resources.Load<Sprite>("Sound");
        noSound = Resources.Load<Sprite>("NoSound");
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Changes the test of the score text to the current score from the UIManager script
        score.text = "Score: " + PlayerPrefs.GetInt ("score");
    }

    //This method controlls the pausing of the game
    public void PauseGame()
    {
        //checks if the game is paused or not and changes the image of the button accordingly.
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            pauseButton.image.sprite = resume;
            Debug.Log("Game is Paused");
        }
        else
        {
            Time.timeScale = 1;
            pauseButton.image.sprite = pause;
            Debug.Log("Game is resumed");
        }
    }

    //This method controlls the sound of the game
    public void Mute()
    {
        //checks if the game is muted or not and changes the image of the button accordingly.
        if (AudioListener.volume == 0)
        {
            AudioListener.volume = 1;
            muteButton.image.sprite = sound;
            isMute = false;
            Debug.Log("Is the game muted?" + isMute);
        }
        else
        {
            AudioListener.volume = 0;
            muteButton.image.sprite = noSound;
            isMute = true;
            Debug.Log("Is the game muted?" + isMute);
        }
    }

    //Restarts the game if the player died!
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
