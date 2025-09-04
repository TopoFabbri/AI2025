namespace ECS.Patron
{
	public abstract class EcsComponent
	{
		public uint EntityOwnerID { get; set; } = 0;

		protected EcsComponent() { }

		public virtual void Dispose() { }
	}
}
