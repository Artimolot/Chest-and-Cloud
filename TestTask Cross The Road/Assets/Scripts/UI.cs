using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	public static UI Instance;

	public Text textScore;
	public GameObject restartGame;
	public GameObject exitGame;

	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
	}

	public void StartGame()
	{
		SceneManager.LoadScene(1);
	}

	public void ExitGame()
	{
		Application.Quit();
	}

}
