using Leopotam.EcsLite.Baking.Runtime.Internal;

namespace Leopotam.EcsLite.Baking.Runtime.Systems
{
	public readonly struct BakingServiceSystem : IEcsPreInitSystem, IEcsDestroySystem
	{
		void IEcsPreInitSystem.PreInit(IEcsSystems systems) =>
			BakingWorld.Initialize(systems.GetWorld());

		void IEcsDestroySystem.Destroy(IEcsSystems systems) =>
			BakingWorld.Dispose();
	}
}