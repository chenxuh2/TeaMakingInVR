using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is used to kinda add physics to the meg, since non-convex mesh colliders can't have physics for some reason (and therefore also no direct grasping of the mug is possible). I simulate this behaviour now, by letting the mug follow an invisible gameobject
public class FollowObject : MonoBehaviour
{

    [SerializeField] private GameObject objectToFollow;
    private Transform transformToFollow;

    // Start is called before the first frame update
    void Start()
    {
        transformToFollow = objectToFollow.transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = transformToFollow.position;
        this.transform.rotation = transformToFollow.rotation;
    }
}
