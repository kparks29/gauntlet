using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;


public class CharacterSelector : MonoBehaviour 
{

	private SelectableObjectHighlighter script;
	private Transform activeObject;
	public string characterSelected;
    private MyLocalPlayer myPlayer;
	private WebServerController webServerController;
	private Character character = new Character();
	private bool characterLoadEventRemoved = false;

	void OnEnable () 
	{
		SetInitialReferences ();
		script.SuccessCallbackEvent += SuccessCallback;
		script.NoHitCallbackEvent += NoHitCallback;
		SceneManager.activeSceneChanged += OnSceneChanged;
		webServerController.GetCharacterSuccessEvent += CharactersLoaded;
	}

	void OnDisable () 
	{
		script.SuccessCallbackEvent -= SuccessCallback;
		script.NoHitCallbackEvent -= NoHitCallback;
		SceneManager.activeSceneChanged -= OnSceneChanged;
		RemoveCharacterLoadEventListener ();
	}

	void Start () 
	{
        
	}

	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Space) && activeObject != null) 
		{
            if(myPlayer != null)
            {
				character.character_name = activeObject.name;
				character.character_class = activeObject.name;
				myPlayer.SetupCharacter(character);
            }
			characterSelected = activeObject.name;
			SceneManager.LoadScene ("Town");
		}
	}

	void SetInitialReferences () 
	{
		script = GetComponent<SelectableObjectHighlighter> ();
		myPlayer = FindObjectOfType<MyLocalPlayer>();
		webServerController = FindObjectOfType<WebServerController>();
	}

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

	bool ValidOption (Transform objectHit) 
	{
		bool isValidObject = activeObject == null || activeObject.name != objectHit.name;

		isValidObject = isValidObject && objectHit.name != "Canvas";
		isValidObject = isValidObject && objectHit.name != "Door1" && objectHit.name != "Door2";

		return isValidObject;
	}

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
		if (newScene.name == "Town") 
		{
//			Debug.Log (characterSelected);
		}
		else if (newScene.name == "CharacterSelector")
		{
			if (myPlayer.newCharacter)
			{
				RemoveCharacterLoadEventListener ();
			}
			else
			{
				webServerController.GetCharacters ();
			}
		}
	}

	void CharactersLoaded ()
	{
		foreach(Character character in myPlayer.user.characters)
		{
			// SPAWN CHARACTERS HERE FOR CONTINUING GAME
			Debug.Log (character.character_class + ": " + character.uuid);
		}
	}

	void RemoveCharacterLoadEventListener ()
	{
		if (!characterLoadEventRemoved)
		{
			characterLoadEventRemoved = true;
			webServerController.GetCharacterSuccessEvent -= CharactersLoaded;
		}
	}

}
