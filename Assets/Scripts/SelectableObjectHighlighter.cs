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

	void OnDisable () 
	{

	}

	void Start () 
	{
        outlineShader = Shader.Find("Custom/ImageEffectShader");
        regularShader = Shader.Find("Legacy Shaders/Diffuse");
    }

	void Update () 
	{
		HighlightCharacter ();
	}

	void SetInitialReferences () 
	{
		if (gameObject.name == "FPSController") 
		{
			camera = GetComponentInChildren<Camera> ();
		}
	}

	void HighlightCharacter () 
	{
		if (camera != null) 
		{
			ray = new Ray (camera.transform.position, camera.transform.forward);

			if (Physics.Raycast (ray, out hit, 20f, layerMask)) 
			{
				var meshRenderer = hit.transform.GetComponent<MeshRenderer> ();
				if (meshRenderer != null) 
				{
                    //meshRenderer.material.shader = Shader.Find ("Custom/ImageEffectShader");      //Dont do Find's at runtime, cache a reference in the start method
                    meshRenderer.material.shader = outlineShader;
                }
				lastHit = hit.transform;
				SuccessCallbackEvent (lastHit);
			} 
			else 
			{
				if (lastHit != null) 
				{
					var meshRenderer = lastHit.GetComponent<MeshRenderer> ();
					if (meshRenderer != null) 
					{
                        //meshRenderer.material.shader = Shader.Find ("Legacy Shaders/Diffuse");   //Dont do Find's at runtime, cache a reference in the start method
                        meshRenderer.material.shader = regularShader;
                    }
					NoHitCallbackEvent (lastHit);
				}
			}
		}
	}
}