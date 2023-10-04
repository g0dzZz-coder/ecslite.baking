using Leopotam.EcsLite.Baking.Runtime.Internal;

namespace Leopotam.EcsLite.Baking.Runtime.Systems
{
	public readonly struct SceneWorldDestroySystem : IEcsDestroySystem
	{
		void IEcsDestroySystem.Destroy(IEcsSystems systems) => SceneWorld.Destroy();
	}
}