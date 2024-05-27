using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider health;
    PlayerObjectController instance = PlayerObjectController.getInstance();

    // Start is called before the first frame update
    void Start()
    {
        health.value = instance.getCurrHealth();
    }

    // Update is called once per frame
    void Update()
    {
        health.value = instance.getCurrHealth();
    }
}
