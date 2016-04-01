using UnityEngine;
using System.Collections;

public static class MathStuff
{
	public static float SignedAngleBetween (Vector3 a, Vector3 b, Vector3 n)
	{
		float angle = Vector3.Angle (a, b);
		float sign = Mathf.Sign (Vector3.Dot (n, Vector3.Cross (a, b)));

		float signedAngle = angle * sign;

		//float angle360 = (signedAngle + 180) % 360;

		return signedAngle * Mathf.Deg2Rad;
	}

	public static float FullAngleBetween (Vector3 a, Vector3 b, Vector3 n)
	{
		float angle = Vector3.Angle (a, b);
		float sign = Mathf.Sign (Vector3.Dot (n, Vector3.Cross (a, b)));

		float signedAngle = angle * sign;

		float angle360 = (signedAngle + 180) % 360;

		return angle360 * Mathf.Deg2Rad;
	}
}