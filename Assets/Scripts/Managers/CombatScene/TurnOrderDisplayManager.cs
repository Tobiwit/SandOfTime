using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnOrderDisplayManager : MonoBehaviour
{

    public static TurnOrderDisplayManager Instance;
    public GameObject prefab;
    private int previousListLength;

    void Awake()
    {
        Instance = this;
    }

    public void SetupTurnOrderDisplay(List<BaseUnit> turnOrderList)
    {
        for (int i = 0; i < turnOrderList.Count; i++)
        {
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = turnOrderList[i].UnitName;
            if (turnOrderList[i].Faction == Faction.Hero)
            {
                obj.transform.GetChild(1).GetComponent<Image>().color = new Color(0.915f, 0.812f, 0.316f, 1.0f);
            } else
            {
                obj.transform.GetChild(1).GetComponent<Image>().color = new Color(0.915f, 0.212f, 0.316f, 1.0f);
            }
            obj.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = turnOrderList[i].GetComponent<SpriteRenderer>().sprite;

        }
        previousListLength = turnOrderList.Count;
    }

    public void UpdateTurnOrderDisplay(List<BaseUnit> turnOrderList, int cutoffNumber)
    {
        ClearTurnOrderDisplay();
        for (int i = cutoffNumber; i < turnOrderList.Count; i++)
        {
            var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i-cutoffNumber);

            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = turnOrderList[i].UnitName;
            if (turnOrderList[i].Faction == Faction.Hero)
            {
                obj.transform.GetChild(1).GetComponent<Image>().color = new Color(0.915f, 0.812f, 0.316f, 1.0f);
            }
            else
            {
                obj.transform.GetChild(1).GetComponent<Image>().color = new Color(0.915f, 0.212f, 0.316f, 1.0f);
            }
            obj.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = turnOrderList[i].GetComponent<SpriteRenderer>().sprite;

        }
        previousListLength = turnOrderList.Count;
    }

    public void ClearTurnOrderDisplay()
    {
        for (int i = 0; i < this.GetComponent<Transform>().childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3((-i * (100 + 10)) + 375, 0f,  0f);
    }
}
