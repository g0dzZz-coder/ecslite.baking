// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using Leopotam.EcsLite.Baking.Runtime.Entities;
using UnityEngine;

namespace Leopotam.EcsLite.Baking.Runtime.Systems
{
	public readonly struct InitialBakingSystem : IEcsPreInitSystem
	{
		void IEcsPreInitSystem.PreInit(IEcsSystems systems)
		{
			foreach (var authoringEntity in Object.FindObjectsOfType<AuthoringEntity>())
			{
				new AuthoringEntityBaker(authoringEntity).Bake(systems, authoringEntity._customWorld);
			}
		}
	}
}