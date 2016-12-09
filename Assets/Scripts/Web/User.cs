using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{
	public string uuid;
	public string steam_id;
	public string username;
	public string password;
	public string token;

	public List<Character> characters;
}