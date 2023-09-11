using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//This script is given to the MainMenu GameObject and manages the behaviour of the Buttons inside the MainMenu
public class MainMenu : MonoBehaviour
{
    //References-----------------------------------------------------------------
    //Bool whether the options are activated or not and teh options menu game object
    private bool optionsActivated;
    public GameObject optionsMenuObject;

    //The label of the selected level and the selected session
    public TMP_Text levelLabel, sessionLabel;

    //Name input
    TMP_InputField nameInput;

    //Number of trials
    public int numberOfTrials;

    //Data--------------------------------------------------------------------
    //The name of the scene that should start -> Load level now is managed via the build index of the scene
    //private string levelToStart = "Sample Scene";

    //The name of the subject
    private string subjectName;

    //max number of levels and the currently selected one
    int maxLevel = 2;
    private int selectedLevel;

    //selected session
    int currentSession;




    // Start is called before the first frame update
    void Start()
    {
        optionsActivated = false;
        //subjectName = "Enter name";
        nameInput = GameObject.Find("SubjectNameInput").GetComponent<TMP_InputField>();
        subjectName = StaticGameOptions.subjectName;
        nameInput.text = subjectName;
        numberOfTrials = StaticGameOptions.numberOfTrials;


        selectedLevel = StaticGameOptions.levelSelected;
        updateLevelLabel();
        currentSession = StaticGameOptions.sessionNr;
        sessionLabel.text = currentSession.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Chenge Scene ---------------------------------------------------------
    public void StartGame()
    {
        //set the information/options for the game in the static class
        StaticGameOptions.subjectName = subjectName;
        StaticGameOptions.sessionNr = currentSession;
        StaticGameOptions.levelSelected = selectedLevel;
        StaticGameOptions.numberOfTrials = numberOfTrials;

        //Create the order of the levels played
        StaticLevelManager.createSimpleLevelList(numberOfTrials);
        StaticLevelManager.createComplexLevelList(numberOfTrials);

        if (subjectName != "")
        {
            /*if(selectedLevel == 0)
            {
                SceneManager.LoadScene(0);
            }

            if(selectedLevel >= 1)
            {
                SceneManager.LoadScene(selectedLevel + 1);
            }*/
            if (selectedLevel == 0)
            {
                SceneManager.LoadScene(0);
            }
            else if (selectedLevel == 1)
            {
                SceneManager.LoadScene((StaticLevelManager.getNextSimpleLevel() % 2) + 2);
            }
            else
            {
                SceneManager.LoadScene((StaticLevelManager.getNextComplexLevel() % 2) + 4);
            }
            
        }
    }


    public void ShowQuestionnaires()
    {
        try
        {
            SceneManager.LoadScene(7);
        } catch
        {
            Debug.Log("Problem when loading Questionnaire-Scene");
        }
            
    }

    //Level selection functions --------------------------------------------
    public void selectLevelLeft()
    {
        selectedLevel--;
        if (selectedLevel < 0) selectedLevel = 0;
        updateLevelLabel();
    }

    public void selectLevelRight()
    {
        selectedLevel++;
        if (selectedLevel > maxLevel) selectedLevel = maxLevel;
        updateLevelLabel();
    }

    private void updateLevelLabel()
    {
        //levelLabel.text = (selectedLevel + 1).ToString();
        if(selectedLevel == 0)
        {
            levelLabel.text = "Familiarization";
        }
        else if(selectedLevel == 1)
        {
            levelLabel.text = "Simple";
        }
        else if (selectedLevel == 2)
        {
            levelLabel.text = "Complex";
        } else
        {
            levelLabel.text = "Default";
        }

    }

    //Session selection functions --------------------------------------------
    public void selectSessionLeft()
    {
        currentSession--;
        if (currentSession < 1) currentSession = 1;
        updateSessionLabel();
    }

    public void selectSessionRight()
    {
        currentSession++;
        updateSessionLabel();
    }

    private void updateSessionLabel()
    {
        sessionLabel.text = currentSession.ToString();
    }
    //---------------------------------------------------------------------
    //Subject name Input
    public void setSubjectName(string name)
    {                                                                                                   
        subjectName = name;   
    }


    //close the options if they are already opened and open the options if they are not already open
    public void ToggleOptionsMenu()
    {
        optionsActivated = !optionsActivated;
        optionsMenuObject.SetActive(optionsActivated);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
