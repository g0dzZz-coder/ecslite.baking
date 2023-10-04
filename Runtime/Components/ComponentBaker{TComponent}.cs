// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using UnityEngine;

namespace Leopotam.EcsLite.Baking.Runtime.Components
{
	public abstract class ComponentBaker<TComponent> : ComponentBaker where TComponent : struct
	{
		[SerializeField] private TComponent _value;

		internal override void Bake(int entity, EcsWorld world)
		{
			var pool = world.GetPool<TComponent>();
			if (pool.Has(entity))
			{
				pool.Del(entity);
			}

			pool.Add(entity) = _value;
		}
	}
}