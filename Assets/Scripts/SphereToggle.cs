using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SphereToggle : MonoBehaviour
{
    [SerializeField] protected GameObject sphere;
    [SerializeField] protected GameObject colorPicker;
    [SerializeField] protected Color color;
    [SerializeField] protected GameObject exitPickerPanel;

    public void InstantiateSphereToggle(GameObject sphere, Color color, GameObject colorPicker, GameObject exitPickerPanel) {
        this.sphere = sphere;
        this.color = color;
        this.colorPicker = colorPicker;
        this.exitPickerPanel = exitPickerPanel;
    }
    
    public void setActiveSphere(bool isOn) {
        sphere.SetActive(isOn);
    }

    public void applyColor(Color color) {
        this.color = color;
        #if UNITY_EDITOR
            Renderer rend = sphere.GetComponent<Renderer>();
            Material tmpMaterial = new Material(rend.sharedMaterial);
            tmpMaterial.SetColor("_Color", color);
            rend.sharedMaterial = tmpMaterial;
        #else
            sphere.GetComponent<Renderer>().material.SetColor("_Color", color);
        #endif
        gameObject.transform.Find("ColorPicker").gameObject.GetComponent<Image>().color = color;
    }

    public void chooseColor() {
        colorPicker.SetActive(true);
        exitPickerPanel.SetActive(true);
        colorPicker.GetComponent<FlexibleColorPicker>().SetColor(color);
        Button confirmButton = GameObject.Find("ConfirmColorButton").GetComponentInChildren<Button>();
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => applyPickedColor(colorPicker));
    }

    public void applyPickedColor(GameObject colorPicker) {
        applyColor(colorPicker.GetComponent<FlexibleColorPicker>().GetColor());
    }

    public void modifyName(string newName) {
        sphere.name = newName;
        gameObject.name = "Toggle " + newName;
        gameObject.GetComponentInChildren<TMP_InputField>().text = newName;
    }

    public string getSphereName() {
        return sphere.name;
    }

    public void destroySphere() {
        DestroyImmediate(sphere);
        DestroyImmediate(gameObject);
    }
    
}