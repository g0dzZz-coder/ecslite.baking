namespace Leopotam.EcsLite.Baking.Runtime.Components
{
	public interface IAuthoring
	{
		IBaker CreateBaker(EcsPackedEntityWithWorld entity);
	}
}