using UnityEngine;
using System.Collections;

public class Enviroment : MonoBehaviour
{
	[System.Serializable]public enum Type
	{
		Static,
		Breakable,
		Mobile
	}
	public Type type;


}