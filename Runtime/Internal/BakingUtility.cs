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
	internal static class BakingUtility
	{
		public static void Bake(AuthoringEntity authoringEntity, IEcsSystems systems, string worldName)
		{
			var world = systems.GetWorld(worldName == string.Empty ? null : worldName);
			var entity = world.NewEntity();
			var packedEntity = world.PackEntityWithWorld(entity);

			foreach (var authoring in authoringEntity.GetComponents<IAuthoring>())
			{
				authoring.CreateBaker(packedEntity).Bake(authoring);
				Object.Destroy((Component) authoring);
			}

			authoringEntity.MarkAsProcessed();
			FinalizeConversion(authoringEntity, packedEntity);
		}

		private static void FinalizeConversion(AuthoringEntity authoring, EcsPackedEntityWithWorld entity)
		{
			switch (authoring._mode)
			{
				case ConversionMode.CONVERT_AND_DESTROY:
					Object.Destroy(authoring.gameObject);
					break;
				case ConversionMode.CONVERT_AND_INJECT:
					Object.Destroy(authoring);
					break;
				case ConversionMode.CONVERT_AND_SAVE:
					authoring.Initialize(entity);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}