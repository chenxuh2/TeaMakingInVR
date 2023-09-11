using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ErrorTask", order = 2)]
public class ErrorTaskScriptable : ScriptableObject
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
