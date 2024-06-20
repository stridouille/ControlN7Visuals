using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class SeriesSphereToggle : SphereToggle {

    public override void addListeners() {

        // Add a listener to the toggle
        Toggle toggle = gameObject.GetComponent<Toggle>();
        toggle.isOn = true;
        UnityEventTools.AddVoidPersistentListener(toggle.onValueChanged, setActiveSphere);

        // Add a listener to the play/pause button
        GameObject playPauseButton = gameObject.transform.Find("PlayPause").gameObject;
        UnityAction<GameObject> callbackPlayPause = new UnityAction<GameObject>(PlayPause);
        UnityEventTools.AddObjectPersistentListener<GameObject>(playPauseButton.GetComponentInChildren<Button>().onClick, callbackPlayPause, playPauseButton);


        // Add a listener to the forward button
        GameObject forwardButton = gameObject.transform.Find("Forward").gameObject;
        UnityEventTools.AddVoidPersistentListener(forwardButton.GetComponentInChildren<Button>().onClick, sphere.GetComponent<SphereAnimationManager>().nextTexture);
    

        // Add a listener to the backward button
        GameObject backwardButton = gameObject.transform.Find("Backward").gameObject;
        UnityEventTools.AddVoidPersistentListener(backwardButton.GetComponentInChildren<Button>().onClick, sphere.GetComponent<SphereAnimationManager>().previousTexture);
    }

    private void PlayPause(GameObject playPauseButton) {
        playPauseButton.GetComponentInChildren<SwitchImage>().ChangeImage();
        sphere.GetComponent<SphereAnimationManager>().PlayPause();
    }
}