using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField]
    Key key;
    [SerializeField]
    bool needKey;
    [SerializeField]
    bool exit, goal;

    bool hasKey = false;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (!needKey)
            GetKey();
    }

    public void GetKey()
    {
        hasKey = true;
        animator.SetBool("hasKey", true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasKey)
        {
            if(exit)
                FindObjectOfType<SceneController>().LevelCompleteExit();
            else if(goal)
                FindObjectOfType<SceneController>().LevelCompleteGoal();
        }
    }
}
