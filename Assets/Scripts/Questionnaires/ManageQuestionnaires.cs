using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

//Activates exactly one questionnaire gameobject at the time, depending on a value that can be selected with the arrow key left and right
public class ManageQuestionnaires : MonoBehaviour
{
    [SerializeField] private List<GameObject> QuestionnaireObjects;
    private int currentQuestionnaire = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        //Activate the first questionnaire in the list (if there is at least one)
        if(QuestionnaireObjects.Count > 0)
        {
            activateQuestionnaireByIndex(currentQuestionnaire);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentQuestionnaire++;
            currentQuestionnaire = Mathf.Clamp(currentQuestionnaire, 0, QuestionnaireObjects.Count - 1);        //It would be sufficient to only check that the upper boundary is kept, i.e. currentQuestionaire < QuestionnaireObjects.Count
            activateQuestionnaireByIndex(currentQuestionnaire);
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentQuestionnaire--;
            currentQuestionnaire = Mathf.Clamp(currentQuestionnaire, 0, QuestionnaireObjects.Count - 1);        //It would be sufficient to only check that the lower boundary is kept, i.e. currentQuestionaire >= 0
            activateQuestionnaireByIndex(currentQuestionnaire);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    //Activate the current questionnaire and deactivate the others (should be sufficient to just deactivate the two questionnaires next to the indexed one, since the current one could only have been activated after one of them was active, and therefore the for-loop can be avoided, but performance really isn't an issue here)
    private void activateQuestionnaireByIndex(int index)
    {
        for(int i = 0; i < QuestionnaireObjects.Count; i++)
        {
            GameObject obj = QuestionnaireObjects[i];
            
            if(i != index) obj.SetActive(false);
            else obj.SetActive(true);
        }
    }
}
