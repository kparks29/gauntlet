using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameTypeSelector : MonoBehaviour 
{

	private SelectableObjectHighlighter script;
	private string doorSelected;
	private bool isGameTypeSelected = false;
	public bool isNewGame = true;
	private MyLocalPlayer localPlayer;

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
	}

	void Start () 
	{

	}

	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Space) && doorSelected != null && !isGameTypeSelected) 
		{
			if (doorSelected == "Door1") 
			{
				localPlayer.newCharacter = true;
			} 
			else if (doorSelected == "Door2") 
			{
				localPlayer.newCharacter = false;
			}
			isGameTypeSelected = true;
			SceneManager.LoadScene ("CharacterSelector");
		}
	}

	void SetInitialReferences () 
	{
		GameObject fpsController = GameObject.Find ("FPSController");
		script = fpsController.GetComponent<SelectableObjectHighlighter> ();
		localPlayer = GetComponent<MyLocalPlayer>();
		DontDestroyOnLoad (this);
	}

	void SuccessCallback (Transform objectHit) 
	{
		doorSelected = objectHit.name;
	}

	void NoHitCallback (Transform lastObjectHit) 
	{
		doorSelected = null;
	}

	void OnSceneChanged (Scene previousScene, Scene newScene) 
	{
		if (newScene.name == "CharacterSelector") 
		{
			Debug.Log ("This is a new game: " + localPlayer.newCharacter.ToString());
		}
	}
}