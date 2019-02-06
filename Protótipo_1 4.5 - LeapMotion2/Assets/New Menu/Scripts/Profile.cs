using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Profile
{

    private string _name;
    public string name
    {
        get { return this._name; }
    }


    public Profile(string name, string avatar)
    {
        this._name = name;
		this._avatar = avatar;
    }

	private string _avatar;
	public string avatar {
		get {
			return _avatar;
		}
	}
}
