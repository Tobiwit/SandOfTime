using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHeroButton : MonoBehaviour
{
    public void AddHeroToList(BaseUnit unit) {
        HeroManager.Instance.AddHeroToList(unit);
    }
}
