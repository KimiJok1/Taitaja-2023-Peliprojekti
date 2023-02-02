using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    [SerializeField] private float offValue = 0.7f;
    [SerializeField] private float onValue = 0.3f;

    [SerializeField] private Volume v;
    [SerializeField] private Vignette vg;




    // Start is called before the first frame update
    void Start()
    {
        v.profile.TryGet(out vg);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("l")){
            if(vg.intensity.value != onValue){
                
            }
        }
        else{
            vg.intensity.value = offValue;
        }
    }
}
