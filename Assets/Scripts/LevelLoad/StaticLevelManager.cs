using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

public static class StaticLevelManager
{
    //Declare the variables and set default values
    public static string subjectName = "Enter name";
    public static int simpleLevelCounter = -1;
    public static int complexLevelCounter = -1;


    //Lists of the Levels
    public static float minSpeed = -2f;

    public static List<int> simpleLevels = new List<int>() {1, 2};
    public static List<int> complexLevels = new List<int>() { 1, 2 };

    public static void createSimpleLevelList(int numberOfTrialsPerLevel)
    {
        simpleLevels = new List<int>();
        for(int level = 0; level < 2; level++)
        {
            for (int i = 0; i < numberOfTrialsPerLevel - 1; i++)
            {
                simpleLevels.Add(level);
            }
        }

        //Encode "error" levels as 2 for the first complex task and 3 for the second complex task
        simpleLevels.Add(2);
        simpleLevels.Add(3);

        simpleLevels.shuffleList();
        /*foreach (var x in simpleLevels)
        {
            Debug.Log(x.ToString());
        }
        Debug.Log("----");*/
    }

    public static void createComplexLevelList(int numberOfTrialsPerLevel)
    {
        complexLevels = new List<int>();
        for (int level = 0; level < 2; level++)
        {
            for (int i = 0; i < numberOfTrialsPerLevel - 1; i++)
            {
                complexLevels.Add(level);
            }
        }

        //Encode "error" levels as 2 for the first complex task and 3 for the second complex task
        complexLevels.Add(2);
        complexLevels.Add(3);


        complexLevels.shuffleList();
        /*foreach (var x in complexLevels)
        {
            Debug.Log(x.ToString());
        }*/
    }


    //Functions to get the current/next/last level-----------------------------------------------------------------------
    //Simple levels
    public static int getSimpleLevelFromBefore()
    {
        simpleLevelCounter--;
        simpleLevelCounter = Mathf.Clamp(simpleLevelCounter, 0, simpleLevels.Count - 1);

        return simpleLevels[simpleLevelCounter];
    }

    public static int getCurrentSimpleLevel()
    {
        int tempIndex = Mathf.Clamp(simpleLevelCounter, 0, simpleLevels.Count-1);
        return simpleLevels[tempIndex];
    }

    public static int getNextSimpleLevel()
    {
        simpleLevelCounter++;
        simpleLevelCounter = Mathf.Clamp(simpleLevelCounter, 0, simpleLevels.Count - 1);

        return simpleLevels[simpleLevelCounter];
    }

    //Complex levels
    public static int getComplexLevelFromBefore()
    {
        complexLevelCounter--;
        complexLevelCounter = Mathf.Clamp(complexLevelCounter, 0, complexLevels.Count - 1);

        return complexLevels[complexLevelCounter];
    }

    public static int getCurrentComplexLevel()
    {
        return complexLevels[complexLevelCounter];
    }

    public static int getNextComplexLevel()
    {
        complexLevelCounter++;
        complexLevelCounter = Mathf.Clamp(complexLevelCounter, 0, complexLevels.Count - 1);

        return complexLevels[complexLevelCounter];
    }

    //This function randomly shuffles a list
    public static void shuffleList<T>(this IList<T> values)
    {
        System.Random rand = new System.Random();

        for (int i = values.Count - 1; i > 0; i--)
        {
            int k = rand.Next(i + 1);
            T value = values[k];
            values[k] = values[i];
            values[i] = value;
        }
    }
}





