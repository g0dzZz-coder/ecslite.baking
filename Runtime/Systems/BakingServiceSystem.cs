// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using Leopotam.EcsLite.Baking.Runtime.Services;

namespace Leopotam.EcsLite.Baking.Runtime.Systems
{
	public readonly struct BakingServiceSystem : IEcsPreInitSystem, IEcsDestroySystem
	{
		void IEcsPreInitSystem.PreInit(IEcsSystems systems) =>
			BakingWorld.Initialize(systems.GetWorld());

		void IEcsDestroySystem.Destroy(IEcsSystems systems) =>
			BakingWorld.Dispose();
	}
}