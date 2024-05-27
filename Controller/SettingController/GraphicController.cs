using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicController : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown;

    // Start is called before the first frame update
    void Start()
    {
        // Get the available graphics options and add them to the dropdown
        string[] graphicsOptions = QualitySettings.names;
        graphicsDropdown.AddOptions(new List<string>(graphicsOptions));

        // Set the dropdown's value to the current graphics quality level
        graphicsDropdown.value = QualitySettings.GetQualityLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGraphicsDropdownChanged()
    {
        // Set the graphics quality based on the selected option
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }
}

