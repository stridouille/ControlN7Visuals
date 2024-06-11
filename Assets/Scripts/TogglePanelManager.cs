using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TogglePanelManager : MonoBehaviour
{
    [SerializeField] private GameObject togglePrefab;

    public void addToggle(GameObject newSphere) {
        
        // Create a new toggle
        GameObject newToggle = Instantiate(togglePrefab);
        newToggle.transform.SetParent(gameObject.transform);
        newToggle.transform.SetAsFirstSibling();
        newToggle.GetComponentInChildren<TMP_InputField>().text = newSphere.name;

        // Add a listener to the toggle
        Toggle toggleComponent = newToggle.GetComponent<Toggle>();
        toggleComponent.isOn = true;
        toggleComponent.onValueChanged.AddListener((value) =>
        {
            newSphere.SetActive(value);
        });
    }   
}
