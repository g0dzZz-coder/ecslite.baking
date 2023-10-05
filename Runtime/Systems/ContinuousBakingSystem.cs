// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using Leopotam.EcsLite.Baking.Runtime.Entities;
using Leopotam.EcsLite.Baking.Runtime.Internal;

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
		private EcsPool<ConvertibleEntityRef> _convertibles;

		void IEcsPreInitSystem.PreInit(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_entities = _world.Filter<ConvertibleEntityRef>().End();
			_convertibles = _world.GetPool<ConvertibleEntityRef>();
		}

		void IEcsRunSystem.Run(IEcsSystems systems)
		{
			foreach (var entity in _entities)
			{
				ref var convertible = ref _convertibles.Get(entity);
				if (convertible.GameObject && convertible.GameObject.TryGetComponent(out AuthoringEntity authoring))
				{
					BakingUtility.Bake(authoring, systems, convertible.WorldName);
				}

				_world.DelEntity(entity);
			}
		}
	}
}