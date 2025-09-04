using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECS.Patron;
using Flocking.Scripts;
using UnityEngine;

namespace ECS.Implementation
{
    public sealed class FlockingSystem : EcsSystem
    {
        private ParallelOptions parallelOptions;
        private ConcurrentDictionary<uint, BoidComponent> boidComponents;
        private IEnumerable<uint> queriedEntities;

        public override void Initialize()
        {
            parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 32 };
        }

        protected override void PreExecute(float deltaTime)
        {
            boidComponents ??= EcsManager.GetComponents<BoidComponent>();
            queriedEntities ??= EcsManager.GetEntitiesWhitComponentTypes(typeof(BoidComponent));
        }

        protected override void Execute(float deltaTime)
        {
            Parallel.ForEach(queriedEntities, parallelOptions, entityId =>
            {
                BoidComponent boid = boidComponents[entityId];
                ICollection<BoidComponent> neighbors = GetNeighboursInRadius(boid);

                Vector3 alignment = Alignment(neighbors) * boid.alignmentFactor;
                Vector3 cohesion = Cohesion(boid, neighbors) * boid.cohesionFactor;
                Vector3 separation = Separation(boid, neighbors) * boid.separationFactor;
                Vector3 direction = Direction(boid) * boid.directionFactor;

                Vector3 acs = (alignment + cohesion + separation + direction).normalized;

                Vector3 boidPos = new(boid.x, boid.y, boid.z);
                Vector3 boidUp = new(boid.upX, boid.upY, boid.upZ);
                
                boidPos += boidUp * (boid.speed * deltaTime);
                boidUp = Vector3.Lerp(boidUp, acs, boid.turnSpeed * deltaTime).normalized;
                
                boid.x = boidPos.x;
                boid.y = boidPos.y;
                boid.z = boidPos.z;
                
                boid.upX = boidUp.x;
                boid.upY = boidUp.y;
                boid.upZ = boidUp.z;
            });
        }

        protected override void PostExecute(float deltaTime)
        {
        }

        private static Vector3 Alignment(ICollection<BoidComponent> neighbors)
        {
            if (neighbors.Count == 0)
                return Vector3.zero;

            Vector3 avg = Vector3.zero;
            foreach (BoidComponent b in neighbors)
            {
                avg += new Vector3(b.x, b.y, b.z);
            }

            return (avg / neighbors.Count).normalized;
        }

        private static Vector3 Cohesion(BoidComponent boid, ICollection<BoidComponent> neighbors)
        {
            if (neighbors.Count == 0)
                return Vector3.zero;

            Vector3 avg = Vector3.zero;

            foreach (BoidComponent b in neighbors)
                avg += new Vector3(b.x, b.y, b.z);

            avg /= neighbors.Count;
            return (avg - new Vector3(boid.x, boid.y, boid.z)).normalized;
        }

        private static Vector3 Separation(BoidComponent boid, ICollection<BoidComponent> neighbors)
        {
            if (neighbors.Count == 0)
                return Vector3.zero;

            Vector3 avg = Vector3.zero;

            foreach (BoidComponent b in neighbors)
                avg += (new Vector3(boid.x, boid.y, boid.z) - new Vector3(b.x, b.y, b.z));

            return (avg / neighbors.Count).normalized;
        }

        private static Vector3 Direction(BoidComponent boid)
        {
            return (new Vector3(boid.targetX, boid.targetY, boid.targetZ) - new Vector3(boid.x, boid.y, boid.z)).normalized;
        }

        private ICollection<BoidComponent> GetNeighboursInRadius(BoidComponent boid)
        {
            List<BoidComponent> neighboursInRadius = new();

            foreach (BoidComponent boidComponent in boidComponents.Values)
            {
                if (boidComponent == boid) continue;

                float distance = UnityEngine.Vector3.Distance(new UnityEngine.Vector3(boid.x, boid.y, boid.z),
                    new UnityEngine.Vector3(boidComponent.x, boidComponent.y, boidComponent.z));

                if (distance <= boid.detectionRadius)
                    neighboursInRadius.Add(boidComponent);
            }

            return neighboursInRadius;
        }
    }
}