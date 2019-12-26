using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePre : MonoBehaviour
{
    [SerializeField]
    private SimpleAnimation simpleAnimation = null;

    [SerializeField]
    private Camera mainCamera = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown (0)) {
            var ray = mainCamera.ScreenPointToRay (Input.mousePosition);

            RaycastHit Hit;
            if (Physics.Raycast (ray, out Hit, Mathf.Infinity)) {
                if(simpleAnimation.IsPlaying("idle")) {
                    simpleAnimation.CrossFade("run", 0.5f);
                }
                else if(simpleAnimation.IsPlaying("run")) {
                    simpleAnimation.CrossFade("idle", 0.5f);
                }
            }        
        }
    }
}
