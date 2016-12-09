using UnityEngine;
using System.Collections;
using System;

public class SelectableObjectHighlighter : MonoBehaviour 
{

	private Camera camera;
	private Ray ray;
	private RaycastHit hit;
	private Transform lastHit;

	public delegate void CallbackEvent (Transform t);
	public event CallbackEvent SuccessCallbackEvent;
	public event CallbackEvent NoHitCallbackEvent;
	public LayerMask layerMask;

    Shader outlineShader;
    Shader regularShader;

	void OnEnable () 
	{
		SetInitialReferences ();
	}

	// find the shaders
	void Start () 
	{
        outlineShader = Shader.Find("Custom/ImageEffectShader");
        regularShader = Shader.Find("Legacy Shaders/Diffuse");
    }

	void Update () 
	{
		HighlightCharacter ();
	}

	// get initial reference to camera if FPS
	void SetInitialReferences () 
	{
		if (gameObject.name == "FPSController") 
		{
			camera = GetComponentInChildren<Camera> ();
		}
	}

	void HighlightCharacter () 
	{
		// if the camera is present continue
		if (camera != null) 
		{
			// get a ray cast forward from the camera
			ray = new Ray (camera.transform.position, camera.transform.forward);

			// show hit of anything in the layerMask
			if (Physics.Raycast (ray, out hit, 20f, layerMask)) 
			{
				// set the shader to the outline shader
				var meshRenderer = hit.transform.GetComponent<MeshRenderer> ();
				if (meshRenderer != null) 
				{
                    //meshRenderer.material.shader = Shader.Find ("Custom/ImageEffectShader");      //Dont do Find's at runtime, cache a reference in the start method
                    meshRenderer.material.shader = outlineShader;
                }
				// store last hit and trigger success callback event
				lastHit = hit.transform;
				SuccessCallbackEvent (lastHit);
			}
			// if there are no hits run this
			else 
			{
				// if the last hit is not empty reset the shader back to the regular shader
				if (lastHit != null) 
				{
					var meshRenderer = lastHit.GetComponent<MeshRenderer> ();
					if (meshRenderer != null) 
					{
                        //meshRenderer.material.shader = Shader.Find ("Legacy Shaders/Diffuse");   //Dont do Find's at runtime, cache a reference in the start method
                        meshRenderer.material.shader = regularShader;
                    }
					// trigger the nothing hit callback event
					NoHitCallbackEvent (lastHit);
				}
			}
		}
	}
}