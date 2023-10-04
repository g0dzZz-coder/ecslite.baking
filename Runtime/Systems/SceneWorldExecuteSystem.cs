using Leopotam.EcsLite.Baking.Runtime.Internal;

namespace Leopotam.EcsLite.Baking.Runtime.Systems
{
#if ENABLE_IL2CPP
	using Unity.IL2CPP.CompilerServices;

	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
	public sealed class SceneWorldExecuteSystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsWorld _world;
		private EcsFilter _entities;
		private EcsPool<ConvertibleGameObject> _convertibles;

		void IEcsPreInitSystem.PreInit(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_entities = _world.Filter<ConvertibleGameObject>().End();
			_convertibles = _world.GetPool<ConvertibleGameObject>();
		}

		void IEcsRunSystem.Run(IEcsSystems systems)
		{
			foreach (var entity in _entities)
			{
				ref var convertible = ref _convertibles.Get(entity);

				if (convertible.GameObject)
				{
					SceneEntity.TryConvert(convertible.GameObject, systems, convertible.WorldName);
				}

				_world.DelEntity(entity);
			}
		}
	}
}