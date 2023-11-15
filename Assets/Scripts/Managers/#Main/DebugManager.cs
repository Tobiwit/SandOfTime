using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    bool showConsole;

    string input;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            OnTabPressed();
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            print("Done");
            CheckInput();
            input = "";
        }
    }


    public void OnTabPressed() {
            showConsole = !showConsole;
        }

    private void OnGUI() {
        if (showConsole) {
            float y = 0f;
            GUI.Box(new Rect(0, y, Screen.width, 50), "");
            input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 10f, 30f), input);
        }
    }

    public void CheckInput() {
        if (input.Contains("cc win")) {
            GameManager.Instance.UpdateGameState(GameState.Victory);
        }
    }

}
