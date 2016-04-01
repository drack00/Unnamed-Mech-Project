using UnityEngine;
using System.Collections;

public class ChangeTargetButton : InGameButton 
{
	public Mecha mecha;
	public bool nextTarget;
	public override void OnControl (Ray control, out Vector3 reticlePosition)
	{
		if(!controlled)
		{
			mecha.ChangeTarget (nextTarget);
		}

		base.OnControl (control, out reticlePosition);
	}
	public override void OnRelease ()
	{
		base.OnRelease ();
	}
}