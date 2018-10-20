using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public bool onMove = true;
	private void Update()
	{
		if (onMove)
		{
			gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (2f * Time.deltaTime));
		}
	}
}
