using ECS.Patron;

namespace ECS.Implementation
{
    public class PositionComponent : EcsComponent
    {
        public float x;
        public float y;
        public float z;

        public PositionComponent(float x, float y, float z) 
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
