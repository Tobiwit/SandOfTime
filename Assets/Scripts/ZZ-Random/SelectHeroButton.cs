using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectHeroButton : MonoBehaviour
{

    [SerializeField]
    private Sprite normal_sprite;
    [SerializeField]
    private Sprite pressed_sprite;

    public bool On = false;
    public void AddHeroToList(BaseUnit unit) {
        if(On) {
            HeroManager.Instance.RemoveHeroFromList(unit);
        } else {
            HeroManager.Instance.AddHeroToList(unit);
        }
    }

    public void Switch() {
        Button m_button;
        m_button = GetComponent<Button>();
        if(On){
            On = false;
            Debug.Log("Turned Off");
            m_button.image.sprite = normal_sprite;
            m_button.interactable = false;
            m_button.interactable = true;
        } else {
            if (HeroManager.Instance.selectedHeros.Count < 4) {
                On = true;
                Debug.Log("Turned On");
                m_button.image.sprite = pressed_sprite;
                m_button.interactable = false;
                m_button.interactable = true;
            } else {
                Debug.Log("Too Many");
            }
        }
    }
}
