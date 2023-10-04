// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using Leopotam.EcsLite.Baking.Runtime.Internal;
using UnityEngine;

namespace Leopotam.EcsLite.Baking.Runtime.Entities
{
	[DisallowMultipleComponent]
	public sealed class ConvertibleEntity : MonoBehaviour
	{
		[SerializeField] internal ConvertMode _mode;
		[SerializeField] internal string _customWorld;

		private bool _isProcessed;
		private EcsPackedEntityWithWorld _entity;

		private void Start()
		{
			var world = SceneWorld.World;
			if (world == null || _isProcessed)
			{
				return;
			}

			var entity = world.NewEntity();
			ref var convertibleReference = ref world.GetPool<ConvertibleGameObject>().Add(entity);
			convertibleReference.GameObject = gameObject;
			convertibleReference.WorldName = _customWorld;
		}

		public int? TryGetEntity() => _entity.Unpack(out _, out var entity) ? entity : null;

		internal void Initialize(EcsPackedEntityWithWorld entity) => _entity = entity;

		internal void MarkAsProcessed() => _isProcessed = true;
	}
}