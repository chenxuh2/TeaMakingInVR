using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class TaskManagingSystem : MonoBehaviour
{
    //Reference to a text object for completed tasks
    [SerializeField] private GameObject completedTaskTextObject;
    private TextMeshProUGUI completedTaskText;                               

    //Reference to the text object that display the current task
    [SerializeField] private GameObject taskTextObject;
    private TextMeshProUGUI taskText;

    //Reference to the text object that shows the next task
    //[SerializeField] private GameObject nextTaskTextObject;
    //private TextMeshProUGUI nextTaskText;

    //List of all tasks
    //private List<Task> tasks = new List<Task>();
    [SerializeField] private List<TaskScriptableObject> tasks;

    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the list of tasks
        /*tasks.Add(new Task("ElectricStoveActivation", "Activate the electric stove", string.Empty));
        tasks.Add(new Task("KettlePlacing", "Place the tea kettle on top of the electric stove", string.Empty));
        tasks.Add(new Task("TeaBagPlacing", "Place the tea bag inside the mug", string.Empty));
        tasks.Add(new Task("WaitForBoiling", "Wait for the water in the tea kettle to boil", string.Empty));
        tasks.Add(new Task("Put(Boiling)Water in cup", "Put the boiling water into the mug", string.Empty));
        tasks.Add(new Task("WaitForBrewingOfTea", "Wait for the tea to finish brewing", string.Empty));
        tasks.Add(new Task("Prepare sugar", "Take a spoon and the sugar box", string.Empty));
        tasks.Add(new Task("UseSugar", "Use the spoon to put sugar into the tea", "(The spoon must be inside the tea)"));*/



        taskText = taskTextObject.GetComponent<TextMeshProUGUI>();
        completedTaskText = completedTaskTextObject.GetComponent<TextMeshProUGUI>();
        //nextTaskText = nextTaskTextObject.GetComponent<TextMeshPro>();

        showTasksByIndex(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow)) 
        {
            counter++;
            showTasksByIndex(counter);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            counter--;
            showTasksByIndex(counter);
        }
    }

    private string generateStringForTaskByIndex(int index)
    {
        string result = (index+1).ToString();
        result = result + " " + tasks[index].description;
        return result;
    }

    //Show the last task, the current task and the next task by index in the text components
    private void showTasksByIndex(int index)
    {
        //Clear every text
        clearTextComponents();

        //Only show a previous task if we are not at the first task and the index is not out of bounds
        if(index > 0 && index <= tasks.Count)
        {
            completedTaskText.text = generateStringForTaskByIndex(index - 1);
        }

        //Show the current task, but also its additional info
        if(index >= 0 && index < tasks.Count)
        {
            string newTaskText = generateStringForTaskByIndex(index);
            
            //Add the additional information if possible (i.e. if there is an addiional information)
            if (tasks[index].additionalInformation != null && tasks[index].additionalInformation != string.Empty)
            {
                newTaskText += "\n   " + tasks[index].additionalInformation;
            }

            //Add the next task (if there is one)
            if(index < tasks.Count - 1)
            {
                newTaskText += "\n" + generateStringForTaskByIndex(index + 1);
            }


            taskText.text = newTaskText;
        }

    }

    private void clearTextComponents()
    {
        completedTaskText.text = string.Empty;
        taskText.text = string.Empty;
        //nextTaskText.text = string.Empty;
    }

}
/*public class Task
{
    public string name;
    public string description;
    public string additionalInformation;

    public Task(string nameGiven, string desc, string add)
    {
        name = nameGiven;
        description = desc; 
        additionalInformation = add;
    }
}*/