using System;
using System.Collections.Generic;

namespace ECS.Patron
{
    public class EcsEntity
    {
        private static class EntityID
        {
            private static uint _lastEntityID;
            internal static uint GetNew() => _lastEntityID++;
        }

        private readonly uint id;
        private readonly List<Type> componentsType;

        public EcsEntity()
        {
            id = EntityID.GetNew();
            componentsType = new List<Type>();
        }

        public uint GetID() => id;

        public void Dispose()
        {
            componentsType.Clear();
        }

        public void AddComponentType<TComponentType>() where TComponentType : EcsComponent
        {
            AddComponentType(typeof(TComponentType));
        }

        public void AddComponentType(Type ComponentType)
        {
            componentsType.Add(ComponentType);
        }

        public bool ContainsComponentType<ComponentType>() where ComponentType : EcsComponent
        {
            return ContainsComponentType(typeof(ComponentType));
        }

        public bool ContainsComponentType(Type ComponentType)
        {
            return componentsType.Contains(ComponentType);
        }
    }
}
