using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class SeriesSphereToggle : SphereToggle {

    void Start() {

        // Add a listener to the toggle
        Toggle toggle = gameObject.GetComponent<Toggle>();
        toggle.isOn = true;
        toggle.onValueChanged.AddListener((v) => setActiveSphere(v));

        // Add a listener to the play/pause button
        GameObject playPauseButton = gameObject.transform.Find("PlayPause").gameObject;
        playPauseButton.GetComponentInChildren<Button>().onClick.AddListener(() => PlayPause(playPauseButton));


        // Add a listener to the forward button
        GameObject forwardButton = gameObject.transform.Find("Forward").gameObject;
        forwardButton.GetComponentInChildren<Button>().onClick.AddListener(sphere.GetComponent<SphereAnimationManager>().nextTexture);
    

        // Add a listener to the backward button
        GameObject backwardButton = gameObject.transform.Find("Backward").gameObject;
        backwardButton.GetComponentInChildren<Button>().onClick.AddListener(sphere.GetComponent<SphereAnimationManager>().previousTexture);

        // Add a listener to the color picker button
        GameObject colorPickerButton = gameObject.transform.Find("ColorPicker").gameObject;
        colorPickerButton.GetComponent<Button>().onClick.AddListener(() => applyPickedColor(colorPicker));
    }

    private void PlayPause(GameObject playPauseButton) {
        playPauseButton.GetComponentInChildren<SwitchImage>().ChangeImage();
        sphere.GetComponent<SphereAnimationManager>().PlayPause();
    }
}