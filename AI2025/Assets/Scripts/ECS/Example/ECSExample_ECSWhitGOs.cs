using System.Collections.Generic;
using ECS.Implementation;
using ECS.Patron;
using UnityEngine;

public class ECSExample_ECSWhitGOs : MonoBehaviour
{
    public int entityCount = 100;
    public float velocity = 10.0f;
    public GameObject prefab;

    private Dictionary<uint, GameObject> entities;

    void Start()
    {
        EcsManager.Init();
        entities = new Dictionary<uint, GameObject>();
        for (int i = 0; i < entityCount; i++)
        {
            uint entityID = EcsManager.CreateEntity();
            EcsManager.AddComponent<PositionComponent>(entityID,
                new PositionComponent(0, -i, 0));
            EcsManager.AddComponent<VelocityComponent>(entityID,
                new VelocityComponent(velocity, Vector3.right.x, Vector3.right.y, Vector3.right.z));
            entities.Add(entityID, Instantiate(prefab, new Vector3(0, -i, 0), Quaternion.identity));
        }
    }

    void Update()
    {
        EcsManager.Tick(Time.deltaTime);
    }

    void LateUpdate()
    {
        foreach (KeyValuePair<uint, GameObject> entity in entities)
        {
            PositionComponent position = EcsManager.GetComponent<PositionComponent>(entity.Key);
            entity.Value.transform.SetPositionAndRotation(new Vector3(position.x, position.y, position.z), Quaternion.identity);
        }
    }
}
