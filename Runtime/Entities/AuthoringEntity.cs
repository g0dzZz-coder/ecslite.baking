// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using Leopotam.EcsLite.Baking.Runtime.Services;
using UnityEngine;

namespace Leopotam.EcsLite.Baking.Runtime.Entities
{
	[DisallowMultipleComponent]
	public sealed class AuthoringEntity : MonoBehaviour
	{
		[SerializeField] internal ConversionMode _mode;
		[SerializeField] internal string _customWorld;

		private bool _processed;
		private EcsPackedEntityWithWorld _entity;

		private void OnEnable()
		{
			var world = BakingWorld.World;
			if (world == null || _processed)
			{
				return;
			}

			ref var bakingEntity = ref world.GetPool<BakingEntityRef>().Add(world.NewEntity());
			bakingEntity.GameObject = gameObject;
			bakingEntity.WorldName = _customWorld;
		}

		public int? TryGetEntity() => _entity.Unpack(out _, out var entity) ? entity : null;

		internal void Initialize(EcsPackedEntityWithWorld entity) => _entity = entity;

		internal void MarkAsProcessed() => _processed = true;
	}
}