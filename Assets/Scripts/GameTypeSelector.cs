using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameTypeSelector : MonoBehaviour 
{

	private SelectableObjectHighlighter script;
	private string doorSelected;
	private bool isGameTypeSelected = false;
	private MyLocalPlayer localPlayer;

	void OnEnable () 
	{
		SetInitialReferences ();
		// subscribe to events
		script.SuccessCallbackEvent += SuccessCallback;
		script.NoHitCallbackEvent += NoHitCallback;
	}

	void OnDisable () 
	{
		// unsubscribe to events
		script.SuccessCallbackEvent -= SuccessCallback;
		script.NoHitCallbackEvent -= NoHitCallback;
	}

	void Update () 
	{
		// check to see when space bar is pressed and viewing a door
		if (Input.GetKeyDown (KeyCode.Space) && doorSelected != null && !isGameTypeSelected) 
		{
			// door 1 means new character
			if (doorSelected == "Door1") 
			{
				localPlayer.newCharacter = true;
			}
			// if game cannot be continued ignore door 2
			else if (!localPlayer.canContinue && doorSelected == "Door2")
			{
				Debug.Log ("cannot get to door 2");
				return;
			}
			// if game can be continued it means it is not a new character
			else if (doorSelected == "Door2") 
			{
				localPlayer.newCharacter = false;
			}
			// game type has been selected, load next scene
			isGameTypeSelected = true;
			SceneManager.LoadScene ("CharacterSelector");
		}
	}

	// get initial references to game objects and components
	// prevent the game type selector from being destroyed
	void SetInitialReferences () 
	{
		GameObject fpsController = GameObject.Find ("FPSController");
		script = fpsController.GetComponent<SelectableObjectHighlighter> ();
		localPlayer = GetComponent<MyLocalPlayer>();
		DontDestroyOnLoad (this);
	}

	// sets which door has been selected if door has been hit
	void SuccessCallback (Transform objectHit) 
	{
		doorSelected = objectHit.name;
	}

	// sets the door to null if nothing was hit
	void NoHitCallback (Transform lastObjectHit) 
	{
		doorSelected = null;
	}
}