using System;
using UnityEngine;

namespace Flocking.Scripts
{
    public class Boid : MonoBehaviour
    {
        [SerializeField] private float speed = 2.5f;
        [SerializeField] private float turnSpeed = 5f;
        [SerializeField] private float detectionRadius = 3.0f;

        [SerializeField] private float alignmentFactor;
        [SerializeField] private float cohesionFactor;
        [SerializeField] private float separationFactor;
        [SerializeField] private float directionFactor;

        private Func<Boid, Vector2> alignment;
        private Func<Boid, Vector2> cohesion;
        private Func<Boid, Vector2> separation;
        private Func<Boid, Vector2> direction;

        public float DetectionRadius => detectionRadius;

        public void Init(Func<Boid, Vector2> alignment, Func<Boid, Vector2> cohesion, Func<Boid, Vector2> separation, Func<Boid, Vector2> direction)
        {
            this.alignment = alignment;
            this.cohesion = cohesion;
            this.separation = separation;
            this.direction = direction;
        }

        private void Update()
        {
            transform.position += transform.up * (speed * Time.deltaTime);
            transform.up = Vector3.Lerp(transform.up, Acs(), turnSpeed * Time.deltaTime);
        }

        private Vector2 Acs()
        {
            Vector2 acs = alignment(this) * alignmentFactor + cohesion(this) * cohesionFactor + separation(this) * separationFactor + direction(this) * directionFactor;

            return acs.normalized;
        }
    }
}