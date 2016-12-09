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

	private WebServerController webServerController;

    Renderer rend;
    //This will become private
    public bool newCharacter = true;
	public bool canContinue = false;
	public bool canSave = true;
	public Character currentCharacter;

	private Text continueDoorText;

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
        rend = GetComponent<Renderer>();
        DontDestroyOnLoad(gameObject);
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

	public void ToggleContinueDoor ()
	{
		if (continueDoorText != null)
		{
			continueDoorText.text = continueDoorText.text != "Continue Adventure" ? "Continue Adventure" : "Not Available Yet";
		}
	}

    
    public void SetupCharacter(Character selectedCharacter)
    {
		switch (selectedCharacter.character_name)
        {
            case "Wizard":
                SetupWizard();
                break;
            case "Warrior":
                SetupWarrior();
                break;
            case "Archer":
                SetupArcher();
                break;
            default:
                SetupWarrior();
                break;
        }

		if (newCharacter)
		{
			webServerController.AddCharacter (selectedCharacter);	
		}
		else
		{
			currentCharacter = selectedCharacter; // remove this once characters are spawned

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

    void SetupWizard()
    {
        if (newCharacter)
        {
            myColor = Color.blue;
        }
        else
        {
            myColor = new Color(myColor.r, myColor.g, myColor.b - 75);
        }
    }

    void SetupWarrior()
    {
        if (newCharacter)
        {
            myColor = Color.red;
        }
        else
        {
            myColor = new Color(myColor.r - 75, myColor.g, myColor.b);
        }
    }

    void SetupArcher()
    {
        if (newCharacter)
        {
            myColor = Color.green;
        }
        else
        {
            myColor = new Color(myColor.r, myColor.g - 75, myColor.b);
        }
    }

    void SetupSkills()
    {

    }

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
