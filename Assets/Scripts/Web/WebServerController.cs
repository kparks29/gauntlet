using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WebServerController : MonoBehaviour 
{

	private MyLocalPlayer localPlayer;
	public string serverUrl;

	private CharacterStats characterToAdd;

	void OnEnable ()
	{
		SetInitialReferences ();
	}

	void Start ()
	{
		if (localPlayer != null)
		{
			StartCoroutine (LoadPlayer ("test"));
		}
	}
	
	IEnumerator LoadPlayer (string test)
	{
		Debug.Log (test);
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
				Debug.Log (www.error);
			}
			else
			{
				JsonUtility.FromJsonOverwrite(www.downloadHandler.text, localPlayer.user);
			}
		}
	}

	IEnumerator GetCharacters ()
	{
		UnityWebRequest www = UnityWebRequest.Get (serverUrl + "/characters");
		www.SetRequestHeader ("Access-Token", localPlayer.user.token);
		yield return www.Send ();

		if (www.isError)
		{
			Debug.Log (www.error);
		}
		else
		{
			GetCharactersResponse response = new GetCharactersResponse ();
			JsonUtility.FromJsonOverwrite (www.downloadHandler.text, response);
			localPlayer.user.characters = response.characters;
		}
	}

	IEnumerator CreateCharacter ()
	{
		UnityWebRequest www = UnityWebRequest.Post (serverUrl + "/characters", JsonUtility.ToJson (characterToAdd));
		www.SetRequestHeader ("Access-Token", localPlayer.user.token);
		yield return www.Send ();

		if (www.isError)
		{
			Debug.Log (www.error);
		}
		else
		{
			Debug.Log(www.downloadHandler.text);
		}
	}

	public void AddCharacter (CharacterStats character)
	{
		characterToAdd = character;
		StartCoroutine (CreateCharacter ());
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
