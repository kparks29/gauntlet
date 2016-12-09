using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;


public class CharacterSelector : MonoBehaviour 
{

	private SelectableObjectHighlighter script;
	private Transform activeObject;
	private Character characterSelected;
    private MyLocalPlayer myPlayer;
	private WebServerController webServerController;
	private Character character = new Character();
	private bool characterLoadEventRemoved = false;
	private CharacterSpawner spawner;

	void OnEnable () 
	{
		// all the setup calls
		SetInitialReferences ();
		// subscribe to events
		script.SuccessCallbackEvent += SuccessCallback;
		script.NoHitCallbackEvent += NoHitCallback;
		SceneManager.activeSceneChanged += OnSceneChanged;
	}

	void OnDisable () 
	{
		// unsubscribe to events
		script.SuccessCallbackEvent -= SuccessCallback;
		script.NoHitCallbackEvent -= NoHitCallback;
		SceneManager.activeSceneChanged -= OnSceneChanged;

		// run function to unsubscribe to this event
		RemoveCharacterLoadEventListener ();
	}

	void Update () 
	{
		// if space bar is pressed and character is highlighted 
		if (Input.GetKeyDown (KeyCode.Space) && activeObject != null) 
		{
			// if there is a local player object found
            if(myPlayer != null)
            {
				// get the character and send it to my player object
				character = activeObject.GetComponent<CharacterStatLoader> ().character;
				myPlayer.SetupCharacter(character);
            }
			characterSelected = character;
			SceneManager.LoadScene ("Town");
		}
	}

	// get initial references to components and objects
	void SetInitialReferences () 
	{
		script = GetComponent<SelectableObjectHighlighter> ();
		myPlayer = FindObjectOfType<MyLocalPlayer>();
		webServerController = FindObjectOfType<WebServerController>();
	}

	// gameobject was found in raycast, set the canvas active
	void SuccessCallback (Transform objectHit) 
	{
		
		if (ValidOption(objectHit)) 
		{
			activeObject = objectHit;
			var canvas = activeObject.FindChild ("Canvas");
			if (canvas != null) 
			{
				canvas.gameObject.SetActive (true);
			}
		}
	}

	// tests to see object should be hit
	bool ValidOption (Transform objectHit) 
	{
		bool isValidObject = activeObject == null || activeObject.name != objectHit.name;

		isValidObject = isValidObject && objectHit.name != "Canvas";
		isValidObject = isValidObject && objectHit.name != "Door1" && objectHit.name != "Door2";

		return isValidObject;
	}

	// nothing currently in view callback
	void NoHitCallback (Transform lastObjectHit) 
	{
		if (activeObject != null && lastObjectHit.name != "Canvas") 
		{
			var canvas = activeObject.FindChild ("Canvas");
			if (canvas != null) 
			{
				canvas.gameObject.SetActive (false);
			}
			activeObject = null;
		}
	}

	void OnSceneChanged (Scene previousScene, Scene newScene) 
	{
		// character has been selected when moving to town
		if (newScene.name == "Town") 
		{
			Debug.Log (characterSelected.character_class);
		}
		else if (newScene.name == "CharacterSelector")
		{
			// get the spawner once the player switches to character selection screen
			if (spawner == null)
			{
				var go = GameObject.Find ("CharacterSpawner");
				if (go != null)
				{
					spawner = go.GetComponent<CharacterSpawner> ();
				}
			}

			// if is a new character, remove the get characters listener and spawn the characters
			if (myPlayer.newCharacter)
			{
				RemoveCharacterLoadEventListener ();
				SpawnCharacters ();
			}
			// if is a returning player with characters remove the get characters listener and spawn the existing characters
			else if (myPlayer.user.characters.Count > 0)
			{
				RemoveCharacterLoadEventListener ();
				SpawnCharacters ();
			}
			// this still needs to get the characters
			else
			{
				webServerController.GetCharacterSuccessEvent += SpawnCharacters;
				webServerController.GetCharacters ();
			}
		}
	}
		
	void SpawnCharacters ()
	{
		if (spawner != null) 
		{
			spawner.SpawnCharacters ();
		}
		else
		{
			Debug.LogError ("Could not locate spawner");
		}
	}

	void RemoveCharacterLoadEventListener ()
	{
		// remove the event if the event hasn't been removed yet
		if (!characterLoadEventRemoved)
		{
			characterLoadEventRemoved = true;
			webServerController.GetCharacterSuccessEvent -= SpawnCharacters;
		}
	}

}
