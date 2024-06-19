using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class SimpleSphereToggle : SphereToggle {
  
    public override void addListeners() {

        // Add a listener to the toggle
        Toggle toggle = gameObject.GetComponent<Toggle>();
        toggle.isOn = true;
        UnityEventTools.AddVoidPersistentListener(toggle.onValueChanged, setActiveSphere);
    }
}