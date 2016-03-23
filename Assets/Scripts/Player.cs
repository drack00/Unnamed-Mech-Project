using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public static Player singleton
	{
		get
		{
			return FindObjectOfType<Player> ();
		}
	}
		
	public Vector2 analog1
	{
		get
		{
			float x = Input.GetAxis ("Horizontal");
			float y = Input.GetAxis ("Vertical");

			return new Vector2 (x, y);
		}
	}
	public bool fire1
	{
		get
		{
			return Input.GetButton ("Fire1");
		}
	}
	public Vector2 analog2
	{
		get
		{
			float x = Input.GetAxis ("Horizontal");
			float y = Input.GetAxis ("Vertical");

			return new Vector2 (x, y);
		}
	}
	public bool fire2
	{
		get
		{
			return Input.GetButton ("Fire2");
		}
	}

	public GameObject reticle1;
	public GameObject reticle2;
	public float reticleSpeed;

	void OnDrawGizmosSelected ()
	{
		Gizmos.DrawRay (reticle1.transform.position, reticle1.transform.TransformDirection (Vector3.forward));
		Gizmos.DrawRay (reticle2.transform.position, reticle2.transform.TransformDirection (Vector3.forward));
	}

	void Start ()
	{
		Cursor.visible = false;
	}

	void Update ()
	{
		if(analog1 != Vector2.zero)OnAnalog1 ();
		if(Input.GetButtonDown ("Fire1"))OnFire1 ();
		if(analog2 != Vector2.zero)OnAnalog2 ();
		if(Input.GetButtonDown ("Fire2"))OnFire2 ();
	}
		
	private void OnAnalog1 ()
	{
		reticle1.transform.RotateAround (reticle1.transform.position, transform.up, analog1.x);
		reticle1.transform.RotateAround (reticle1.transform.position, transform.right, -1 * analog1.y);
	}
	private void OnFire1 ()
	{
		RaycastHit[] hits = Physics.RaycastAll (reticle1.transform.position, reticle1.transform.TransformDirection (Vector3.forward));
		foreach(RaycastHit hit in hits)
		{
			if(hit.collider != null && hit.collider.gameObject != null && hit.collider.gameObject.GetComponent<IInGameInput> () != null)
			{
				IInGameInput button = hit.collider.gameObject.GetComponent<IInGameInput> ();
				if(button.GetControl () == Control.None) 
				{
					button.OnClick (Control.Fire1);
					break;
				}
			}
		}
	}
	private void OnAnalog2 ()
	{
		reticle2.transform.RotateAround (reticle2.transform.position, transform.up, analog2.x);
		reticle2.transform.RotateAround (reticle2.transform.position, transform.right, -1 * analog2.y);
	}
	private void OnFire2 ()
	{
		RaycastHit[] hits = Physics.RaycastAll (reticle2.transform.position, reticle2.transform.TransformDirection (Vector3.forward));
		foreach(RaycastHit hit in hits)
		{
			if(hit.collider != null && hit.collider.gameObject != null && hit.collider.gameObject.GetComponent<IInGameInput> () != null)
			{
				IInGameInput button = hit.collider.gameObject.GetComponent<IInGameInput> ();
				if(button.GetControl () == Control.None)
				{
					button.OnClick (Control.Fire2);
					break;
				}
			}
		}
	}
}
