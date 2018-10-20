using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public static int score;

	#region Road
	public Queue<GameObject> road = new Queue<GameObject>();
	private const float distanceRoad = 60f;
	#endregion

	#region Enemy
	public GameObject prefab;
	public Queue<EnemyController> enemys = new Queue<EnemyController>();
	private int countEnemys = 20;
	private float line = 2;
	private float distance = 2;
	#endregion

	private void Awake()
	{
		road.Enqueue(GameObject.Find("Road0"));
		road.Enqueue(GameObject.Find("Road1"));
	}

	private void Start()
	{
		for (int i = 0; i < countEnemys; i++)
		{
			GameObject obj;
			if (line % 2 == 0)
			{
				obj = Instantiate(prefab, new Vector3(Random.Range(12, 20), transform.position.y, transform.position.z + distance), Quaternion.identity);
				obj.GetComponent<EnemyController>().onRight = true;
				line++;
			}
			else
			{
				obj = Instantiate(prefab, new Vector3(Random.Range(-12, -20), transform.position.y, transform.position.z + distance), Quaternion.identity);
				obj.GetComponent<EnemyController>().onRight = false;
				line++;
			}
			distance += 2;
			enemys.Enqueue(obj.GetComponent<EnemyController>());
		}
	}

	private void Update()
	{
		if (!road.Peek().GetComponent<Renderer>().IsVisibleFrom(Camera.main))
		{
			ResetRoad(road.Dequeue());
		}

		#region Enemy
		if (!enemys.Peek().GetComponent<Renderer>().IsVisibleFrom(Camera.main) && Camera.main.transform.position.z > enemys.Peek().transform.position.z + 5f)
		{
			ResetObject(enemys.Dequeue());
		}
		foreach (EnemyController enemy in enemys)
		{
			if (enemy.GetComponent<EnemyController>().onRight == true)
			{
				enemy.transform.position = new Vector3(enemy.transform.position.x + enemy.GetComponent<EnemyController>().speed * Time.deltaTime, enemy.transform.position.y, enemy.transform.position.z);
				if (enemy.transform.position.x < -12)
				{
					ResetLineObject(enemy);
				}
			}
			else
			{
				enemy.transform.position = new Vector3(enemy.transform.position.x - enemy.GetComponent<EnemyController>().speed * Time.deltaTime, enemy.transform.position.y, enemy.transform.position.z);
				if (enemy.transform.position.x > 12)
				{
					ResetLineObject(enemy);
				}
			}
		}
		#endregion
	}

	#region Enemy
	private void ResetObject(EnemyController obj)
	{
		if (obj.GetComponent<EnemyController>().onRight == true)
		{
			obj.transform.position = new Vector3(Random.Range(12, 20), obj.transform.position.y, obj.transform.position.z + distance);
		}
		else
		{
			obj.transform.position = new Vector3(Random.Range(-12, -20), obj.transform.position.y, obj.transform.position.z + distance);
		}
		obj.GetComponent<EnemyController>().speed -= 5f;
		enemys.Enqueue(obj);
		line++;
		distance += 2;
	}

	private void ResetLineObject(EnemyController obj)
	{
		if (obj.GetComponent<EnemyController>().onRight == true)
		{
			obj.transform.position = new Vector3(Random.Range(12, 20), obj.transform.position.y, obj.transform.position.z);
		}
		else
		{
			obj.transform.position = new Vector3(Random.Range(-12, -20), obj.transform.position.y, obj.transform.position.z);
		}
	}
	#endregion

	private void ResetRoad(GameObject obj)
	{
		obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z + distanceRoad);
		road.Enqueue(obj);
	}
}
