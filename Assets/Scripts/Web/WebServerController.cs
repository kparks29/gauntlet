using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WebServerController : MonoBehaviour {

	private MyLocalPlayer localPlayer;
	public string serverUrl;
	public delegate void GetCharacterSuccess ();
	public event GetCharacterSuccess GetCharacterSuccessEvent;

	void OnEnable ()
	{
		SetInitialReferences ();
	}

	// public call to login
	public void Login ()
	{
		if (localPlayer != null)
		{
			StartCoroutine (LoadPlayer ());
		}
	}
	
	IEnumerator LoadPlayer ()
	{
		bool isValid = false;
		WWWForm form = new WWWForm();
		// checks to see if it needs to set the steam_id or username property
		if (localPlayer.user.steam_id != null && localPlayer.user.steam_id != string.Empty)
		{
			form.AddField ("steam_id", localPlayer.user.steam_id);
			isValid = true;
		}
		else if (localPlayer.user.username != null && localPlayer.user.username != string.Empty && localPlayer.user.password != null && localPlayer.user.password != string.Empty)
		{
			form.AddField ("username", localPlayer.user.username);
			form.AddField ("password", localPlayer.user.password);
			isValid = true;
		}

		// does a POST to /users (logs in the user or creates the users and logins in)
		if (isValid)
		{
			UnityWebRequest www = UnityWebRequest.Post (serverUrl + "/users", form);
			yield return www.Send ();

			if (www.isError)
			{
				Debug.LogError (www.error);
			}
			else
			{
				// set the results to the user property of MyLocalPlayer
				JsonUtility.FromJsonOverwrite(www.downloadHandler.text, localPlayer.user);
				// get the characters for this user
				GetCharacters ();
			}
		}
	}

	// gets a list of characters for the users and sets it to the user.characters propery of MyLocalPlayer
	IEnumerator GetCharactersCall ()
	{
		GetCharactersResponse response = new GetCharactersResponse ();
		UnityWebRequest www = UnityWebRequest.Get (serverUrl + "/characters");
		www.SetRequestHeader ("Access-Token", localPlayer.user.token);
		yield return www.Send ();

		if (www.isError) {
			Debug.LogError (www.error);
		} else {
			JsonUtility.FromJsonOverwrite (www.downloadHandler.text, response);
			localPlayer.user.characters = response.characters;
			// if successfull and there are callbacks subscribed to this event, trigger it
			if (GetCharacterSuccessEvent != null) {
				GetCharacterSuccessEvent ();	
			}
		}
	}

	IEnumerator CreateCharacter (Character character)
	{
		// creates a new character by adding it's class and name.
		// the server will set the rest of it's default properties for protections
		WWWForm form = new WWWForm();
		if (character.character_class != null)
		{
			form.AddField ("character_class", character.character_class);
		}
		if (character.character_name == null)
		{
			character.character_name = string.Empty;
		}
		form.AddField ("character_name", character.character_name);
		UnityWebRequest www = UnityWebRequest.Post (serverUrl + "/characters", form);
		// only logged in users can create a character
		www.SetRequestHeader ("Access-Token", localPlayer.user.token);
		yield return www.Send ();

		if (www.isError)
		{
			Debug.LogError (www.error);
		}
		else
		{
			// gets the character list after creating the character
			// this allows us to retrieve the starting stats from the server
			GetCharacters ();
		}
	}

	// public call to add characters
	public void AddCharacter (Character character)
	{
		// if the player is logged in then we can create a character
		if (localPlayer.canSave)
		{
			StartCoroutine (CreateCharacter (character));
		}
		else
		{
			// just add the default settings that we set to the character list and run the event handler
			localPlayer.user.characters.Add(character);
			if (GetCharacterSuccessEvent != null) {
				GetCharacterSuccessEvent ();	
			}
		}
	}

	// public call to get characters
	public void GetCharacters ()
	{
		if (localPlayer.canSave)
		{
			StartCoroutine (GetCharactersCall ());
		}
	}

	// gets references to the local player and also sets a url for the server if one isnt present
	void SetInitialReferences ()
	{
		localPlayer = GetComponent<MyLocalPlayer> ();
		if (serverUrl == null || serverUrl == string.Empty)
		{
			serverUrl = "http://gauntlet-server.herokuapp.com";
		}
	}
}
