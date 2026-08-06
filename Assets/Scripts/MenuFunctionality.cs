using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuFunctionality : MonoBehaviour
{
    // re-assign the local level's Game Over UI the moment a new scene finishes loading.

    [Header("Persistent Settings")]
    [SerializeField] private string gameOverObjectName = "GameOverInterface"; // The exact name of your UI object in Level One
    private GameObject localGameOverInterface;

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
    public float SceneTransitionTime = 1f; // Animation float that I can adjust
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
        LogHandler.Log("Game State Reset: Ready for another run.");

        isPaused = false;
        Music.GetComponent<AudioSource>().enabled = true;
        //DontDestroyOnLoad(Music);

        
    }

    // Update is called once per frame
    void Update()
    {
        //tell me every frame if the QTE is actually there and active.
        // TEST THIS: Start the main game scene, and monitor this if else statement.
        if (QTEPanel != null)
        {
            
            LogHandler.Log("The QTE Panel is exists.");
        }
        else
        {
            LogHandler.Log("The QTE Panel doesn't exist.");
        }

        //exit update if this is happening:
        if (isGameOver) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            /*I implemented a temporary fix for Main Menu UI somewhat persisting.
             * I will sort out a proper fix later.
             */
            Debug.Log($"<color=yellow>The Main Menu is turned OFF.</color>");

            /* 1. Safety Check: If we are in the Main Menu, we don't want to pause.
             * Using buildIndex is fine, but check the .buildIndex property specifically.
             */

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                LogHandler.Log("In Main Menu: Esc disabled.");
                return; // 'return' exits the function immediately so nothing below runs.
            }

            // 2. Logic Toggle: If we aren't in the menu, toggle the pause state.
            // TODO: The main doesn't toggle after two "esc" presses.

            isPaused = !isPaused;
            
            if (isPaused)
            {
                PauseGame();                       
            }
            else
            {
                ResumeGame();
                LogHandler.Log("Game Paused");
            }
        }

    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true; // Correctly sets paused state

        // UI Management
        if (MainMenu != null) MainMenu.SetActive(false);
        if (QTEPanel != null) QTEPanel.SetActive(false); // Hide active QTE prompts while paused
        if (PauseInterface != null) PauseInterface.SetActive(true); // Show pause menu

        // Cursor Management
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("<color=yellow>[Pause]</color> Game simulation frozen.");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false; // FIX: Changed from true to false!

        // UI Management
        if (MainMenu != null) MainMenu.SetActive(false); // KEEP FALSE: You don't want the main menu panel in the middle of gameplay
        if (QTEPanel != null) QTEPanel.SetActive(true);  // Bring back QTE prompts if one was active
        if (PauseInterface != null) PauseInterface.SetActive(false); // Hide pause menu

        // Cursor Management
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("<color=green>[Resume]</color> Game simulation restored.");
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


    public void Quit()
    {
        Application.Quit();
        LogHandler.Log("The Application has come to an end.");
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
        MainMenu.SetActive(false);
        CanvasGroup canvasGroup = transition.GetComponent<CanvasGroup>();

        // 1. Block clicks so the player can't spam the "Play" button during the fade
        if (canvasGroup != null) canvasGroup.blocksRaycasts = true;

        if (transition != null)
        {
            transition.SetTrigger("Start");
            transition.Update(-1f);
        }


        // Wait for the animation to cover the screen
        yield return new WaitForSeconds(SceneTransitionTime);
        LogHandler.Log("Co-routine for animation has been triggered");

        // 3. HIDE the Main Menu UI elements while the screen is black
        if (MainMenu != null)
        {
            GameOverInterface.SetActive(false);
            QTEPanel.SetActive(false); // Keep this FALSE until the level is fully loaded
            MainMenu.SetActive(false);
        }

        // Load the actual gameplay scene
        SceneManager.LoadScene(levelIndex);

        // --- NEW/RESTORED LOGIC FOR THE NEW SCENE ---

        // 4. Tell the persistent animator to fade back to clear/Idle
        if (transition != null)
        {
            LogHandler.Log("The animation trigger has been set to 'End'");
            transition.SetTrigger("End");
            transition.Update(-1f);
        }

        // 5. Explicitly ensure your pause screen starts completely turned off in the new scene
        if (PauseInterface != null)
        {
            PauseInterface.SetActive(false);
        }

        // 6. Now that the level is loaded, safely activate the QTE panel mechanics
        if (QTEPanel != null)
        {
            // Level Loaded, QTE Panel has been activated.
            QTEPanel.SetActive(true);
            LogHandler.Log("Level Loaded, QTE Panel has been activated");
        }

        // 7. Make the UI interactive again
        if (canvasGroup != null) canvasGroup.blocksRaycasts = false;
        LogHandler.Log("UI should now be interactive again");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        Music.GetComponent<AudioSource>().enabled = false;

        LogHandler.Log("Menu Scene has now been loaded");

    }

    // This runs automatically right after Level One finishes loading
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Hunt for the Game Over screen inside the newly loaded scene
        localGameOverInterface = GameObject.Find(gameOverObjectName);

        if (localGameOverInterface != null)
        {
            Debug.Log($"<color=green>SUCCESS:</color> Found and linked {gameOverObjectName} in scene: {scene.name}");
            localGameOverInterface.SetActive(false); // Make sure it starts hidden
        }
    }

    // Call this method when the player flunks the QTE or dies
    public void TriggerGameOver()
    {
        if (localGameOverInterface != null)
        {
            localGameOverInterface.SetActive(true);
        }
        else
        {
            Debug.LogError($"Cannot display Game Over! No GameObject named '{gameOverObjectName}' found in this scene.");
        }
    }


    // tell the methods to listen .
    private void OnEnable()
    {
        GameEvents.OnVictoryAchieved += DisplayVictory;
        GameEvents.OnPlayerImpact += DisplayGameOver;
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
   

    private void OnDisable()
    {
        GameEvents.OnVictoryAchieved -= DisplayVictory;
        GameEvents.OnPlayerImpact -= DisplayGameOver;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    


}
