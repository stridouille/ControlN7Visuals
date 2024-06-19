using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEditor.Events;

public class TogglePanelManager : MonoBehaviour
{
    [SerializeField] private GameObject togglePrefab;
    [SerializeField] private GameObject toggleSeriesPrefab;
    [SerializeField] private GameObject gridSphere;
    [SerializeField] private GameObject gridColorPicker;
    [SerializeField] private GameObject colorPicker;

    // Add a toggle for a new sphere
    public void addToggle(GameObject newSphere, Color newColor, bool isSeries) {
        
        SphereToggle toggleScript;

        // Create a new toggle
        GameObject newToggle = isSeries ? Instantiate(toggleSeriesPrefab) : Instantiate(togglePrefab);
        newToggle.transform.SetParent(gameObject.transform);
        newToggle.GetComponentInChildren<TMP_InputField>().text = newSphere.name;

        //add toggle listeners
        toggleScript = isSeries ? newToggle.AddComponent<SeriesSphereToggle>() : newToggle.AddComponent<SimpleSphereToggle>();
        toggleScript.InstantiateSphereToggle(newSphere, newColor);
        toggleScript.addListeners();

        //add color picker listener
        GameObject colorPickerButton = newToggle.transform.Find("ColorPicker").gameObject;        
        newToggle.transform.Find("ColorPicker").gameObject.GetComponent<Image>().color = newColor;
        UnityAction<GameObject> callback = new UnityAction<GameObject>(toggleScript.applyPickedColor);
        UnityEventTools.AddObjectPersistentListener<GameObject>(colorPickerButton.GetComponent<Button>().onClick, callback, colorPicker);
    }   

    public void recolorGrid() {
        Color newColor = colorPicker.GetComponent<FlexibleColorPicker>().GetColor();
        gridSphere.GetComponent<Renderer>().material.SetColor("_Color", newColor);
        gridColorPicker.GetComponent<Image>().color = newColor;
    }
}
