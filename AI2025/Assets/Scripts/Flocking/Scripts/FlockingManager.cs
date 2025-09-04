using System.Collections.Generic;
using UnityEngine;

namespace Flocking.Scripts
{
	public class FlockingManager : MonoBehaviour
	{
		public Transform target;
		public int boidCount = 50;
		public Boid boidPrefab;
		private readonly List<Boid> boids = new();

		private void Start()
		{
			for (int i = 0; i < boidCount; i++)
			{
				GameObject boidGo = Instantiate(boidPrefab.gameObject, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10)), Quaternion.identity);
				Boid boid = boidGo.GetComponent<Boid>();
				boid.Init(Alignment, Cohesion, Separation, Direction);
				boids.Add(boid);
			}
		}

		private Vector2 Alignment(Boid boid)
		{
			List<Boid> neighbors = GetBoidsInsideRadius(boid);
			if (neighbors.Count == 0)
				return Vector2.zero;

			Vector2 avg = Vector2.zero;
			foreach (Boid b in neighbors)
			{
				avg += (Vector2)b.transform.up;
			}

			return (avg / neighbors.Count).normalized;
		}

		private Vector2 Cohesion(Boid boid)
		{
			List<Boid> neighbors = GetBoidsInsideRadius(boid);
			if (neighbors.Count == 0)
				return Vector2.zero;

			Vector2 avg = Vector2.zero;
			foreach (Boid b in neighbors)
			{
				avg += (Vector2)b.transform.position;
			}

			avg /= neighbors.Count;
			return (avg - (Vector2)boid.transform.position).normalized;
		}

		private Vector2 Separation(Boid boid)
		{
			List<Boid> neighbors = GetBoidsInsideRadius(boid);
			if (neighbors.Count == 0)
				return Vector2.zero;

			Vector2 avg = Vector2.zero;
			foreach (Boid b in neighbors)
			{
				avg += ((Vector2)boid.transform.position - (Vector2)b.transform.position);
			}

			return (avg / neighbors.Count).normalized;
		}

		private Vector2 Direction(Boid boid)
		{
			return ((Vector2)target.position - (Vector2)boid.transform.position).normalized;
		}

		private List<Boid> GetBoidsInsideRadius(Boid boid)
		{
			List<Boid> insideRadiusBoids = new();

			foreach (Boid b in boids)
			{
				if (Vector2.Distance(boid.transform.position, b.transform.position) < boid.DetectionRadius)
				{
					insideRadiusBoids.Add(b);
				}
			}

			return insideRadiusBoids;
		}
	}
}