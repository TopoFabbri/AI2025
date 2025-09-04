using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECS.Patron
{
    public static class EcsManager
    {
        private static readonly ParallelOptions ParallelOptions = new() { MaxDegreeOfParallelism = 32 };

        private static ConcurrentDictionary<uint, EcsEntity> _entities;
        private static ConcurrentDictionary<Type, ConcurrentDictionary<uint, EcsComponent>> _components;
        private static ConcurrentDictionary<Type, ECSSystem> _systems;

        public static void Init()
        {
            _entities = new ConcurrentDictionary<uint, EcsEntity>();
            _components = new ConcurrentDictionary<Type, ConcurrentDictionary<uint, EcsComponent>>();
            _systems = new ConcurrentDictionary<Type, ECSSystem>();

            foreach (Type classType in typeof(ECSSystem).Assembly.GetTypes())
            {
                if (typeof(ECSSystem).IsAssignableFrom(classType) && !classType.IsAbstract)
                {
                    _systems.TryAdd(classType, Activator.CreateInstance(classType) as ECSSystem);
                }
            }

            foreach (KeyValuePair<Type, ECSSystem> system in _systems)
            {
                system.Value.Initialize();
            }

            foreach (Type classType in typeof(EcsComponent).Assembly.GetTypes())
            {
                if (typeof(EcsComponent).IsAssignableFrom(classType) && !classType.IsAbstract)
                {
                    _components.TryAdd(classType, new ConcurrentDictionary<uint, EcsComponent>());
                }
            }
        }

        public static void Tick(float deltaTime)
        {
            Parallel.ForEach(_systems, ParallelOptions, system =>
            {
                system.Value.Run(deltaTime);
            });
        }

        public static uint CreateEntity()
        {
            EcsEntity ecsEntity = new();
            
            _entities.TryAdd(ecsEntity.GetID(), ecsEntity);
            return ecsEntity.GetID();
        }

        public static void AddComponent<TComponent>(uint entityID, TComponent component) where TComponent : EcsComponent
        {
            component.EntityOwnerID = entityID;
            _entities[entityID].AddComponentType(typeof(TComponent));
            _components[typeof(TComponent)].TryAdd(entityID, component);
        }

        public static bool ContainsComponent<TComponent>(uint entityID) where TComponent : EcsComponent
        {
            return _entities[entityID].ContainsComponentType<TComponent>();
        }

        public static IEnumerable<uint> GetEntitiesWhitComponentTypes(params Type[] componentTypes)
        {
            ConcurrentBag<uint> matchs = new ConcurrentBag<uint>();
            Parallel.ForEach(_entities, ParallelOptions, entity =>
            {
                for (int i = 0; i < componentTypes.Length; i++)
                {
                    if (!entity.Value.ContainsComponentType(componentTypes[i]))
                        return;
                }
                matchs.Add(entity.Key);
            });
            return matchs;
        }

        public static ConcurrentDictionary<uint, TComponent> GetComponents<TComponent>() where TComponent : EcsComponent
        {
            if (!_components.ContainsKey(typeof(TComponent))) return null;
            
            ConcurrentDictionary<uint, TComponent> comps = new();

            Parallel.ForEach(_components[typeof(TComponent)], ParallelOptions, component => 
            { 
                comps.TryAdd(component.Key, component.Value as TComponent);
            });

            return comps;

        }

        public static TComponent GetComponent<TComponent>(uint entityID) where TComponent : EcsComponent 
        {
            return _components[typeof(TComponent)][entityID] as TComponent;
        }
    }
}
