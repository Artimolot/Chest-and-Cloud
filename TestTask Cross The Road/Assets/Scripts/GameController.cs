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
	public EnemyController enemyPrefab;
	public EnemyController enemyColonPrefab;
	private Queue<EnemyController> enemyPool = new Queue<EnemyController>();
	private float countEnemyInGame = 25;
	private float directionEnemy = 1;
	private float distanceBetweEnemy = 2;
	private const float xPosMinEnemy = 15;
	private const float xPosMaxEnemy = 25;
	#endregion

	private void Awake()
	{
		road.Enqueue(GameObject.Find("Road0"));
		road.Enqueue(GameObject.Find("Road1"));
	}

	private void Start()
	{
		SpawnEnemy();
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
		if (!enemyPool.Peek().GetComponent<Renderer>().IsVisibleFrom(Camera.main) && Camera.main.transform.position.z > enemyPool.Peek().transform.position.z + 5f)
		{
			NewPositionEnemy(enemyPool.Dequeue());
		}
		foreach (EnemyController enemy in enemyPool)
		{

			if (enemy.onRight == true)
			{
				if (enemy.moveTime <= 0f)
				{
					enemy.transform.position = new Vector3(enemy.transform.position.x - enemy.speed * Time.deltaTime, enemy.transform.position.y, enemy.transform.position.z);
					if (enemy.transform.position.x > 15)
					{
						ResetLineEnemy(enemy);
					}
				}
				else
				{
					enemy.moveTime -= Time.deltaTime;
				}
			}
			else
			{
				if (enemy.moveTime <= 0f)
				{
					enemy.transform.position = new Vector3(enemy.transform.position.x + enemy.speed * Time.deltaTime, enemy.transform.position.y, enemy.transform.position.z);
					if (enemy.transform.position.x < -15)
					{
						ResetLineEnemy(enemy);
					}
				}
				else
				{
					enemy.moveTime -= Time.deltaTime;
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

	private void SpawnEnemy()
	{
		for (int i = 0; i < countEnemyInGame; i++)
		{
			EnemyController obj;
			if (directionEnemy % 2 == 0)
			{
				obj = Instantiate(enemyPrefab, new Vector3(Random.Range(-xPosMinEnemy, -xPosMaxEnemy), transform.position.y, transform.position.z + distanceBetweEnemy), Quaternion.identity);
				obj.onRight = true;
			}
			else if (i % 5 == 0)
			{
				obj = Instantiate(enemyColonPrefab, new Vector3(Random.Range(-xPosMinEnemy, -xPosMaxEnemy), transform.position.y, transform.position.z + distanceBetweEnemy), Quaternion.identity);
				obj.onRight = true;
			}
			else if (i % 9 == 0)
			{
				obj = Instantiate(enemyColonPrefab, new Vector3(Random.Range(xPosMinEnemy, xPosMaxEnemy), transform.position.y, transform.position.z + distanceBetweEnemy), Quaternion.identity);
				obj.onRight = false;
			}
			else
			{
				obj = Instantiate(enemyPrefab, new Vector3(Random.Range(xPosMinEnemy, xPosMaxEnemy), transform.position.y, transform.position.z + distanceBetweEnemy), Quaternion.identity);
				obj.onRight = false;
			}
			obj.moveTime = Random.Range(0.5f, 4.0f);
			enemyPool.Enqueue(obj);
			distanceBetweEnemy += 2;
			directionEnemy++;
		}
	}

	private void ResetLineEnemy(EnemyController obj)
	{
		if (obj.onRight)
		{
			obj.transform.position = new Vector3(Random.Range(-xPosMinEnemy, -xPosMaxEnemy), obj.transform.position.y, obj.transform.position.z);
		}
		else
		{
			obj.transform.position = new Vector3(Random.Range(xPosMinEnemy, xPosMaxEnemy), obj.transform.position.y, obj.transform.position.z);
		}
		obj.moveTime = Random.Range(0.5f, 4.0f);
	}

	private void NewPositionEnemy(EnemyController obj)
	{
		obj.transform.position = new Vector3(Random.Range(xPosMinEnemy, xPosMaxEnemy), obj.transform.position.y, transform.position.z + distanceBetweEnemy);
		obj.speed -= 3f;
		enemyPool.Enqueue(obj);
		distanceBetweEnemy += 2;
		obj.moveTime = Random.Range(0.5f, 4.0f);
	}
}
