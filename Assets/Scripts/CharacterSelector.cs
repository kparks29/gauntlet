using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;


public class CharacterSelector : MonoBehaviour 
{

	private SelectableObjectHighlighter script;
	private Transform activeObject;
	public Character characterSelected;
    private MyLocalPlayer myPlayer;
	private WebServerController webServerController;
	private Character character = new Character();
	private bool characterLoadEventRemoved = false;
	private CharacterSpawner spawner;

	void OnEnable () 
	{
		SetInitialReferences ();
		script.SuccessCallbackEvent += SuccessCallback;
		script.NoHitCallbackEvent += NoHitCallback;
		SceneManager.activeSceneChanged += OnSceneChanged;
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
				character = activeObject.GetComponent<CharacterStatLoader> ().character;
				myPlayer.SetupCharacter(character);
            }
			characterSelected = character;
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
			Debug.Log (characterSelected.character_class);
		}
		else if (newScene.name == "CharacterSelector")
		{
			// get the spawner once player switches to scene
			if (spawner == null)
			{
				var go = GameObject.Find ("CharacterSpawner");
				if (go != null)
				{
					spawner = go.GetComponent<CharacterSpawner> ();
				}
			}

			if (myPlayer.newCharacter)
			{
				RemoveCharacterLoadEventListener ();
				SpawnCharacters ();
			}
			else if (myPlayer.user.characters.Count > 0)
			{
				RemoveCharacterLoadEventListener ();
				SpawnCharacters ();
			}
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
		if (!characterLoadEventRemoved)
		{
			characterLoadEventRemoved = true;
			webServerController.GetCharacterSuccessEvent -= SpawnCharacters;
		}
	}

}
