// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using Leopotam.EcsLite.Baking.Runtime.Internal;

namespace Leopotam.EcsLite.Baking.Runtime.Systems
{
	public readonly struct SceneWorldDestroySystem : IEcsDestroySystem
	{
		void IEcsDestroySystem.Destroy(IEcsSystems systems) => SceneWorld.Destroy();
	}
}