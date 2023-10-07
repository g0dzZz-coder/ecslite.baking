// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Leopotam.EcsLite.Baking.Runtime.Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Leopotam.EcsLite.Baking.Runtime.Entities
{
#if ENABLE_IL2CPP
	using Unity.IL2CPP.CompilerServices;

	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
	internal readonly ref struct AuthoringEntityBaker
	{
		private readonly AuthoringEntity _authoringEntity;

		public AuthoringEntityBaker(AuthoringEntity authoringEntity) =>
			_authoringEntity = authoringEntity;

		public void Bake(IEcsSystems systems, string worldName)
		{
			var world = systems.GetWorld(worldName == string.Empty ? null : worldName);
			var entity = world.NewEntity();
			var packedEntity = world.PackEntityWithWorld(entity);

			foreach (var authoring in _authoringEntity.GetComponents<IAuthoring>())
			{
				authoring.CreateBaker(packedEntity).Bake(authoring);
				Object.Destroy((Component) authoring);
			}

			_authoringEntity.MarkAsProcessed();
			FinalizeConversion(packedEntity);
		}

		private void FinalizeConversion(EcsPackedEntityWithWorld entity)
		{
			switch (_authoringEntity._mode)
			{
				case ConversionMode.CONVERT_AND_DESTROY:
					Object.Destroy(_authoringEntity.gameObject);
					break;
				case ConversionMode.CONVERT_AND_INJECT:
					Object.Destroy(_authoringEntity);
					break;
				case ConversionMode.CONVERT_AND_SAVE:
					_authoringEntity.Initialize(entity);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}