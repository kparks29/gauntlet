using UnityEngine;
using System.Collections;
using Steamworks;
using UnityEngine.UI;

public class MyLocalPlayer : MonoBehaviour 
{
    public Color myColor = Color.black;
    public Transform myHead;
    public int myint;
	public User user = new User();

    //This will become private
    public bool newCharacter = true;
	public bool canContinue = false;
	public bool canSave = true;
	public Character currentCharacter;

	private Text continueDoorText;
	private WebServerController webServerController;


	void OnEnable ()
	{
		SetInitialReferences ();
		if (webServerController != null)
		{
			// when characters have been loaded check to see how many there are
			webServerController.GetCharacterSuccessEvent += CheckCharacters;
		}
	}

	void OnDisable ()
	{
		// remove event to listen for check characters
		webServerController.GetCharacterSuccessEvent -= CheckCharacters;
	}

    public void Start()
    {
		// don't destroy the local player
        DontDestroyOnLoad(gameObject);
		// get the steam id if possible
		GetSteamId ();
    }

	public void GetSteamId ()
	{
		// if steam is running get steam id and login user
		if (SteamManager.Initialized) 
		{
			user.steam_id = SteamUser.GetSteamID().ToString();
			webServerController.Login();
		}
		// if steam is not running, but a steam_id is present login
		else if (user.steam_id != null && user.steam_id != string.Empty)
		{
			webServerController.Login();
		}
		// if steam is not open log error, turn off continue door, and prevent user from saving character
		else
		{
			canSave = false;
			ToggleContinueDoor ();
			Debug.LogError("Steam is not opened, cannot get Steam Id");
		}
	}

	// check to see if there are any characters and turn canContinue to true allowing characters to continue game
	public void CheckCharacters ()
	{
		if (user.characters.Count > 0)
		{
			canContinue = true;
		}
	}

	// toggle door text
	public void ToggleContinueDoor ()
	{
		if (continueDoorText != null)
		{
			continueDoorText.text = continueDoorText.text != "Continue Adventure" ? "Continue Adventure" : "Not Available Yet";
		}
	}

    
    public void SetupCharacter(Character selectedCharacter)
    {
		// if it's a new character add the character
		if (newCharacter)
		{
			webServerController.AddCharacter (selectedCharacter);	
		}
		// if it's a returning character find it in the character list
		else
		{
			// Determine which character this is and set currentCharacter to it
			foreach (Character character in user.characters)
			{
				if (character.id == selectedCharacter.id)
				{
					currentCharacter = selectedCharacter;
				}
			}
		}
    }

	// get game objects and components
	void SetInitialReferences ()
	{
		webServerController = GetComponent<WebServerController> ();
		GameObject continueDoor = GameObject.Find ("Door2");
		if (continueDoor != null)
		{
			continueDoorText = continueDoor.GetComponentInChildren<Text> ();
		}
	}

}
