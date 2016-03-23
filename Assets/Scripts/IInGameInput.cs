using UnityEngine;
using System.Collections;

public interface IInGameInput
{
	void OnControl (Ray control);
	void OnRelease (Ray control);
	Sprite GetDisplaySprite ();
}