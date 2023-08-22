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

    //Data--------------------------------------------------------------------
    //The name of the scene that should start -> Load level now is managed via the build index of the scene
    //private string levelToStart = "Sample Scene";

    //The name of the subject
    private string subjectName;

    //max number of levels and the currently selected one
    int maxLevel = 6;
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


        selectedLevel = StaticGameOptions.levelSelected;
        updateLevelLabel();
        currentSession = StaticGameOptions.sessionNr;
        sessionLabel.text = currentSession.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        //set the information/options for the game in the static class
        StaticGameOptions.subjectName = subjectName;
        StaticGameOptions.sessionNr = currentSession;
        StaticGameOptions.levelSelected = selectedLevel;

        if(subjectName != "")
        {
            if(selectedLevel == 0)
            {
                SceneManager.LoadScene(0);
            }

            if(selectedLevel >= 1)
            {
                SceneManager.LoadScene(selectedLevel + 1);
            }
            
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
        levelLabel.text = (selectedLevel + 1).ToString();
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
