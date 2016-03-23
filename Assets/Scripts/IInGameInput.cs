using UnityEngine;
using System.Collections;

public interface IInGameInput
{
	void OnControl (Ray control, out Vector3 reticlePosition);
	void OnRelease ();
	Sprite GetDisplaySprite ();
}