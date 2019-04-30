using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Services : MonoBehaviour {

    public static Services Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
