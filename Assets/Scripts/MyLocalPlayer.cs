using UnityEngine;
using System.Collections;
using Steamworks;

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

	void OnEnable ()
	{
		webServerController = GetComponent<WebServerController> ();
	}

    public void Start()
    {
        rend = GetComponent<Renderer>();
        DontDestroyOnLoad(gameObject);
		GetSteamId ();
    }

	public void GetSteamId ()
	{
		if (SteamManager.Initialized) 
		{
			user.steam_id = SteamUser.GetSteamID().ToString();
			webServerController.Login();
		}
		else
		{
			Debug.LogError("Steam is not opened, cannot get Steam Id");
		}
	}
    
    public void SetupCharacter(Character character)
    {
		switch (character.character_name)
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
			webServerController.AddCharacter (character);	
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

}
