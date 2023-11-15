using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerFramework : MonoBehaviour
{
    public static ManagerFramework Instance;


    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

    }
}
