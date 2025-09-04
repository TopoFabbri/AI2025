using ECS.Patron;

namespace ECS.Implementation
{
    public class ScaleComponent : EcsComponent
    {
        public float x;
        public float y;
        public float z;

        public ScaleComponent(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}