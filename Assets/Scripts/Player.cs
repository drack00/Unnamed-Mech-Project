using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
	public static Player singleton
	{
		get
		{
			return FindObjectOfType<Player> ();
		}
	}

	[System.Serializable]public class Reticle
	{
		public Transform transform;
		public GameObject container;
		public GameObject display;
		public Vector3 defaultPosition;
		public Vector3 defaultScale;
		public Vector3 controlScale;
		public Image hoverDisplay;
		public GameObject controlDisplay;
		public bool bindable;
		private bool binding
		{
			get
			{
				return bindable && reset;
			}
		}
		private bool show
		{
			get
			{
				return !bindable || reset || grab;
			}
		}
		public Transform referenceRet;
		public string analogHorizontalInput;
		public string analogVerticalInput;
		public float speed;
		private Vector2 analog
		{
			get
			{
				float x = Input.GetAxis (analogHorizontalInput);
				float y = Input.GetAxis (analogVerticalInput);

				return new Vector2 (x, y);
			}
		}
		public string resetInput;
		private bool reset
		{
			get
			{
				return Input.GetButton (resetInput);
			}
		}
		public string grabInput;
		public bool useGrabAxis;
		public bool invertGrabAxis;
		private bool grab
		{
			get
			{
				if(useGrabAxis)
				{
					if(invertGrabAxis)return Mathf.Approximately(Input.GetAxisRaw (grabInput), -1.0f);
					return Mathf.Approximately(Input.GetAxisRaw (grabInput), 1.0f);
				}
				return Input.GetButton (grabInput);
			}
		}
		private Ray ray
		{
			get
			{
				return new Ray (container.transform.position, container.transform.TransformDirection (Vector3.forward));
			}
		}
		public IInGameInput hover
		{
			get
			{
				IInGameInput _hover = null;

				RaycastHit[] hits = Physics.RaycastAll (ray);
				foreach(RaycastHit hit in hits)
				{
					if(hit.collider != null && hit.collider.gameObject != null && hit.collider.gameObject.GetComponent<IInGameInput> () != null)
					{
						_hover = hit.collider.gameObject.GetComponent<IInGameInput> ();
						break;
					}
				}

				return _hover;
			}
		}
		private IInGameInput _control;
		public IInGameInput control
		{
			get
			{
				return _control;
			}
			private set
			{
				if(!grab)
				{
					if(_control != null)_control.OnRelease ();

					hoverDisplay.gameObject.SetActive (value != null);
					if(value != null)hoverDisplay.sprite = value.GetDisplaySprite ();
					else hoverDisplay.sprite = null;
					controlDisplay.SetActive (false);

					value = null;
				} 
				else controlDisplay.SetActive (true);

				if(_control == null || !grab)_control = value;
			}
		}
		public void Update (float deltaTime)
		{
			if(!bindable)
			{
				if(reset) 
				{
					container.transform.localPosition = Vector3.zero;
					container.transform.localRotation = Quaternion.identity;
				}
				container.transform.RotateAround (container.transform.position, transform.TransformDirection(Vector3.up), analog.x * speed * deltaTime);
				container.transform.RotateAround (container.transform.position, transform.TransformDirection(Vector3.right), analog.y * speed * deltaTime);
			}
			if(binding)
			{
				control = null;
				if(grab)container.transform.rotation = referenceRet.rotation;
			}
			else control = hover;
			Vector3 containerPosition = defaultPosition;
			if (control != null) 
			{
				Vector3 _containerPosition;
				control.OnControl (ray, out _containerPosition);
				containerPosition = container.transform.InverseTransformPoint(_containerPosition);
				display.transform.localScale = controlScale;
			} 
			else display.transform.localScale = defaultScale;
			display.transform.localPosition = containerPosition;
			display.gameObject.SetActive (show);
		}
	}

	public Reticle grab1;
	public Reticle[] shortcuts1;
	public Reticle grab2;
	public Reticle[] shortcuts2;

	void FixedUpdate ()
	{
		grab1.Update (Time.fixedDeltaTime);
		foreach(Reticle ret in shortcuts1)
		{
			ret.Update (Time.fixedDeltaTime);
		}
		grab2.Update (Time.fixedDeltaTime);
		foreach(Reticle ret in shortcuts2)
		{
			ret.Update (Time.fixedDeltaTime);
		}
	}
}
