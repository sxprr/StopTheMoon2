using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctionality : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject PauseInterface;
    public GameObject GameOverInterface;
    public GameObject VictoryInterface; // New UI for winning!
    private bool isPaused;
    public SoundManager Music;

    public static MenuFunctionality Instance = null;

    public static bool isGameOver;
    private void Awake()
    {
        DontDestroyOnLoad(Music);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Reset the "Game Over" gate so the 'E' button works again
        MenuFunctionality.isGameOver = false;

        // Optional: Log it so you can see it's working in the console
        Debug.Log("Game State Reset: Ready for another run.");

        isPaused = false;
        Music.GetComponent<AudioSource>().enabled = true;
        //DontDestroyOnLoad(Music);
    }

    // Update is called once per frame
    void Update()
    {
        //exit update if this is happening:
        if (isGameOver) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 1. Safety Check: If we are in the Main Menu, we don't want to pause.
            // Using buildIndex is fine, but check the .buildIndex property specifically.
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Debug.Log("In Main Menu: Esc disabled.");
                return; // 'return' exits the function immediately so nothing below runs.
            }

            // 2. Logic Toggle: If we aren't in the menu, toggle the pause state.
            if (isPaused)
            {
                ResumeGame(); // Assuming you have a Resume function
            }
            else
            {
                PauseGame();
            }
        }

    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        MainMenu.SetActive(false);
        isPaused = true;
        PauseInterface.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void DisplayGameOver()
    {
        isGameOver = true;

        // stop time and show results
        Time.timeScale = 0;
        GameOverInterface.SetActive(true);

        // unlock the cursor for the player.

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


    }

    public void DisplayVictory()
    {
        isGameOver = true;

        // stop time and show results
        Time.timeScale = 0;
        VictoryInterface.SetActive(true);

        // unlock the cursor for the player.

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        MainMenu.SetActive(true);
        isPaused = false;
        PauseInterface.SetActive(false); ;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Application has quit");
    }

    public void LoadMainGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);

        // Check if the music object actually exists before touching it
        if (Music != null)
        {
            AudioSource source = Music.GetComponent<AudioSource>();
            if (source != null) source.enabled = true;
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        Music.GetComponent<AudioSource>().enabled = false;

    }



}
