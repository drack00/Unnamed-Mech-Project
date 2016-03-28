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
			if(leftBiasSign != InputBiasSign.Negative)xBias += -1.0f * leftBiasInput.floatValue;
			if(rightBiasSign != InputBiasSign.Negative)xBias += rightBiasInput.floatValue;
			float yBias = ((2.0f * leftBiasInput.floatValue) * (2.0f * rightBiasInput.floatValue)) / 2.0f;

			Vector2 _inputDir = new Vector2 (xBias, yBias);

			return _inputDir;
		}
	}

	void Update ()
	{
		Debug.Log ("final " + inputDir);
	}
}