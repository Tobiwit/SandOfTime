using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarBehaviour : MonoBehaviour
{
    public Vector3 offset;

    private float lerpTimer;
    private float health;
    private float maxHealth;
    private float chipSpeed = 10f;

    public GameObject frontHealthbar;
    public GameObject backHealthbar;


    public void SetupHealthbar(int hpMax, float barOffset, int currentHealth)
    {
        maxHealth = (float)hpMax;
        health = currentHealth;
        Vector3 currentPosition = transform.position;
        transform.position = currentPosition + new Vector3(0, barOffset, 0);
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        float fillF = frontHealthbar.transform.localScale.x;
        float fillB = backHealthbar.transform.localScale.x;
        float hFraction = health / maxHealth;
        if(fillB > hFraction)
        {
            FrontHealthbarSet(hFraction);
            backHealthbar.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            lerpTimer += Time.deltaTime;
            float percentageComplete = lerpTimer / chipSpeed;
            percentageComplete = percentageComplete * percentageComplete;
            BackHealthbarSet(Mathf.Lerp(fillB, hFraction, percentageComplete));
        }
        if (fillF < hFraction)
        {
            BackHealthbarSet(hFraction);
            backHealthbar.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentageComplete = lerpTimer / chipSpeed;
            percentageComplete = percentageComplete * percentageComplete;
            FrontHealthbarSet(Mathf.Lerp(fillF, hFraction, percentageComplete));
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }

    private void FrontHealthbarSet(float newValue)
    {
        Vector3 scale = frontHealthbar.transform.localScale;
        scale.x = newValue;
        frontHealthbar.transform.localScale = scale;
        Vector3 pos = frontHealthbar.transform.localPosition;
        pos.x = ((1-newValue)/10) * -1;
        frontHealthbar.transform.localPosition = pos;
    }

    private void BackHealthbarSet(float newValue)
    {
        Vector3 scale = backHealthbar.transform.localScale;
        scale.x = newValue;
        backHealthbar.transform.localScale = scale;
        Vector3 pos = backHealthbar.transform.localPosition;
        pos.x = ((1- newValue) / 10) * -1;
        backHealthbar.transform.localPosition = pos;
    }
}
