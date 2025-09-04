using System.Collections.Generic;
using System.Threading.Tasks;
using ECS.Patron;
using Flocking.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ECS.Example
{
    public class EcsFlockingExample : MonoBehaviour
    {
        [SerializeField] private int entityCount = 50;
        [SerializeField] private Transform target;
        [SerializeField] private GameObject drawPrefab;
        [SerializeField] private bool useGameObjects;
        
        private readonly List<uint> entities = new();
        private readonly List<GameObject> instances = new();
        
        private Mesh prefabMesh;
        private Material prefabMaterial;

        private const int MaxObjectsPerDrawcall = 1000;

        private void Start()
        {
            prefabMesh = drawPrefab.GetComponent<MeshFilter>().sharedMesh;
            prefabMaterial = drawPrefab.GetComponent<MeshRenderer>().sharedMaterial;

            EcsManager.Init();

            for (int i = 0; i < entityCount; i++)
            {
                uint entityId = EcsManager.CreateEntity();
                InitBoidEntity(entityId);
                entities.Add(entityId);

                if (useGameObjects)
                {
                    GameObject go = Instantiate(drawPrefab, transform, true);
                    instances.Add(go);
                }
            }
        }

        private void Update()
        {
            EcsManager.Tick(Time.deltaTime);
        }

        private void LateUpdate()
        {
            List<Matrix4x4[]> drawMatrices = new();
            int meshes = entities.Count;

            for (int i = 0; i < entities.Count; i += MaxObjectsPerDrawcall)
            {
                drawMatrices.Add(new Matrix4x4[meshes > MaxObjectsPerDrawcall ? MaxObjectsPerDrawcall : meshes]);
                meshes -= MaxObjectsPerDrawcall;
            }

            if (useGameObjects)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    BoidComponent boid = EcsManager.GetComponent<BoidComponent>(entities[i]);
                
                    Vector3 position = new(boid.upX, boid.upY, boid.upZ);
                    Quaternion rotation = Quaternion.LookRotation(position, Vector3.forward);

                    instances[i].transform.SetPositionAndRotation(position, rotation);
                }
            }
            else
            {
                Parallel.For(0, entities.Count, i =>
                {
                    BoidComponent boid = EcsManager.GetComponent<BoidComponent>(entities[i]);
                
                    Vector3 position = new(boid.upX, boid.upY, boid.upZ);
                    Quaternion rotation = Quaternion.LookRotation(position, Vector3.forward);
                    Vector3 scale = Vector3.one;
                    
                    drawMatrices[i / MaxObjectsPerDrawcall][i % MaxObjectsPerDrawcall].SetTRS(position, rotation, scale);
                });
            }

            if (useGameObjects) return;
            
            foreach (Matrix4x4[] drawMatrix in drawMatrices)
                Graphics.DrawMeshInstanced(prefabMesh, 0, prefabMaterial, drawMatrix);
        }

        private static void InitBoidEntity(uint entityId)
        {
            float x = Random.Range(-10f, 10f);
            float y = Random.Range(-10f, 10f);
            const float z = 0f;
            const float upX = 0f;
            const float upY = 1f;
            const float upZ = 0f;
            const float speed = 2.5f;
            const float turnSpeed = 5f;
            const float detectionRadius = 3.0f;
            const float alignmentFactor = 1.0f;
            const float cohesionFactor = .5f;
            const float separationFactor = 1.5f;
            const float directionFactor = 1.0f;

            EcsManager.AddComponent(entityId,
                new BoidComponent(speed, turnSpeed, detectionRadius, alignmentFactor, cohesionFactor, separationFactor, directionFactor, x, y, z, upX, upY, upZ));
        }
    }
}