using UnityEngine;
using System.Collections;

namespace Gauntlet {
	public class FPSController : MonoBehaviour {

		private float maxX = 360;
		private float minX = -360;
		private float maxY = 60;
		private float minY = -60;
		private float rotationY = 0F;
		private float rotationX;

		void OnEnable () {
		
		}

		void OnDisable () {
		
		}

		void Start () {

		}

		void Update () {
			rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 15f;
			rotationY += Input.GetAxis("Mouse Y") * 15f;
			rotationY = Mathf.Clamp (rotationY, minY, maxY);
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}

		void SetInitialReferences () {

		}
	}
}