using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraProvider : MonoBehaviour
{
    private void Start() {
        GridManager.Instance.SetCamera(gameObject);
    }
}
