using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;

    public AttacksDisplay starterDeck, currentDeck;
    public AttacksContainerDisplay starterDeckContainer,currentDeckContainer;

    public BaseAttack unpowered;

    private int ScarabCrystals = 5;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        starterDeckContainer.ComputeListIntoDictionary();
            foreach (KeyValuePair<BaseAttackContainer, int> pair in starterDeckContainer.Container) {
            currentDeckContainer.AddAttack(pair.Key, 1);
        }
        UnpackContainerToCurrentDeck(currentDeckContainer);
    }

    public void UnpackContainerToCurrentDeck(AttacksContainerDisplay inputContainer) {
        currentDeck.Container.Clear();
        foreach (KeyValuePair<BaseAttackContainer,int> pair in inputContainer.Container) {
            BaseAttack attackToAdd;
            if (pair.Value == 0) {
                attackToAdd = unpowered;
            } else {
                attackToAdd = pair.Key.GetBaseAttackByLevel(pair.Value);
            }
            currentDeck.AddAttack(attackToAdd);
        }
    }

    public void AddHeroStarterDecksToCurrentDeck() {
        foreach (BaseHero hero in HeroManager.Instance.selectedHeros) {
            hero.StarterDeck.Container.Clear();
            hero.StarterDeck.ComputeListIntoDictionary();
            foreach (KeyValuePair<BaseAttackContainer, int> pair in hero.StarterDeck.Container) {
                currentDeckContainer.AddAttack(pair.Key, 1);
            }
        }
        UnpackContainerToCurrentDeck(currentDeckContainer);
    }

    public AttacksDisplay GetCurrentDeck() {
        UnpackContainerToCurrentDeck(currentDeckContainer);
        return currentDeck;
    }

    private void OnApplicationQuit() {
        currentDeck.Container.Clear();
    }

    public int GetScarabCrystals() {
        return ScarabCrystals;
    }

    public void UpdateScarabCrystals(int differenceInCrystals) {
        ScarabCrystals += differenceInCrystals;
        print("New ScarabCrystal Balance: " + ScarabCrystals);
    }

    public void UpdatePairValueByIndex(int index, int differenceInValue) {
        var pair = currentDeckContainer.Container[index];
        KeyValuePair<BaseAttackContainer, int> newPair = new KeyValuePair<BaseAttackContainer, int>(pair.Key, pair.Value + differenceInValue);
        currentDeckContainer.Container.RemoveAt(index);
        currentDeckContainer.Container.Insert(index, newPair);
    }

    public KeyValuePair<BaseAttackContainer,int> GetPairByIndex(int index) {
        return currentDeckContainer.Container[index];
    }


}
