using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageLevelLoad : MonoBehaviour
{
    [SerializeField] private bool thisSceneIsComplex, thisSceneIsFamiliarization;
    private int numberOfLevels = 2;

    //Error (canvas) variables
    [SerializeField] private GameObject errorCanvas, errorTaskTextObject;
    private TextMeshProUGUI errorText;
    [SerializeField] private List<ErrorTaskScriptable> errorTasks = new List<ErrorTaskScriptable>();
    private ErrorTaskScriptable currentError = null;

    //Do all the stuff in awake, so I can use the information for the name/path/title of the data tracking file which is created in EyeTrackingSample.cs
    private void Awake()
    {
        if(thisSceneIsFamiliarization)
        {
            return;
        }

        //Get the error text component
        errorText = errorTaskTextObject.GetComponent<TextMeshProUGUI>();
        errorText.text = string.Empty;

        //Deactivate the error canvas
        errorCanvas.SetActive(false);

        //Set the current error to null
        currentError = null;

        //Todo: maybe even don't difference between complex errors and 
        if (!thisSceneIsComplex && StaticLevelManager.getCurrentSimpleLevel() > 1)
        {
            //TODO: Start a random ERROR- task
            Debug.Log("This is a simple error level");

            //Reactivate the error canvas, if necessary
            //Get a arandom error
            string randomErrorString = getRandomErrorTask();
            errorText.text = randomErrorString;

            errorCanvas.SetActive(true);
        }
        else if (thisSceneIsComplex && StaticLevelManager.getCurrentComplexLevel() > 1)
        {
            //Todo: Start a random ERROR- task
            Debug.Log("This is a complex error level");

            //Reactivate the error canvas, if necessary
            string randomErrorString = getRandomErrorTask();
            errorText.text = randomErrorString;

            errorCanvas.SetActive(true);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Possibility to return to the main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(1); //Scene 1 is the main menu
        }


        //Only use the level management if you are not in the familiarization
        if(!thisSceneIsFamiliarization)
        {
            //Go to the level before
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!thisSceneIsComplex)
                {
                    //return to the main menu if you want to go before the 1st level
                    if (StaticLevelManager.simpleLevelCounter <= 0)
                    {
                        SceneManager.LoadScene(1); //Scene 1 is the main menu
                        StaticLevelManager.simpleLevelCounter = -1;
                        return;
                    }

                    int nextLevel = StaticLevelManager.getSimpleLevelFromBefore() % numberOfLevels;                 //Use modulo to get the error levels right

                    //Debug.Log(nextLevel);

                    SceneManager.LoadScene(nextLevel + 2);

                }
                else
                {
                    //return to the main menu if you want to go before the 1st level
                    if (StaticLevelManager.complexLevelCounter <= 0)
                    {
                        SceneManager.LoadScene(1); //Scene 1 is the main menu
                        StaticLevelManager.complexLevelCounter = -1;
                        return;
                    }

                    int nextLevel = StaticLevelManager.getComplexLevelFromBefore() % numberOfLevels;

                    SceneManager.LoadScene(nextLevel + 4);
                }
            }

            //Go to the next level
            if (Input.GetKeyDown(KeyCode.O))
            {
                if (!thisSceneIsComplex)
                {
                    //return to the main menu if you want to go after the last level
                    if (StaticLevelManager.simpleLevelCounter == StaticLevelManager.simpleLevels.Count - 1)
                    {
                        SceneManager.LoadScene(1); //Scene 1 is the main menu
                        StaticLevelManager.simpleLevelCounter = -1;
                        return;
                    }

                    int nextLevel = StaticLevelManager.getNextSimpleLevel() % numberOfLevels;
                    SceneManager.LoadScene(nextLevel + 2);
                }
                else
                {
                    //return to the main menu if you want to go after the last level
                    Debug.Log(StaticLevelManager.complexLevels.Count);
                    Debug.Log(StaticLevelManager.complexLevelCounter);
                    if (StaticLevelManager.complexLevelCounter == StaticLevelManager.complexLevels.Count - 1)
                    {
                        SceneManager.LoadScene(1); //Scene 1 is the main menu
                        StaticLevelManager.complexLevelCounter = -1;
                        return;
                    }

                    int nextLevel = StaticLevelManager.getNextComplexLevel() % numberOfLevels;
                    SceneManager.LoadScene(nextLevel + 4);
                }
            }
        }
        
    }

    private string getRandomErrorTask()
    {
        int randomIndex = Random.Range(0, errorTasks.Count);
        currentError = errorTasks[randomIndex];

        return currentError.description;
    }

    public string getCurrentErrorName ()
    {
        if(currentError == null)
        {
            return "NONE";
        }
        return currentError.name;
    }
}
