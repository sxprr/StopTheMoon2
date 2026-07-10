using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuFunctionality : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject MainMenu;
    public GameObject PauseInterface;
    public GameObject GameOverInterface;
    public GameObject VictoryInterface; // New UI for winning!
    public GameObject QTEPanel; // New UI for winning!

    [Header("Music")]
    public SoundManager Music;

    [Header("Animator")]
    public Animator transition;

    public static MenuFunctionality Instance = null;

    [Header("Parameters")]
    public float SceneTransitionTime = 1f;
    private bool isPaused;
    public static bool isGameOver;
    
    private void Awake()
    {
        DontDestroyOnLoad(Music);

        DontDestroyOnLoad(this.gameObject);
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
        QTEPanel.SetActive(false);
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
        QTEPanel.SetActive(false);

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
        QTEPanel.SetActive(true);
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


        StartCoroutine(LoadLevel(1));

        // Check if the music object actually exists before touching it
        if (Music != null)
        {
            AudioSource source = Music.GetComponent<AudioSource>();
            if (source != null) source.enabled = true;
        }
    }

    //co routine for playing transition, then loading level
    IEnumerator LoadLevel(int levelIndex)
    {
      
        CanvasGroup canvasGroup = transition.GetComponent<CanvasGroup>();

        // 1. Block clicks so the player can't spam the "Play" button during the fade
        if (canvasGroup != null) canvasGroup.blocksRaycasts = true;

        if(transition != null)
        {
            transition.SetTrigger("Start");
        }


        // Wait for the animation to cover the screen
        yield return new WaitForSeconds(SceneTransitionTime);

        // 3. HIDE the Main Menu UI elements while the screen is black
        if (MainMenu != null)
        {
            MainMenu.SetActive(false);
        }
        
        SceneManager.LoadScene(levelIndex);

        // 2. Optional: Trigger a "Fade In" animation here if you have one
        // transition.SetTrigger("End");

        // 3. Make the UI a "ghost" again so the new scene is interactive
        if (canvasGroup != null) canvasGroup.blocksRaycasts = false;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        Music.GetComponent<AudioSource>().enabled = false;

    }

   

    // tell the methods to listen .
    private void OnEnable()
    {
        GameEvents.OnVictoryAchieved += DisplayVictory;
        GameEvents.OnPlayerImpact += DisplayGameOver;
       
    }
   

    private void OnDisable()
    {
        GameEvents.OnVictoryAchieved -= DisplayVictory;
        GameEvents.OnPlayerImpact -= DisplayGameOver;
    }
    


}
