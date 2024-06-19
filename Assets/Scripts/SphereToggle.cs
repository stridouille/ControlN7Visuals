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
        sphere.GetComponent<Renderer>().material.SetColor("_Color", color);
        gameObject.transform.Find("ColorPicker").gameObject.GetComponent<Image>().color = color;
    }

    public void applyPickedColor(GameObject colorPicker) {
        applyColor(colorPicker.GetComponent<FlexibleColorPicker>().GetColor());
    }
    
}