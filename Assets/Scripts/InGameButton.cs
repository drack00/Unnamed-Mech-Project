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
	public bool controlled = false;
	public virtual void OnControl (Ray control, out Vector3 reticlePosition)
	{
		controlled = true;

		reticlePosition = transform.position;
	}
	public virtual void OnRelease ()
	{
		controlled = false;
	}
}
