using Leopotam.EcsLite.Baking.Runtime.Entities;
using Leopotam.EcsLite.Baking.Runtime.Internal;
using UnityEngine;

namespace Leopotam.EcsLite.Baking.Runtime.Systems
{
	public readonly struct SceneWorldInitSystem : IEcsPreInitSystem
	{
		void IEcsPreInitSystem.PreInit(IEcsSystems systems)
		{
			foreach (var convertible in Object.FindObjectsOfType<ConvertibleEntity>())
			{
				SceneEntity.TryConvert(convertible.gameObject, systems, convertible._customWorld);
			}

			SceneWorld.Initialize(systems.GetWorld());
		}
	}
}