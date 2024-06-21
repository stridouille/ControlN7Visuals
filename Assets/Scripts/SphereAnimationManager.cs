using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SphereAnimationManager : MonoBehaviour
{
    private bool isPlaying;
    private float previousTime;
    public List<Texture2D> textures;
    public float animationTimeDelta;
    private int currentTextureIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        isPlaying = false;
        previousTime = Time.time;
        GetComponent<Renderer>().material.mainTexture = textures[currentTextureIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            float currentTime = Time.time;
            if (currentTime - previousTime > animationTimeDelta)
            {
                previousTime = currentTime;
                // Switch to the next texture
                nextTexture();
            }
        }
    }

    public void nextTexture() {
        currentTextureIndex = (currentTextureIndex + 1) % textures.Count;
        GetComponent<Renderer>().material.mainTexture = textures[currentTextureIndex];
    }

    public void previousTexture() {
        currentTextureIndex = (currentTextureIndex - 1 + textures.Count) % textures.Count;
        GetComponent<Renderer>().material.mainTexture = textures[currentTextureIndex];
    }

    // switch play/pause state
    public void PlayPause()
    {
        if (isPlaying)
        {
            isPlaying = false;
        }
        else
        {
            isPlaying = true;
        }
    }
}
