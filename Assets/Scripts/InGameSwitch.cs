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
	public float lowerLimit;
	public float upperLimit;
	public bool invertControls;
	private Plane plane
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
			return new Plane (_planarPoints [0], _planarPoints [1], _planarPoints [2]);
		}
	}
	public float floatDefault;
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
			float _value = Mathf.InverseLerp(floatMin, floatMax, value);
			shaft.localPosition = Vector3.Lerp (startPoint, endPoint, _value);
		}
	}
	private float _floatValue;
	[System.Serializable]public enum ControlMethod
	{
		None,
		Gradual,
		Instant
	}
	public ControlMethod whileControlled;
	public float controlledSpeed;
	public ControlMethod whileReleased;
	public float releasedSpeed;
	private bool controlled = false;
	public Sprite displaySprite;
	public Sprite GetDisplaySprite ()
	{
		return displaySprite;
	}
	public virtual void OnControl (Ray control, out Vector3 reticlePosition)
	{
		controlled = true;

		//planar intersection
		float rayDistance;
		plane.Raycast (control, out rayDistance);
		reticlePosition = control.GetPoint (rayDistance);

		//distance along primary axis
		float distance = 0.0f;
		switch(primaryAxis)
		{
		case Axis.X:
			distance = transform.InverseTransformPoint (reticlePosition).x;
			break;
		case Axis.Y:
			distance = transform.InverseTransformPoint (reticlePosition).y;
			break;
		case Axis.Z:
			distance = transform.InverseTransformPoint (reticlePosition).z;
			break;
		}

		//invert controls if required
		if(invertControls)distance = -1 * distance;

		//clamp distance
		distance = Mathf.Clamp (distance, lowerLimit, upperLimit);

		//calculate value (ratio of distance between extemes, multiplied by float range)
		float _value = Mathf.InverseLerp(lowerLimit, upperLimit, distance);Debug.Log(_value);
		float value = Mathf.Lerp(floatMin, floatMax, _value);

		//output
		if(whileControlled == ControlMethod.Instant)floatValue = value;
		if(whileControlled == ControlMethod.Gradual)_floatValue = value;
	}
	public virtual void OnRelease ()
	{
		controlled = false;

		if(whileReleased == ControlMethod.Instant)floatValue = floatDefault;
		if(whileReleased == ControlMethod.Gradual)_floatValue = floatDefault;
	}

	void Start ()
	{
		floatValue = floatDefault;
	}
	void FixedUpdate ()
	{
		if(controlled) 
		{
			if(whileControlled == ControlMethod.Gradual)floatValue = Mathf.Lerp (floatValue, _floatValue, controlledSpeed * Time.fixedDeltaTime);
		} 
		else 
		{
			if(whileReleased == ControlMethod.Gradual)floatValue = Mathf.Lerp (floatValue, _floatValue, releasedSpeed * Time.fixedDeltaTime);
		}
	}
}
