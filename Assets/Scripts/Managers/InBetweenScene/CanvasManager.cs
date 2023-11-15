using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;
    [SerializeField] public GameObject mainCanvas, rewardCanvas, cardsCanvas, heroesCanvas, mapCanvas;

    private void Awake() {
        Instance = this;
    }


    public void TabButton(int canvasNumber) {
        rewardCanvas.SetActive(false);
        cardsCanvas.SetActive(false);
        heroesCanvas.SetActive(false);
        mapCanvas.SetActive(false);
        MapManager.Instance.transform.GetChild(0).GetComponent<Canvas>().gameObject.SetActive(false);
        switch (canvasNumber) {
            case 1:
                rewardCanvas.SetActive(true);
                mainCanvas.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "Rewards";
                break;
            case 2:
                cardsCanvas.SetActive(true);
                mainCanvas.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "Card Deck";
                break;
            case 3:
                heroesCanvas.SetActive(true);
                mainCanvas.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "Heroes";
                break;
            case 4:
                mapCanvas.SetActive(true);
                MapManager.Instance.transform.GetChild(0).GetComponent<Canvas>().gameObject.SetActive(true);
                mainCanvas.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "Phyramid Map";
                break;
        }
    }
}
