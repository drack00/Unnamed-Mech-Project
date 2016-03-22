using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collider))]
public class InGameSwitch : MonoBehaviour, IInGameInput
{
	public Transform shaft;
	public Vector3 startPoint;
	public Vector3 endPoint;

	[System.Serializable]public enum Axis
	{
		X,
		Y,
		Z
	}
	public Axis primaryAxis;
	public Axis secondaryAxis;
	private Vector3[] planarPoints
	{
		get
		{
			Vector3[] _planarPoints = new Vector3[3];
			_planarPoints [0] = transform.position;
			switch(primaryAxis)
			{
			case Axis.X:
				_planarPoints [1] = transform.TransformPoint(Vector3.right);
				break;
			case Axis.Y:
				_planarPoints [1] = transform.TransformPoint(Vector3.up);
				break;
			case Axis.Z:
				_planarPoints [1] = transform.TransformPoint(Vector3.forward);
				break;
			}
			switch(secondaryAxis)
			{
			case Axis.X:
				_planarPoints [2] = transform.TransformPoint(Vector3.right);
				break;
			case Axis.Y:
				_planarPoints [2] = transform.TransformPoint(Vector3.up);
				break;
			case Axis.Z:
				_planarPoints [2] = transform.TransformPoint(Vector3.forward);
				break;
			}
			return _planarPoints;
		}
	}

	public float floatMin;
	public float floatMax;
	public float floatValue
	{
		get
		{
			float x = Mathf.InverseLerp (startPoint.x, endPoint.x, shaft.localPosition.x);
			float y = Mathf.InverseLerp (startPoint.y, endPoint.y, shaft.localPosition.y);
			float z = Mathf.InverseLerp (startPoint.z, endPoint.z, shaft.localPosition.z);
			return new Vector3 (x, y, z).magnitude;
		}
		private set
		{
			shaft.localPosition = Vector3.Lerp (startPoint, endPoint, value);
		}
	}
	public float floatDelay;
	public float floatDefault;
	public bool resetOnRelease;
	public bool gradualChange;
	private float _floatValue;
	private IEnumerator MoveLever ()
	{
		while(Player.singleton.holdingFire1)
		{
			if (gradualChange)_floatValue = GetCursorPositionValue ();
			else floatValue = GetCursorPositionValue ();
			yield return null;
		}

		OnRelease ();
	}
	private float GetCursorPositionValue ()
	{
		float value = 0.0f;

		//planar intersection
		Ray ray = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
		Plane plane = new Plane (planarPoints [0], planarPoints [1], planarPoints [2]);
		float rayDistance;
		plane.Raycast (ray, out rayDistance);
		Vector3 planarPosition = ray.GetPoint (rayDistance);

		//position of intersection relative to transform origin
		Vector3 relativePosition = transform.InverseTransformPoint (planarPosition);

		//distance and bounds along primary axis
		float distance = 0.0f;
		float colliderMin = 0.0f;
		float colliderMax = 0.0f;
		switch(primaryAxis)
		{
		case Axis.X:
			distance = relativePosition.x;
			colliderMin = GetComponent<Collider> ().bounds.min.x;
			colliderMax = GetComponent<Collider> ().bounds.max.x;
			break;
		case Axis.Y:
			distance = relativePosition.y;
			colliderMin = GetComponent<Collider> ().bounds.min.y;
			colliderMax = GetComponent<Collider> ().bounds.max.y;
			break;
		case Axis.Z:
			distance = relativePosition.z;
			colliderMin = GetComponent<Collider> ().bounds.min.z;
			colliderMax = GetComponent<Collider> ().bounds.max.z;
			break;
		}

		//calculate value (ratio of distance between extemes, multiplied by float range)
		value = (distance / (colliderMax - colliderMin)) * (floatMax - floatMin);

		//clamp output
		value = Mathf.Clamp (value, floatMin, floatMax);

		return value;
	}

	public virtual void OnClick ()
	{
		StartCoroutine (MoveLever ());
	}
	public virtual void OnRelease ()
	{
		if (resetOnRelease) 
		{
			if (gradualChange)_floatValue = floatDefault;
			else floatValue = floatDefault;
		}
	}

	void Start ()
	{
		floatValue = floatDefault;
	}
	void Update ()
	{
		if (gradualChange)floatValue = Mathf.Lerp (floatValue, _floatValue, floatDelay * Time.deltaTime);
	}
}
