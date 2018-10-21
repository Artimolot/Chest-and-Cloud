using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public static int score;

	#region Road
	private Queue<GameObject> road = new Queue<GameObject>();
	private const float distanceRoad = 60f;
	#endregion

	#region Enemy
	public GameObject enemyPrefab;
	public GameObject enemyColonPrefab;
	public GameObject Player;
	private Queue<EnemyController> enemyPool = new Queue<EnemyController>();
	private float countEnemyInGame = 15;
	private float directionEnemy = 1;
	private float distanceBetweEnemy = 2;
	private const float xPosMinEnemy = 11;
	private const float xPosMaxEnemy = 20;
	public bool newSpawn = true;
	#endregion

	private void Awake()
	{
		road.Enqueue(GameObject.Find("Road0"));
		road.Enqueue(GameObject.Find("Road1"));
	}

	private void Start()
	{
		StartCoroutine(SpawnEnemy());
	}

	private void Update()
	{
		#region Road
		if (!road.Peek().GetComponent<Renderer>().IsVisibleFrom(Camera.main))
		{
			ResetRoad(road.Dequeue());
		}
		#endregion

		#region Enemy
		if(countEnemyInGame == 0 && newSpawn == true)
		{
			if (!enemyPool.Peek().GetComponent<Renderer>().IsVisibleFrom(Camera.main) && Camera.main.transform.position.z > enemyPool.Peek().transform.position.z + 5f)
			{
				StartCoroutine(NewPositionEnemy(enemyPool.Dequeue()));
			}
		}
		foreach (EnemyController enemy in enemyPool)
		{
			if (enemy.GetComponent<EnemyController>().onRight == true)
			{
				enemy.transform.position = new Vector3(enemy.transform.position.x - enemy.GetComponent<EnemyController>().speed * Time.deltaTime, enemy.transform.position.y, enemy.transform.position.z);
				if (enemy.transform.position.x > 12)
				{
					ResetLineEnemy(enemy);
				}
			}
			else
			{
				enemy.transform.position = new Vector3(enemy.transform.position.x + enemy.GetComponent<EnemyController>().speed * Time.deltaTime, enemy.transform.position.y, enemy.transform.position.z);
				if (enemy.transform.position.x < -12)
				{
					ResetLineEnemy(enemy);
				}
			}
		}
		#endregion
	}

	private void ResetRoad(GameObject obj)
	{
		obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z + distanceRoad);
		road.Enqueue(obj);
	}

	private void StartSpawnEnemy()
	{
		if (countEnemyInGame > 0)
		{
			GameObject obj;
			if (directionEnemy % 2 == 0)
			{
				obj = Instantiate(enemyPrefab, new Vector3(Random.Range(-xPosMinEnemy, -xPosMaxEnemy), transform.position.y, Player.transform.position.z + distanceBetweEnemy), Quaternion.identity);
				obj.GetComponent<EnemyController>().onRight = true;
			}
			else if(countEnemyInGame == 8)
			{
				obj = Instantiate(enemyColonPrefab, new Vector3(Random.Range(-xPosMinEnemy, -xPosMaxEnemy), transform.position.y, Player.transform.position.z + distanceBetweEnemy), Quaternion.identity);
				obj.GetComponent<EnemyController>().onRight = true;
			}
			else if(countEnemyInGame == 4)
			{
				obj = Instantiate(enemyColonPrefab, new Vector3(Random.Range(xPosMinEnemy, xPosMaxEnemy), transform.position.y, Player.transform.position.z + distanceBetweEnemy), Quaternion.identity);
				obj.GetComponent<EnemyController>().onRight = false;
			}
			else
			{
				obj = Instantiate(enemyPrefab, new Vector3(Random.Range(xPosMinEnemy, xPosMaxEnemy), transform.position.y, Player.transform.position.z + distanceBetweEnemy), Quaternion.identity);
				obj.GetComponent<EnemyController>().onRight = false;
			}
			enemyPool.Enqueue(obj.GetComponent<EnemyController>());
			distanceBetweEnemy += 2;
			directionEnemy++;
			countEnemyInGame--;
			StartCoroutine(SpawnEnemy());
		}
	}

	private void ResetLineEnemy(EnemyController obj)
	{
		if (obj.GetComponent<EnemyController>().onRight)
		{
			obj.transform.position = new Vector3(Random.Range(-xPosMinEnemy, -xPosMaxEnemy), obj.transform.position.y, obj.transform.position.z);
		}
		else
		{
			obj.transform.position = new Vector3(Random.Range(xPosMinEnemy, xPosMaxEnemy), obj.transform.position.y, obj.transform.position.z);
		}
	}

	IEnumerator NewPositionEnemy(EnemyController obj)
	{
		float time = 1.0f;
		newSpawn = false;
		while (time > 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		obj.transform.position = new Vector3(Random.Range(12, 20), obj.transform.position.y, Player.transform.position.z + distanceBetweEnemy);
		obj.GetComponent<EnemyController>().speed -= 3f;
		newSpawn = true;
		enemyPool.Enqueue(obj);
		distanceBetweEnemy += 2;
		yield return null;
	}

	IEnumerator SpawnEnemy()
	{
		float time = 1.5f;
		while(time > 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		StartSpawnEnemy();
	}
}
