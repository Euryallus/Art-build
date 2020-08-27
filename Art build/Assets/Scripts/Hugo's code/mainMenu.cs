using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//
// ## HUGO BAILEY
// ## Written: Proof of Concept phase & edited prototype phase
// ## Purpose: Controls all methods needed to run the main menu (attached to Buttons)
//

public class mainMenu : MonoBehaviour
{
    [SerializeField]
    private Dropdown difficultyDropDown;

    [SerializeField]
    private Dropdown mode;

    [SerializeField]
    private Text highscoreText;

    [SerializeField]
    private GameObject endlessPopUp;

    private void Start()
    {
        difficultyDropDown.value = PlayerPrefs.GetInt("Difficulty", 1);
        mode.value = PlayerPrefs.GetInt("EndlessMode", 0);

        if(PlayerPrefs.GetInt("CompletedMain", 0) == 0)
        {
            endlessPopUp.SetActive(true);
        }
        else
        {
            endlessPopUp.SetActive(false);
        }
    }
    public void loadScene(string sceneToLoad) // Loads scene whos name has been passed as a parameter (e.g. "MAP" loads the map scene from Build)
    {
        //SceneManager.LoadScene(sceneToLoad);


        SaveLoadManager.instance.LoadSceneWithFade(sceneToLoad);
    }

    public void Update()
    {
        //Updates Endless mode highscore only if endless mode is selected
        if(PlayerPrefs.GetInt("EndlessMode", 0) == 1 && PlayerPrefs.GetInt("Highscore", 0) != 0)
        {
            highscoreText.text = "Endless mode highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString();
            
        }
        else
        {
            //hides highscore if endless mode is not active
            highscoreText.text = "";
        }

        if (PlayerPrefs.GetInt("CompletedMain", 0) == 0)
        {
            PlayerPrefs.SetInt("EndlessMode", 0);
            mode.value = 0;
        }
    }

    public void optionsMenu(GameObject options) // Activates or deactivates options based on whether it's been toggled on or not
    {
        options.SetActive(!options.activeSelf);
    }

    public void difficultyChange()
    {
        PlayerPrefs.SetInt("Difficulty", difficultyDropDown.value);
        //alters difficulty value (0, 1, 2) based on value selected via dropdown
    }

    public void modeChange()
    {
        //alters mode selected from dropdown value

        if(PlayerPrefs.GetInt("CompletedMain", 0) == 0)
        {
            PlayerPrefs.SetInt("EndlessMode", 0);
            mode.value = 0;
        }
        else
        {
            PlayerPrefs.SetInt("EndlessMode", mode.value);
        }
        
    }
    

    public void quitGame()
    {
        //Exits application in built application (doesn't function in Editor)
        //Debug.Log("EXIT WAS PRESSED ALERT");
        Application.Quit();
    }

    //Added by Joe:
    public void LoadIntroScene()
    {
        if(PlayerPrefs.GetInt("TutorialCompleted", 0) == 0)
        {
            SaveLoadManager.instance.LoadSceneWithFade("tutorial", true);
        }
        else
        {
            TextCutscene.storyIndex = 0;
            SaveLoadManager.instance.LoadSceneWithFade("IntroScene", false);
        }
        
    }

    public void ResetPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
