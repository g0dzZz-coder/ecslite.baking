// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using Leopotam.EcsLite.Baking.Runtime.Entities;

namespace Leopotam.EcsLite.Baking.Runtime.Systems
{
#if ENABLE_IL2CPP
	using Unity.IL2CPP.CompilerServices;

	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
	public sealed class ContinuousBakingSystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsWorld _world;
		private EcsFilter _entities;
		private EcsPool<BakingEntityRef> _bakingEntities;

		void IEcsPreInitSystem.PreInit(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_entities = _world.Filter<BakingEntityRef>().End();
			_bakingEntities = _world.GetPool<BakingEntityRef>();
		}

		void IEcsRunSystem.Run(IEcsSystems systems)
		{
			foreach (var entity in _entities)
			{
				ref var bakingEntity = ref _bakingEntities.Get(entity);
				if (bakingEntity.GameObject && bakingEntity.GameObject.TryGetComponent(out AuthoringEntity authoring))
				{
					new AuthoringEntityBaker(authoring).Bake(systems, bakingEntity.WorldName);
				}

				_world.DelEntity(entity);
			}
		}
	}
}