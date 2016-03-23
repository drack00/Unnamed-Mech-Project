using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collider))]
public class InGameButton : MonoBehaviour, IInGameInput
{
	private Control control;
	public Control GetControl ()
	{
		return control;
	}
	public virtual void OnClick (Control _control)
	{
		control = _control;
	}
	public virtual void OnRelease ()
	{
		control = Control.None;
	}
}
