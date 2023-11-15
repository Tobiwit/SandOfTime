using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCanvasButton : MonoBehaviour
{
    public void ChooseCanvas (int number) {
        CanvasManager.Instance.TabButton(number);
    }
}
