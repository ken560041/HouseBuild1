using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CaloriesBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public Text healthCounter;
    public GameObject playerState;

    private float currentCalories, maxCalories;
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {

        currentCalories = playerState.GetComponent<PlayerState>().currentCalories;
        maxCalories = playerState.GetComponent<PlayerState>().maxCalories;
        float fillValue = currentCalories / maxCalories;
        slider.value = fillValue;
        healthCounter.text = currentCalories + "/" + maxCalories;



    }
}
