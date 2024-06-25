using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TogglePanelManager : MonoBehaviour
{
    [SerializeField] private GameObject togglePrefab;
    [SerializeField] private GameObject toggleSeriesPrefab;
    public GameObject colorPicker;


    // Add a toggle for a new sphere
    public void addToggle(GameObject newSphere, Color newColor, bool isSeries) {
        
        SphereToggle toggleScript;

        // Create a new toggle
        GameObject newToggle = isSeries ? Instantiate(toggleSeriesPrefab) : Instantiate(togglePrefab);
        newToggle.transform.SetParent(gameObject.transform);
        newToggle.GetComponentInChildren<TMP_InputField>().text = newSphere.name;
        newToggle.name = "Toggle " + newSphere.name;

        // Instantiate toggle script
        toggleScript = isSeries ? newToggle.AddComponent<SeriesSphereToggle>() : newToggle.AddComponent<SimpleSphereToggle>();
        toggleScript.InstantiateSphereToggle(newSphere, newColor, colorPicker);

        // Color button image with the plot's color
        GameObject colorPickerButton = newToggle.transform.Find("ColorPicker").gameObject;        
        colorPickerButton.GetComponent<Image>().color = newColor;
    }
}
