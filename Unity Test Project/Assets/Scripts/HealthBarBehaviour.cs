using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    public Slider slider;
    public Color Low;
    public Color High;
    public Vector2 Offset;

    private RectTransform sliderTransform;

    private void Start()
    {
        sliderTransform = slider.GetComponent<RectTransform>();
    }

    public void SetHealth(float health, float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = health;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, slider.normalizedValue);
        slider.gameObject.SetActive(health < maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.parent.transform.position);
        sliderTransform.anchoredPosition = screenPoint + Offset;

        //Unused code below
        //slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
        //Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.parent.transform.position) + Offset; //handle offset here; (needs to be negative now iirc)
        //sliderTransform.anchoredPosition = transform.parent.transform.position + Offset;
    }
}
