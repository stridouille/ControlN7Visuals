using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class SphereToggle : MonoBehaviour
{
    [SerializeField] protected GameObject sphere;
    public GameObject colorPicker;
    protected Color color;

    public void InstantiateSphereToggle(GameObject sphere, Color color, GameObject colorPicker) {
        this.sphere = sphere;
        this.color = color;
        this.colorPicker = colorPicker;
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

    public void applyPickedColor(GameObject colorPicker) {
        if (colorPicker == null) Debug.Log("Color picker is null");
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