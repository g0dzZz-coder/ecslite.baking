// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using System.Runtime.CompilerServices;

namespace Leopotam.EcsLite.Baking.Runtime.Systems
{
	public static class WorldSystemsExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEcsSystems ConvertScene<TSystems>(this TSystems self) where TSystems : IEcsSystems => self
			.Add(new SceneWorldInitSystem())
			.Add(new SceneWorldExecuteSystem())
			.Add(new SceneWorldDestroySystem());
	}
}