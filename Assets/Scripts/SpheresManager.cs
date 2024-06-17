using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpheresManager : MonoBehaviour
{
    [SerializeField] private Shader _shader;
    private int nextColorNum = 1;


    public GameObject addSphere(Texture2D texture, string nom)
    {
        GameObject newSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        newSphere.name = nom + " " + nextColorNum;
        Renderer rend = newSphere.GetComponent<Renderer>();
        rend.material = new Material(_shader);
        rend.material.mainTexture = texture;
        rend.material.SetColor("_Color", Color.HSVToRGB(getColorRate(nextColorNum), 1, 1));
        nextColorNum++;
        newSphere.transform.parent = transform;
        return newSphere;
    }

    public void changeBackgroundAlpha(float alpha)
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<Renderer>().material.SetFloat("_decreaseAlpha", alpha);
        }
    }

    private float getColorRate(int num) {
        float phi = (1 + Mathf.Sqrt(5))/2;
        return num * phi - Mathf.Floor(num * phi);
    }
}
