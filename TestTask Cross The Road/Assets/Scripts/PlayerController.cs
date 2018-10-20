using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public GameObject particle;
	public GameObject particleCube;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			MovePlayer();
		}
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.W))
		{
			MovePlayer();
		}
#else
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			if(touch.phase == TouchPhase.Began)
			{
				MovePlayer();
			}
		}
#endif

		if (!gameObject.GetComponent<Renderer>().IsVisibleFrom(Camera.main))
		{
			DiePlayer();
		}
	}

	private void MovePlayer()
	{
		gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
		GameController.score++;
		UI.Instance.textScore.text = "Score:\n" + GameController.score;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			Instantiate(particle, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(-90, 0, 0));
			Instantiate(particleCube, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(-90, 0, 0));
			DiePlayer();
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			Instantiate(particle, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(-90, 0, 0));
			Instantiate(particleCube, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(-90, 0, 0));
			DiePlayer();
		}
	}

	private void DiePlayer()
	{
		GameController.score = 0;
		Camera.main.GetComponent<CameraController>().onMove = false;
		UI.Instance.restartGame.SetActive(true);
		UI.Instance.exitGame.SetActive(true);
		Destroy(this.gameObject);
	}
}
