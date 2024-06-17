using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TogglePanelManager : MonoBehaviour
{
    [SerializeField] private GameObject togglePrefab;
    [SerializeField] private GameObject toggleSeriesPrefab;
    [SerializeField] private GameObject gridSphere;
    [SerializeField] private GameObject gridColorPicker;
    [SerializeField] private GameObject colorPicker;

    // Add a toggle for a single plot
    public void addToggle(GameObject newSphere) {
        
        // Create a new toggle
        GameObject newToggle = Instantiate(togglePrefab);
        newToggle.transform.SetParent(gameObject.transform);
        newToggle.GetComponentInChildren<TMP_InputField>().text = newSphere.name;

        addListeners(newSphere, newToggle, false);
    }   

    // Add a toggle for a plot series
    public void addToggleSeries(GameObject newSphere, List<Texture2D> textures) {
        
        // Create a new toggle
        GameObject newToggle = Instantiate(toggleSeriesPrefab);
        newToggle.transform.SetParent(gameObject.transform);
        newToggle.GetComponentInChildren<TMP_InputField>().text = newSphere.name;
        addListeners(newSphere, newToggle, true);
    }

    public void addListeners(GameObject newSphere, GameObject newToggle, bool isSeries) {
        // Add a listener to the toggle
        Toggle toggleComponent = newToggle.GetComponent<Toggle>();
        toggleComponent.isOn = true;
        toggleComponent.onValueChanged.AddListener((value) =>
        {
            newSphere.SetActive(value);
        });

        // Add a listener to the delete button
        GameObject deleteButton = newToggle.transform.Find("Delete").gameObject;
        deleteButton.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            Destroy(newSphere);
            Destroy(newToggle);
        });

        // Add a listener to the color picker button
        GameObject colorPickerButton = newToggle.transform.Find("ColorPicker").gameObject;
        // Set the picker button's color
        colorPickerButton.GetComponent<Image>().color = newSphere.GetComponent<Renderer>().material.GetColor("_Color");
        colorPickerButton.GetComponentInChildren<Button>().onClick.AddListener(() => {
            applyPickedColor(newSphere, colorPickerButton);
        });

        // Add other listeners in case plot series
        if (isSeries) {
            // Add a listener to the play/pause button
            GameObject playPauseButton = newToggle.transform.Find("PlayPause").gameObject;
            playPauseButton.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                playPauseButton.GetComponentInChildren<SwitchImage>().ChangeImage();
                newSphere.GetComponent<SphereAnimationManager>().PlayPause();
            });

            // Add a listener to the forward button
            GameObject forwardButton = newToggle.transform.Find("Forward").gameObject;
            forwardButton.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                newSphere.GetComponent<SphereAnimationManager>().nextTexture();
            });

            // Add a listener to the backward button
            GameObject backwardButton = newToggle.transform.Find("Backward").gameObject;
            backwardButton.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                newSphere.GetComponent<SphereAnimationManager>().previousTexture();
            }); 
        }
    }

    private void applyPickedColor(GameObject sphere, GameObject colorPickerButton) {
        Color newColor = colorPicker.GetComponent<FlexibleColorPicker>().GetColor();
        sphere.GetComponent<Renderer>().material.SetColor("_Color", newColor);
        colorPickerButton.GetComponent<Image>().color = newColor;
    }

    public void recolorGrid() {
        Color newColor = colorPicker.GetComponent<FlexibleColorPicker>().GetColor();
        gridSphere.GetComponent<Renderer>().material.SetColor("_Color", newColor);
        gridColorPicker.GetComponent<Image>().color = newColor;
    }
}
