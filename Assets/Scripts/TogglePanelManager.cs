using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TogglePanelManager : MonoBehaviour
{
    [SerializeField] private GameObject togglePrefab;
    [SerializeField] private GameObject toggleSeriesPrefab;

    public void addToggle(GameObject newSphere) {
        
        // Create a new toggle
        GameObject newToggle = Instantiate(togglePrefab);
        newToggle.transform.SetParent(gameObject.transform);
        newToggle.GetComponentInChildren<TMP_InputField>().text = newSphere.name;

        // Add a listener to the toggle
        Toggle toggleComponent = newToggle.GetComponent<Toggle>();
        toggleComponent.isOn = true;
        toggleComponent.onValueChanged.AddListener((value) =>
        {
            newSphere.SetActive(value);
        });
    }   

    public void addSeriesToggle(GameObject newSphere, List<Texture2D> textures) {
        
        // Create a new toggle
        GameObject newToggle = Instantiate(toggleSeriesPrefab);
        newToggle.transform.SetParent(gameObject.transform);
        newToggle.GetComponentInChildren<TMP_InputField>().text = newSphere.name;

        // Add a listener to the toggle button
        Toggle toggleComponent = newToggle.GetComponent<Toggle>();
        toggleComponent.isOn = true;
        toggleComponent.onValueChanged.AddListener((value) =>
        {
            newSphere.SetActive(value);
        });

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
