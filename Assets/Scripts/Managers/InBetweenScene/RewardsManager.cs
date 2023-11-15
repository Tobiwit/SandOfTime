using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RewardsManager : MonoBehaviour
{
    public static RewardsManager Instance;

    public AttacksContainerDisplay inputContainer;
    public AttacksContainerDisplay currentContainer;


    public List<BaseAttackContainer> inputContainerList = new List<BaseAttackContainer>();
    public List<BaseAttackContainer> rewardsContainerList = new List<BaseAttackContainer>();


    public GameObject cardOne, cardTwo, cardThree;

    public Sprite tempSkillImage;


    private void Awake() {
        Instance = this;
        inputContainer.ComputeListIntoDictionary();
        RollThreeCards();
    }


    public void RollThreeCards() {
        inputContainerList.Clear();
        rewardsContainerList.Clear();
        cardOne.SetActive(true);
        cardTwo.SetActive(true);
        cardThree.SetActive(true);
        foreach (KeyValuePair<BaseAttackContainer, int> pair in inputContainer.Container) {
            inputContainerList.Add(pair.Key);
        }
        for (int i = 0; i < 3; i++) {
            BaseAttackContainer match = inputContainerList.OrderBy(r => Random.value).First();
            inputContainerList.Remove(match);
            rewardsContainerList.Add(match);
        }
        SetRewardCards();
    }

    public void SetRewardCards() {
        CardsManager.Instance.EditDetailCardDisplay(rewardsContainerList[0].GetBaseAttackByLevel(1), cardOne);
        CardsManager.Instance.EditDetailCardDisplay(rewardsContainerList[1].GetBaseAttackByLevel(1), cardTwo);
        CardsManager.Instance.EditDetailCardDisplay(rewardsContainerList[2].GetBaseAttackByLevel(1), cardThree);
        cardOne.transform.GetChild(8).GetChild(1).GetComponent<Image>().sprite = rewardsContainerList[0].GetBaseAttackByLevel(1).skillImage != null ? rewardsContainerList[0].GetBaseAttackByLevel(1).skillImage : tempSkillImage;
        cardTwo.transform.GetChild(8).GetChild(1).GetComponent<Image>().sprite = rewardsContainerList[1].GetBaseAttackByLevel(1).skillImage != null ? rewardsContainerList[1].GetBaseAttackByLevel(1).skillImage : tempSkillImage;
        cardThree.transform.GetChild(8).GetChild(1).GetComponent<Image>().sprite = rewardsContainerList[2].GetBaseAttackByLevel(1).skillImage != null ? rewardsContainerList[2].GetBaseAttackByLevel(1).skillImage : tempSkillImage;
    }

    public void ChooseThisCard (int optionNumber) {
        currentContainer.AddAttack(rewardsContainerList[optionNumber], 1);
        cardOne.SetActive(false);
        cardTwo.SetActive(false);
        cardThree.SetActive(false);
        CardsManager.Instance.UpdateCurrentCardDeck();
    }

}
