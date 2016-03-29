using UnityEngine;
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
		
	//public int locationIndex;
		
	public GameObject reticle1;
	public Transform reticle1Display;
	public Vector3 reticle1DefaultPosition;
	public Vector3 reticle1DefaultScale;
	public Vector3 reticle1ControlScale;
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
					if(_reticle1Control != null)_reticle1Control.OnRelease ();

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
	public bool thumb1
	{
		get
		{
			return Input.GetButton ("Thumb1");
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
	public Transform reticle2Display;
	public Vector3 reticle2DefaultPosition;
	public Vector3 reticle2DefaultScale;
	public Vector3 reticle2ControlScale;
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
					if(_reticle2Control != null)_reticle2Control.OnRelease ();

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
	public bool thumb2
	{
		get
		{
			return Input.GetButton ("Thumb2");
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
		
	void Update ()
	{
		if(thumb1) 
		{
			reticle1.transform.localPosition = Vector3.zero;
			reticle1.transform.localRotation = Quaternion.identity;
		}
		reticle1.transform.RotateAround (reticle1.transform.position, transform.up, analog1.x);
		reticle1.transform.RotateAround (reticle1.transform.position, transform.right, -1 * analog1.y);
		reticle1Control = reticle1Hover;
		Vector3 reticle1Position = reticle1DefaultPosition;
		if (reticle1Control != null) 
		{
			Vector3 _reticle1Position;
			reticle1Control.OnControl (reticle1Ray, out _reticle1Position);
			reticle1Position = reticle1.transform.InverseTransformPoint(_reticle1Position);
			reticle1Display.localScale = reticle1ControlScale;
		} 
		else 
		{
			reticle1Display.localScale = reticle1DefaultScale;
		}
		reticle1Display.localPosition = reticle1Position;

		if(thumb2) 
		{
			reticle2.transform.localPosition = Vector3.zero;
			reticle2.transform.localRotation = Quaternion.identity;
		}
		reticle2.transform.RotateAround (reticle2.transform.position, transform.up, analog2.x);
		reticle2.transform.RotateAround (reticle2.transform.position, transform.right, analog2.y);
		reticle2Control = reticle2Hover;
		Vector3 reticle2Position = reticle2DefaultPosition;
		if(reticle2Control != null)
		{
			Vector3 _reticle2Position;
			reticle2Control.OnControl (reticle2Ray, out _reticle2Position);
			reticle2Position = reticle2.transform.InverseTransformPoint(_reticle2Position);
			reticle2Display.localScale = reticle2ControlScale;
		} 
		else 
		{
			reticle2Display.localScale = reticle2DefaultScale;
		}
		reticle2Display.localPosition = reticle2Position;
	}
}
