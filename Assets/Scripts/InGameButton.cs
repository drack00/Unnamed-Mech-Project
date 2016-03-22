using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (Collider))]
public class InGameButton : MonoBehaviour, IInGameInput
{
	public virtual void OnClick ()
	{
		Debug.Log ("here");
	}
}
