using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class SimpleSphereToggle : SphereToggle {
  
    void Start() {

        // Add a listener to the toggle
        Toggle toggle = gameObject.GetComponent<Toggle>();
        toggle.isOn = true;
        toggle.onValueChanged.AddListener((v) => setActiveSphere(v));

        // Add a listener to the color picker button
        GameObject colorPickerButton = gameObject.transform.Find("ColorPicker").gameObject;
        colorPickerButton.GetComponent<Button>().onClick.AddListener(() => applyPickedColor(colorPicker));
    }
}