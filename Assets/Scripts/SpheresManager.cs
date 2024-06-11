using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpheresManager : MonoBehaviour
{
    [SerializeField] private Shader _shader;

    public GameObject addSphere(Texture2D texture, string url)
    {
        GameObject newSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Renderer rend = newSphere.GetComponent<Renderer>();
        rend.material = new Material(_shader);
        rend.material.mainTexture = texture;
        newSphere.transform.parent = transform;
        newSphere.name = System.IO.Path.GetFileNameWithoutExtension(url);
        return newSphere;
    }

    public void changeBackgroundAlpha(float alpha)
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<Renderer>().material.SetFloat("_decreaseAlpha", alpha);
        }
    }
}
