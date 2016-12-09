using UnityEngine;
using System.Collections;

public class FPSController : MonoBehaviour {
        
	private float maxY = 60;
	private float minY = -60;
	private float rotationY = 0F;
	private float rotationX;

    public float turnSpeed = 30;
    public float moveSpeed = 10.0f;
    public Transform player;

	void OnEnable () 
	{
		SetInitialReferences ();
	}

	void Start () 
	{
		if (player == null) 
		{
			Debug.LogWarning ("Setup Player in FPSController");
		}
	}

    void Update()
    {
        rotationX = player.localEulerAngles.y + Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, minY, maxY);
        player.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

        player.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed, 0, Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed);
    }

	void SetInitialReferences () 
	{
		DontDestroyOnLoad (this);
	}
}