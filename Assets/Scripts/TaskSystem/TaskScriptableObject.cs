using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Task", order = 1)]
public class TaskScriptableObject : ScriptableObject
{
    public string shortName;
    public string description;
    public string additionalInformation;

    /*public Task(string nameGiven, string desc, string add)
    {
        name = nameGiven;
        description = desc;
        additionalInformation = add;
    }*/
}
