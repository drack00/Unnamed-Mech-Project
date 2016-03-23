﻿using UnityEngine;
using UnityEngine.UI;
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
		
	public GameObject reticle1;
	public Image reticle1HoverDisplay;
	public GameObject reticle1ControlDisplay;
	public Ray reticle1Ray
	{
		get
		{
			return new Ray (reticle1.transform.position, reticle1.transform.TransformDirection (Vector3.forward));
		}
	}
	public IInGameInput reticle1Hover
	{
		get
		{
			IInGameInput _reticle1Hover = null;

			RaycastHit[] hits = Physics.RaycastAll (reticle1Ray);
			foreach(RaycastHit hit in hits)
			{
				if(hit.collider != null && hit.collider.gameObject != null && hit.collider.gameObject.GetComponent<IInGameInput> () != null)
				{
					_reticle1Hover = hit.collider.gameObject.GetComponent<IInGameInput> ();
					break;
				}
			}

			return _reticle1Hover;
		}
	}
	private IInGameInput _reticle1Control;
	public IInGameInput reticle1Control
	{
		get
		{
			return _reticle1Control;
		}
		set
		{
			if(reticle2Control != value || value == null)
			{
				if(!fire1)
				{
					if(_reticle1Control != null)_reticle1Control.OnRelease (reticle1Ray);

					reticle1HoverDisplay.gameObject.SetActive (value != null);
					if(value != null)reticle1HoverDisplay.sprite = value.GetDisplaySprite ();
					else reticle1HoverDisplay.sprite = null;
					reticle1ControlDisplay.SetActive (false);

					value = null;
				} 
				else reticle1ControlDisplay.SetActive (true);

				if(_reticle1Control == null || !fire1)_reticle1Control = value;
			}
		}
	}
	public float reticle1Speed;
	public Vector2 analog1
	{
		get
		{
			float x = Input.GetAxis ("Horizontal1");
			float y = Input.GetAxis ("Vertical1");

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

	public GameObject reticle2;
	public Image reticle2HoverDisplay;
	public GameObject reticle2ControlDisplay;
	public Ray reticle2Ray
	{
		get
		{
			return new Ray (reticle2.transform.position, reticle2.transform.TransformDirection (Vector3.forward));
		}
	}
	public IInGameInput reticle2Hover
	{
		get
		{
			IInGameInput _reticle2Hover = null;

			RaycastHit[] hits = Physics.RaycastAll (reticle2Ray);
			foreach(RaycastHit hit in hits)
			{
				if(hit.collider != null && hit.collider.gameObject != null && hit.collider.gameObject.GetComponent<IInGameInput> () != null)
				{
					_reticle2Hover = hit.collider.gameObject.GetComponent<IInGameInput> ();
					break;
				}
			}

			return _reticle2Hover;
		}
	}
	private IInGameInput _reticle2Control;
	public IInGameInput reticle2Control
	{
		get
		{
			return _reticle2Control;
		}
		set
		{
			if (reticle1Control != value || value == null) 
			{
				if(!fire2)
				{
					if(_reticle2Control != null)_reticle2Control.OnRelease (reticle2Ray);

					reticle2HoverDisplay.gameObject.SetActive (value != null);
					if(value != null)reticle2HoverDisplay.sprite = value.GetDisplaySprite ();
					else reticle2HoverDisplay.sprite = null;
					reticle2ControlDisplay.SetActive (false);

					value = null;
				} 
				else reticle2ControlDisplay.SetActive (true);

				if(_reticle2Control == null || !fire2)_reticle2Control = value;
			}
		}
	}
	public float reticle2Speed;
	public Vector2 analog2
	{
		get
		{
			float x = Input.GetAxis ("Horizontal2");
			float y = Input.GetAxis ("Vertical2");

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

	void OnDrawGizmosSelected ()
	{
		Gizmos.DrawRay (reticle1Ray);
		Gizmos.DrawRay (reticle2Ray);
	}

	void Start ()
	{
		Cursor.visible = false;
	}

	void FixedUpdate ()
	{
		if(reticle1Control != null)reticle1Control.OnControl (reticle1Ray);
		if(reticle2Control != null)reticle2Control.OnControl (reticle2Ray);
	}

	void Update ()
	{
		reticle1.transform.RotateAround (reticle1.transform.position, transform.up, analog1.x);
		reticle1.transform.RotateAround (reticle1.transform.position, transform.right, -1 * analog1.y);
		reticle1Control = reticle1Hover;

		reticle2.transform.RotateAround (reticle2.transform.position, transform.up, analog2.x);
		reticle2.transform.RotateAround (reticle2.transform.position, transform.right, analog2.y);
		reticle2Control = reticle2Hover;
	}
}
