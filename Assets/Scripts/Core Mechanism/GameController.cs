using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameController _instance;
    [NonSerialized]
    public int nowScore = 0; //3顺从 -3逃离
    [NonSerialized]
    public int state = 0; //1 goal -1 exit
    [NonSerialized]
    public bool firstInLevel1 = true;

    private GameController(){}

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != _instance)
        {
            Destroy(gameObject);
        }
    }


}
