using UnityEngine;
using System.Collections;

namespace Gauntlet {
	public class CharacterSelector : MonoBehaviour {

		private Camera camera;
		private Ray ray;
		private RaycastHit hit;
		private Transform lastHit;


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

				if (Physics.Raycast (ray, out hit, 10f)) {
					Transform objectHit = hit.transform;
					StopAllCoroutines();
					if (lastHit == null || objectHit.name != lastHit.name && objectHit.name != "Canvas") {
						if (lastHit != null) {
							lastHit.GetComponent<MeshRenderer> ().material.shader = Shader.Find ("Legacy Shaders/Diffuse");
							lastHit.GetChild (0).gameObject.SetActive (false);
						}
						lastHit = objectHit;

					} else if (lastHit != null && objectHit.name == lastHit.name && lastHit.GetComponent<MeshRenderer> ().material.shader.name == "Legacy Shaders/Diffuse" && objectHit.name != "Canvas") {
						objectHit.GetComponent<MeshRenderer> ().material.shader = Shader.Find ("Custom/ImageEffectShader");
						objectHit.GetChild (0).gameObject.SetActive (true);
					}
				} else {
					if (lastHit != null && lastHit.GetComponent<MeshRenderer> ().material.shader.name != "Legacy Shaders/Diffuse") {
						StartCoroutine (DelayHide ());
					}
				}
			}
		}

		IEnumerator DelayHide () {
			yield return new WaitForSeconds(1);
			ray = new Ray (camera.transform.position, camera.transform.forward);
			if (!Physics.Raycast (ray, out hit, 10f)) {
				if (lastHit != null) {
					lastHit.GetComponent<MeshRenderer> ().material.shader = Shader.Find ("Legacy Shaders/Diffuse");
					lastHit.GetChild (0).gameObject.SetActive (false);
				}
			}
		}
	}
}