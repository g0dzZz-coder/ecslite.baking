// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using Leopotam.EcsLite.Baking.Runtime.Entities;
using UnityEngine;

namespace Leopotam.EcsLite.Baking.Runtime.Components
{
	[RequireComponent(typeof(AuthoringEntity))]
	public abstract class AuthoringComponent<TComponent> : MonoBehaviour, IAuthoring where TComponent : struct
	{
		[SerializeField] private TComponent _value;

		public virtual IBaker CreateBaker(EcsPackedEntityWithWorld entity) => new Baker(entity);

		private readonly struct Baker : IBaker
		{
			private readonly EcsPackedEntityWithWorld _entity;

			public Baker(EcsPackedEntityWithWorld entity) => _entity = entity;

			void IBaker.Bake(IAuthoring authoring)
			{
				if (_entity.Unpack(out var world, out var entity))
				{
					world.GetPool<TComponent>().Replace(entity, ((AuthoringComponent<TComponent>) authoring)._value);
				}
			}
		}
	}
}