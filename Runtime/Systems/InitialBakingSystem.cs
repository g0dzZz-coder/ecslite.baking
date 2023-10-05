// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using Leopotam.EcsLite.Baking.Runtime.Entities;
using Leopotam.EcsLite.Baking.Runtime.Internal;
using UnityEngine;

namespace Leopotam.EcsLite.Baking.Runtime.Systems
{
	public readonly struct InitialBakingSystem : IEcsPreInitSystem
	{
		void IEcsPreInitSystem.PreInit(IEcsSystems systems)
		{
			foreach (var authoringEntity in Object.FindObjectsOfType<AuthoringEntity>())
			{
				BakingUtility.Bake(authoringEntity, systems, authoringEntity._customWorld);
			}
		}
	}
}