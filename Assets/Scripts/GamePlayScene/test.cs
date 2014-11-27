using UnityEngine;
using System.Collections;

public class test : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
				Camera.main.gameObject.AddComponent<CameraFollow> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
