using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collider))]
public class InGameButton : MonoBehaviour, IInGameInput
{
	public Sprite displaySprite;
	public Sprite GetDisplaySprite ()
	{
		return displaySprite;
	}
	public virtual void OnControl (Ray control)
	{
		
	}
	public virtual void OnRelease (Ray control)
	{
		
	}
}
