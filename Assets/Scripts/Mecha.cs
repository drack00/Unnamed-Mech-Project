using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mecha : MonoBehaviour 
{
	public InGameSwitch leftBiasInput;
	public InGameSwitch rightBiasInput;
	public enum InputBiasSign
	{
		Zero,
		Positive,
		Negative
	}
	public InputBiasSign leftBiasSign
	{
		get
		{
			if(Mathf.Approximately(leftBiasInput.floatValue, 0f))return InputBiasSign.Zero;
			if(leftBiasInput.floatValue > 0.0f)return InputBiasSign.Positive;
			if(leftBiasInput.floatValue < 0.0f)return InputBiasSign.Negative;
			return InputBiasSign.Zero;
		}
	}
	public InputBiasSign rightBiasSign
	{
		get
		{
			if(Mathf.Approximately(rightBiasInput.floatValue, 0))return InputBiasSign.Zero;
			if(rightBiasInput.floatValue > 0.0f)return InputBiasSign.Positive;
			if(rightBiasInput.floatValue < 0.0f)return InputBiasSign.Negative;
			return InputBiasSign.Zero;
		}
	}
	public Vector2 inputDir
	{
		get
		{
			float xBias = 0.0f;
			xBias += leftBiasSign != InputBiasSign.Negative ? 
				-1.0f * Mathf.Abs(leftBiasInput.floatValue) : 
				-1.0f * Mathf.Abs(leftBiasInput.floatValue) / 2.0f;
			xBias += rightBiasSign != InputBiasSign.Negative ? 
				Mathf.Abs(rightBiasInput.floatValue) : 
				Mathf.Abs(rightBiasInput.floatValue) / 2.0f;
			
			float yBias = 0.0f;
			if(leftBiasSign != InputBiasSign.Negative && rightBiasSign != InputBiasSign.Negative)yBias = ((2.0f * leftBiasInput.floatValue) * (2.0f * rightBiasInput.floatValue)) / 2.0f;
			else yBias = -1 * (Mathf.Abs (leftBiasInput.floatValue) + Mathf.Abs (rightBiasInput.floatValue)) / 2.0f;

			return new Vector2 (xBias, yBias);
		}
	}

	public float moveSpeed;
	public List<GameObject> targets
	{
		get
		{
			List<GameObject> _targets = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Target"));

			_targets.Sort (delegate(GameObject x, GameObject y) {
				Vector3 forward = transform.TransformDirection (Vector3.forward);
				Vector3 n = transform.TransformDirection (Vector3.up);
				Vector3 xPosition = transform.position - x.transform.position;
				Vector3 yPosition = transform.position - y.transform.position;
				float xAngle = MathStuff.FullAngleBetween (forward, xPosition, n);
				float yAngle = MathStuff.FullAngleBetween (forward, yPosition, n);
				return xAngle.CompareTo (yAngle);});

			return _targets;
		}
	}
	private GameObject _target = null;
	public GameObject target
	{
		get
		{
			return _target;
		}
		private set
		{
			Vector3 forward = transform.TransformDirection (Vector3.forward);
			Vector3 n = transform.TransformDirection (Vector3.up);
			Vector3 xPosition = transform.position - value.transform.position;
			float xAngle = MathStuff.FullAngleBetween (forward, xPosition, n);
			Debug.Log (xAngle);

			_target = value;

			if (_target != null) 
			{
				transform.LookAt (_target.transform.position);
			}
		}
	}
	public void ChangeTarget (bool nextTarget)
	{
		if(targets.Count > 0)
		{
			if(target != null && targets.Count > 1) target = nextTarget ? targets[1] : targets[targets.Count - 1];
			else target = targets [0];
		}
	}

	void FixedUpdate ()
	{
		if(target != null) 
		{
			transform.RotateAround (target.transform.position, target.transform.TransformDirection (Vector3.up), inputDir.x * moveSpeed * Time.fixedDeltaTime);
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position, inputDir.y * moveSpeed * Time.fixedDeltaTime);
		}
		else 
		{
			transform.Rotate (transform.TransformDirection (Vector3.up), inputDir.x * moveSpeed * Time.fixedDeltaTime);
			transform.position += transform.TransformDirection (Vector3.forward) * inputDir.y * moveSpeed * Time.fixedDeltaTime;
		}
	}
}