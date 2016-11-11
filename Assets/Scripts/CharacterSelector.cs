using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

namespace Gauntlet {
	public class CharacterSelector : MonoBehaviour {

		private SelectableObjectHighlighter script;
		private Transform activeObject;
		public string characterSelected;
        MyLocalPlayer myPlayer;

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
            myPlayer = FindObjectOfType<MyLocalPlayer>();
		}

		void Update () {
			if (Input.GetKeyDown (KeyCode.Space) && activeObject != null) {
                if(myPlayer != null)
                {
                    myPlayer.SetupCharacter(activeObject.name);
                }
				characterSelected = activeObject.name;
				SceneManager.LoadScene ("Town");
			}
		}

		void SetInitialReferences () {
			script = GetComponent<SelectableObjectHighlighter> ();
		}

		void SuccessCallback (Transform objectHit) {
			
			if (ValidOption(objectHit)) {
				activeObject = objectHit;
				var canvas = activeObject.FindChild ("Canvas");
				if (canvas != null) {
					canvas.gameObject.SetActive (true);
				}
			}
		}

		bool ValidOption (Transform objectHit) {
			bool isValidObject = activeObject == null || activeObject.name != objectHit.name;

			isValidObject = isValidObject && objectHit.name != "Canvas";
			isValidObject = isValidObject && objectHit.name != "Door1" && objectHit.name != "Door2";

			return isValidObject;
		}

		void NoHitCallback (Transform lastObjectHit) {
			if (activeObject != null && lastObjectHit.name != "Canvas") {
				var canvas = activeObject.FindChild ("Canvas");
				if (canvas != null) {
					canvas.gameObject.SetActive (false);
				}
				activeObject = null;
			}
		}

		void OnSceneChanged (Scene previousScene, Scene newScene) {
			if (newScene.name == "Town") {
				Debug.Log (characterSelected);
			}
		}

	}
}