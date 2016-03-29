using UnityEngine;
using System.Collections;

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

	void Update ()
	{Debug.Log (inputDir);
		//transform.position += new Vector3(inputDir.x, 0.0f, inputDir.y) * moveSpeed;
	}
}