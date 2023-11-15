using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventTextManager : MonoBehaviour
{
    public static EventTextManager Instance;

    public Image eventTextHolder;
    public Image prefab;

    public List<Image> ActiveEventText;

    private void Awake() {
        Instance = this;

    }

    IEnumerator KillEventTextAfterTime(float time) {
        yield return new WaitForSeconds(time);

        Destroy(ActiveEventText[0].gameObject);
        ActiveEventText.RemoveAt(0);
    }

    public void NewEventText(string inputText) {
        Image eventTextObject = Instantiate(prefab, eventTextHolder.transform);
        eventTextObject.GetComponentInChildren<TextMeshProUGUI>().text = inputText;
        if (ActiveEventText.Count != 0) {
            MoveOldEventText();
        }
        ActiveEventText.Add(eventTextObject);
        StartCoroutine(KillEventTextAfterTime(5));
    }

    public void NewWarningText(string inputText) {
        NewEventText(inputText);
        ActiveEventText[ActiveEventText.Count - 1].GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
    }

    private void MoveOldEventText() {
        foreach(Image img in ActiveEventText) {
            img.transform.localPosition += new Vector3(0f, -75f, 0f);
        }
        /*if(ActiveEventText.Count >= 999) {
            //List<Image> tempList = ActiveEventText;
            //tempList.RemoveAt(0);
            Destroy(ActiveEventText[0].gameObject);
            ActiveEventText.RemoveAt(0);
            //ActiveEventText = tempList;
        }*/
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
