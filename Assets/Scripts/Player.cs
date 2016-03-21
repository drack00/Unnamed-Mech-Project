using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	void Start ()
	{
		Cursor.visible = false;
	}

	void Update ()
	{
		if(Input.GetButtonDown ("Fire1"))OnFire1 ();
	}

	private void OnFire1 ()
	{
		RaycastHit[] hits = Physics.RaycastAll (Camera.main.ScreenToWorldPoint (Vector3.zero), transform.TransformDirection (Vector3.forward));
		foreach(RaycastHit hit in hits)
		{
			if(hit.collider != null && hit.collider.gameObject != null && hit.collider.gameObject.GetComponent<InGameButton> () != null)
			{
				InGameButton button = hit.collider.gameObject.GetComponent<InGameButton> ();
				button.OnClick ();
				break;
			}
		}
	}
}
