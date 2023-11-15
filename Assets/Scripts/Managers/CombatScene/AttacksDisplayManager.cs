using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AttacksDisplayManager : MonoBehaviour
{
    public static AttacksDisplayManager Instance;
    public AttacksDisplay attacksDisplay;
    public AttacksDisplay inputInventory;
    public BaseAttack attack;

    public List<Image> attacksDisplayed;
    public Image currentSpecialAbility;

    public List<BaseAttack> discardPile;
    public List<BaseAttack> drawPile;
    public List<BaseAttack> onDisplayPile;

    private void Awake()
    {
        Instance = this;
        
    }

    

    private void OnApplicationQuit()
    {
        attacksDisplay.Container.Clear();
    }

    private void GetDeckInitialized() {
        inputInventory = DeckManager.Instance.GetCurrentDeck();
        attacksDisplay.Container.Clear();
        foreach (BaseAttack attack in inputInventory.Container) {
            attacksDisplay.AddAttack(attack);
        }
        attacksDisplay.Container = Shuffle<BaseAttack>(attacksDisplay.Container);
    }

    public void CreateDisplay()
    {
        GetDeckInitialized();
        for(int i = 0; i < attacksDisplay.Container.Count; i++)
        {
            if(i < 7)
            {
                var obj = Instantiate(attacksDisplay.Container[i].prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

                obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = attacksDisplay.Container[i].displayName;
                obj.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = attacksDisplay.Container[i].energyCost.ToString();

                if (attacksDisplay.Container[i].skillImage != null)
                {
                    obj.transform.GetChild(1).GetComponent<Image>().sprite = attacksDisplay.Container[i].skillImage;
                }

                obj.GetComponent<BaseAttackPrefab>().SetScript(attacksDisplay.Container[i]);
                obj.GetComponent<BaseAttackPrefab>().SetArrayNum(i);

                attacksDisplayed.Add(obj.GetComponent<Image>());
                onDisplayPile.Add(attacksDisplay.Container[i]);
            } else
            {
                drawPile.Add(attacksDisplay.Container[i]);
            }
        }
    }

    private void InitOnePrefab(int index, List<BaseAttack> list, int position = 0)
    {
        var obj = Instantiate(list[index].prefab, Vector3.zero, Quaternion.identity, transform);
        obj.GetComponent<RectTransform>().localPosition = GetPosition(position);

        obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = list[index].displayName;
        obj.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = list[index].energyCost.ToString();

        if (list[index].skillImage != null)
        {
            obj.transform.GetChild(1).GetComponent<Image>().sprite = list[index].skillImage;
        }

        obj.GetComponent<BaseAttackPrefab>().SetScript(list[index]);
        obj.GetComponent<BaseAttackPrefab>().SetArrayNum(position);

        attacksDisplayed.Add(obj.GetComponent<Image>());
        onDisplayPile.Add(list[index]);
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(5f, (i * (110 + 20))-325, 0f);
    }


    public void SetSpecialAttack(BaseAttack specialAttack)
    {
        var obj = Instantiate(specialAttack.prefab, Vector3.zero, Quaternion.identity, transform);
        obj.GetComponent<RectTransform>().localPosition = new Vector3(5f, -475f, 0f);

        obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = specialAttack.displayName;
        obj.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = specialAttack.energyCost.ToString();

        if (specialAttack.skillImage != null)
        {
            obj.transform.GetChild(1).GetComponent<Image>().sprite = specialAttack.skillImage;
        }

        obj.GetComponent<BaseAttackPrefab>().SetScript(specialAttack);
        obj.GetComponent<BaseAttackPrefab>().SetArrayNum(99);

        currentSpecialAbility =  obj.GetComponent<Image>();

    }

    public void RemoveSpecialAttack()
    {
        if(currentSpecialAbility == null) { return; }
        Destroy(currentSpecialAbility.gameObject);
        currentSpecialAbility = null;
    }

    public void UpdateDisplayNewCard(BaseAttack removedAttack, int removedAttackNumber)
    {
        if (removedAttackNumber == 99) {
            RemoveSpecialAttack();
            return;
        }

        if (onDisplayPile[removedAttackNumber] == removedAttack)
        {
            onDisplayPile.RemoveAt(removedAttackNumber);
            Destroy(attacksDisplayed[removedAttackNumber].gameObject);
            attacksDisplayed.RemoveAt(removedAttackNumber);
            discardPile.Add(removedAttack);
            if (drawPile.Count > 0)
            {
                InitOnePrefab(0, drawPile);
                drawPile.RemoveAt(0);
            } else
            {
                ShuffleDiscardToDraw();
                InitOnePrefab(0, drawPile);
                discardPile.Clear();
                drawPile.RemoveAt(0);
            }
            for (int j = 0; j < attacksDisplayed.Count; j++)
            {
                if (j>=removedAttackNumber)
                {
                    attacksDisplayed[attacksDisplayed.Count-1].transform.localPosition = new Vector3(5f, 585, 0f);
                    //attacksDisplayed[j].GetComponent<RectTransform>().localPosition = GetPosition(j);
                    LeanTween.moveLocal(attacksDisplayed[j].gameObject, GetPosition(j), 1f).setEase(LeanTweenType.easeOutExpo);
                    attacksDisplayed[j].GetComponent<BaseAttackPrefab>().SetArrayNum(j);
                }
            }
        }
    }

    public void RedrawAttackDisplay() {
        drawPile = Shuffle<BaseAttack>(drawPile);
        foreach (BaseAttack displayedAttack in onDisplayPile) {
            discardPile.Add(displayedAttack);
        }
        onDisplayPile.Clear();
        if (drawPile.Count < 7) {
            ShuffleDiscardToDraw();
            discardPile.Clear();
        }

        foreach (Image card in attacksDisplayed) {
            Destroy(card.gameObject);
        }
        attacksDisplayed.Clear();
        for (int i = 0; i < 7; i++) {
            InitOnePrefab(0, drawPile, i);
            drawPile.RemoveAt(0);
        }
    }

    private void ShuffleDiscardToDraw()
    {
        List<BaseAttack> temp = Shuffle<BaseAttack>(discardPile);
        foreach(BaseAttack newDrawAttack in temp)
        {
            drawPile.Add(newDrawAttack);
        }
    }

    public List<T> Shuffle<T>(List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            T temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }
        return _list;
    }


    public void DeactivateAllAttacks()
    {
        GuiManager.Instance.ShowAttackInfoDisplay(false);
        foreach (var attack in attacksDisplayed)
        {
            attack.gameObject.GetComponent<BaseAttackPrefab>().SetAttackActivity(false);
        }
        if (currentSpecialAbility != null)
        {
            currentSpecialAbility.gameObject.GetComponent<BaseAttackPrefab>().SetAttackActivity(false);
        }
    }

    public void DeactivateAllAttacksExcept(BaseAttack exceptionBaseAttack, int number)
    {
        Image exceptionAttack;
        if (number != 99) {
            exceptionAttack = attacksDisplayed[number];
        } else {
            exceptionAttack = currentSpecialAbility;
        }

        foreach (var attack in attacksDisplayed)
        {
            if (attack != exceptionAttack)
            {
                attack.gameObject.GetComponent<BaseAttackPrefab>().SetAttackActivity(false);
            }
        }
        if (currentSpecialAbility != null && currentSpecialAbility != exceptionAttack)
        {
            currentSpecialAbility.gameObject.GetComponent<BaseAttackPrefab>().SetAttackActivity(false);
        }
    }

}
