using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneButton : MonoBehaviour
{
    public void ChangeScene(string sceneName) {
        LevelManager.Instance.LoadScene(sceneName);

    }

    public void ChangeToSelectedScene() {
        LevelManager.Instance.LoadSelectedScene();
    }
}
