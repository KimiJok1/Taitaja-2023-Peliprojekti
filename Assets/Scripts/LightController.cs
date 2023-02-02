using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    // Default properties
    private float targetValue = 0.7f;
    private float startTime = 0f;

    // Time it takes to fade out
    [SerializeField] private float fadeTime = 3f;
    
    // Pre-set values for on/off
    [SerializeField] private float onValue = 0.3f;
    [SerializeField] private float offValue = 0.7f;

    // Volume and vignette fields
    [SerializeField] private Volume v;
    [SerializeField] private Vignette vg;

    // Start is called before the first frame update
    void Start()
    {
        // Try to get vignette out of volume
        v.profile.TryGet(out vg);
    }

    // Update is called once per frame
    void Update()
    {
        // Get previous value
        float prevValue = targetValue;

        // Check if key is down and change value accordingly
        targetValue = Input.GetKey("l") == true ? onValue : offValue;
        startTime = prevValue != targetValue ? Time.time : startTime;
        
        // Calculate time for smoothstep and set the intensity using it
        float t = (Time.time - startTime) / fadeTime;
        vg.intensity.value = Mathf.SmoothStep(vg.intensity.value, targetValue, t);
    }
}
