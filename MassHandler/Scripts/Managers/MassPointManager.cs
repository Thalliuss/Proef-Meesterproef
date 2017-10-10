using System;
using System.Collections.Generic;
using UnityEngine;

public class MassPointManager : MonoBehaviour
{
    public MassPointHandler massPointHandler;

    private static MassPointManager _instance;
    public static MassPointManager Instance
    {
        get
        {
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);

        _instance = this;
    }
}

