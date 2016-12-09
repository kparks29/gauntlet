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

	void Start ()
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
				JsonUtility.FromJsonOverwrite(www.downloadHandler.text, localPlayer.user);
			}
		}
	}

	public IEnumerator GetCharactersCall ()
	{
		GetCharactersResponse response = new GetCharactersResponse ();
		UnityWebRequest www = UnityWebRequest.Get (serverUrl + "/characters");
		www.SetRequestHeader ("Access-Token", localPlayer.user.token);
		yield return www.Send ();

		if (www.isError)
		{
			Debug.LogError (www.error);
		}
		else
		{
			JsonUtility.FromJsonOverwrite(www.downloadHandler.text, response);
			localPlayer.user.characters = response.characters;
			if (GetCharacterSuccessEvent != null)
			{
				GetCharacterSuccessEvent ();	
			}
		}
	}

	IEnumerator CreateCharacter (Character character)
	{
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
		www.SetRequestHeader ("Access-Token", localPlayer.user.token);
		yield return www.Send ();

		if (www.isError)
		{
			Debug.LogError (www.error);
		}
		else
		{
			StartCoroutine(GetCharactersCall ());
		}
	}

	public void AddCharacter (Character character)
	{
		StartCoroutine (CreateCharacter(character));
	}

	public void GetCharacters ()
	{
		StartCoroutine (GetCharactersCall());
	}

	void SetInitialReferences ()
	{
		localPlayer = GetComponent<MyLocalPlayer> ();
		if (serverUrl == null || serverUrl == string.Empty)
		{
			serverUrl = "http://gauntlet-server.herokuapp.com";
		}
	}
}
