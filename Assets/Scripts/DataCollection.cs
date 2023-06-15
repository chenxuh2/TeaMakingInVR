using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DataCollection : MonoBehaviour
{
    private String path = "";
    public String subjectName = "Subject Name";

    string day = DateTime.Now.Day.ToString();
    string month = DateTime.Now.Month.ToString();
    string year = DateTime.Now.Year.ToString();
    string hour = DateTime.Now.Hour.ToString();
    string minutes = DateTime.Now.Minute.ToString();
    string seconds = DateTime.Now.Second.ToString();

    // Start is called before the first frame update
    void Start()
    {
        path = "C:\\Users\\vhi\\Desktop\\DataTeaCooking\\" + subjectName + "_" + day + "_" + month + "_" + year + "_" + hour + "_" + minutes + "_" + seconds + ".txt";
        InvokeRepeating("CreateText", 1.0f, 0.01f);
    }

    void CreateText()
    {
        // create file if it doesn't exist
        if (!File.Exists(path))
        {

            File.WriteAllText(path, "Unity time; " +
                                    "State; Score;\n");

        }

        //additional info?
        ////foreach (string x in allVar)
        //for (int i=0; i<allVar.Count; i+=10)
        //{

        //    // File.AppendAllText(path, x.ToString() + "\n");
        //    File.AppendAllText(path, allVar[i].ToString() + "\n");

        //}

        StreamWriter writer = new StreamWriter(path, true);

        writer.WriteLine(Time.realtimeSinceStartup.ToString() + ";"
            );
        writer.Close();

    }
}
