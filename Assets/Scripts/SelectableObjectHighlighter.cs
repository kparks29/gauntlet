using UnityEngine;
using System.Collections;
using System;

namespace Gauntlet {
	public class SelectableObjectHighlighter : MonoBehaviour {

		private Camera camera;
		private Ray ray;
		private RaycastHit hit;
		private Transform lastHit;
		public delegate void CallbackEvent (Transform t);
		public event CallbackEvent SuccessCallbackEvent;
		public event CallbackEvent NoHitCallbackEvent;
		public LayerMask layerMask;

		void OnEnable () {
			SetInitialReferences ();
		}

		void OnDisable () {

		}

		void Start () {

		}

		void Update () {

			HighlightCharacter ();

		}

		void SetInitialReferences () {
			if (gameObject.name == "FPSController") {
				camera = GetComponentInChildren<Camera> ();
			}
		}

		void HighlightCharacter () {
			if (camera != null) {
				ray = new Ray (camera.transform.position, camera.transform.forward);

				if (Physics.Raycast (ray, out hit, 20f, layerMask)) {
					hit.transform.GetComponent<MeshRenderer> ().material.shader = Shader.Find ("Custom/ImageEffectShader");
					lastHit = hit.transform;
					SuccessCallbackEvent (lastHit);
				} else {
					if (lastHit != null) {
						lastHit.GetComponent<MeshRenderer> ().material.shader = Shader.Find ("Legacy Shaders/Diffuse");
						NoHitCallbackEvent (lastHit);
					}
				}
			}
		}
	}
}