using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WebServerController : MonoBehaviour {

	private MyLocalPlayer localPlayer;
	public string serverUrl;

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
		if (localPlayer.steam_id != null && localPlayer.steam_id != string.Empty)
		{
			form.AddField ("steam_id", localPlayer.steam_id);
			isValid = true;
		}
		else if (localPlayer.username != null && localPlayer.username != string.Empty && localPlayer.password != null && localPlayer.password != string.Empty)
		{
			form.AddField ("username", localPlayer.username);
			form.AddField ("password", localPlayer.password);
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
				Debug.Log (www.downloadHandler.text);
			}
		}
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
