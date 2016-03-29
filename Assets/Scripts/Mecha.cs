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
			if(leftBiasInput.floatValue > 0.0f)return InputBiasSign.Positive;
			if(leftBiasInput.floatValue < 0.0f)return InputBiasSign.Negative;
			return InputBiasSign.Zero;
		}
	}
	public InputBiasSign rightBiasSign
	{
		get
		{
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
			float yBias = 0.0f;
			if(leftBiasSign != InputBiasSign.Negative)xBias += -1.0f * leftBiasInput.floatValue;
			if(rightBiasSign != InputBiasSign.Negative)xBias += rightBiasInput.floatValue;
			if(leftBiasSign != InputBiasSign.Negative && rightBiasSign != InputBiasSign.Negative)yBias = ((2.0f * leftBiasInput.floatValue) * (2.0f * rightBiasInput.floatValue)) / 2.0f;
			else yBias = -1 * (Mathf.Abs(2.0f * leftBiasInput.floatValue) * Mathf.Abs(2.0f * rightBiasInput.floatValue)) / 2.0f;

			Vector2 _inputDir = new Vector2 (xBias, yBias);

			return _inputDir;
		}
	}

	void Update ()
	{
		Debug.Log ("final " + inputDir);
	}
}