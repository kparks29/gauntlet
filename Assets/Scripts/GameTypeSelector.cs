using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Gauntlet {
	public class GameTypeSelector : MonoBehaviour {

		private SelectableObjectHighlighter script;
		private string doorSelected;
		public bool isNewGame = true;

		void OnEnable () {
			SetInitialReferences ();
			script.SuccessCallbackEvent += SuccessCallback;
			script.NoHitCallbackEvent += NoHitCallback;
			SceneManager.activeSceneChanged += OnSceneChanged;
		}

		void OnDisable () {
			script.SuccessCallbackEvent -= SuccessCallback;
			script.NoHitCallbackEvent -= NoHitCallback;
			SceneManager.activeSceneChanged -= OnSceneChanged;
		}

		void Start () {

		}

		void Update () {
			if (Input.GetKeyDown (KeyCode.Space) && doorSelected != null) {
				if (doorSelected == "Door1") {
					isNewGame = true;
				} else if (doorSelected == "Door2") {
					isNewGame = false;
				}
				SceneManager.LoadScene ("CharacterSelector");
			}
		}

		void SetInitialReferences () {
			GameObject fpsController = GameObject.Find ("FPSController");
			script = fpsController.GetComponent<SelectableObjectHighlighter> ();
			DontDestroyOnLoad (this);
		}

		void SuccessCallback (Transform objectHit) {
			doorSelected = objectHit.name;
		}

		void NoHitCallback (Transform lastObjectHit) {
			doorSelected = null;
		}

		void OnSceneChanged (Scene previousScene, Scene newScene) {
			if (newScene.name == "CharacterSelector") {
				Debug.Log (isNewGame);
			}
		}
	}
}