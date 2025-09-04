using ECS.Patron;

namespace ECS.Implementation
{
    public class RotationComponent : EcsComponent
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public RotationComponent(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }
}