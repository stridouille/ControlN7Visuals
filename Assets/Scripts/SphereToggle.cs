using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class SphereToggle : MonoBehaviour
{
    [SerializeField] protected GameObject sphere;
    protected Color color;

    public void InstantiateSphereToggle(GameObject sphere, Color color) {
        this.sphere = sphere;
        this.color = color;
    }
    
    public abstract void addListeners();

    public void setActiveSphere() {
        sphere.SetActive(gameObject.GetComponent<Toggle>().isOn);
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