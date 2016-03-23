using UnityEngine;
using System.Collections;

public interface IInGameInput
{
	void OnClick (Control control);
	Control GetControl ();
}

public enum Control
{
	None,
	Fire1,
	Fire2
}