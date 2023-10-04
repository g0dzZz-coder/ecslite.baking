// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Leopotam.EcsLite.Baking.Runtime.Components;
using Leopotam.EcsLite.Baking.Runtime.Entities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Leopotam.EcsLite.Baking.Runtime.Internal
{
#if ENABLE_IL2CPP
	using Unity.IL2CPP.CompilerServices;

	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
	internal static class SceneEntity
	{
		public static void TryConvert(GameObject root, IEcsSystems systems, string worldName)
		{
			if (root.TryGetComponent(out ConvertibleEntity convertible))
			{
				Convert(convertible, systems, worldName);
			}
		}

		private static void Convert(ConvertibleEntity convertible, IEcsSystems systems, string worldName)
		{
			var world = systems.GetWorld(worldName == string.Empty ? null : worldName);
			var entity = world.NewEntity();

			foreach (var baker in convertible.GetComponents<ComponentBaker>())
			{
				baker.Bake(entity, world);
				Object.Destroy(baker);
			}

			convertible.MarkAsProcessed();
			FinalizeConversion(convertible.gameObject, convertible, world.PackEntityWithWorld(entity));
		}

		private static void FinalizeConversion(Object root, ConvertibleEntity convertible,
			EcsPackedEntityWithWorld entity)
		{
			switch (convertible._mode)
			{
				case ConvertMode.CONVERT_AND_DESTROY:
					Object.Destroy(root);
					break;
				case ConvertMode.CONVERT_AND_INJECT:
					Object.Destroy(convertible);
					break;
				case ConvertMode.CONVERT_AND_SAVE:
					convertible.Initialize(entity);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}