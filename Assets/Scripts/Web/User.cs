using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class User
{
	public string uuid;
	public string steam_id;
	public string username;
	public string password;
	public string token;

	public List<CharacterStats> characters;
}

