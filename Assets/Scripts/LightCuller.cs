using UnityEngine;
using System.Collections;

public class LightCuller : MonoBehaviour 
{
	new Camera camera
	{
		get
		{
			return GetComponent<Camera> ();
		}
	}

	void OnPreCull ()
	{
		foreach(Light light in FindObjectsOfType<Light> ())
		{
			if((camera.cullingMask & light.cullingMask) == 0)light.enabled = false;
		}
	}
	void OnPostRender ()
	{
		foreach(Light light in FindObjectsOfType<Light> ())
		{
			light.enabled = true;
		}
	}
}
