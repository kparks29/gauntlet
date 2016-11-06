using UnityEngine;
using System.Collections;
using System;

namespace Gauntlet {
	public class CharacterSelector : MonoBehaviour {

		private SelectableObjectHighlighter script;

		void OnEnable () {
			SetInitialReferences ();
			script.SuccessCallbackEvent += SuccessCallback;
			script.NoHitCallbackEvent += NoHitCallback;
		}

		void OnDisable () {
			script.SuccessCallbackEvent -= SuccessCallback;
			script.NoHitCallbackEvent -= NoHitCallback;
		}

		void Start () {

		}

		void Update () {


		}

		void SetInitialReferences () {
			script = GetComponent<SelectableObjectHighlighter> ();
		}

		void SuccessCallback (Transform objectHit) {
			
		}

		void NoHitCallback (Transform lastObjectHit) {
			
		}

	}
}