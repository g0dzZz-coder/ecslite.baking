// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using Leopotam.EcsLite.Baking.Runtime.Entities;
using UnityEngine;

namespace Leopotam.EcsLite.Baking.Runtime.Components
{
	[RequireComponent(typeof(ConvertibleEntity))]
	public abstract class ComponentBaker : MonoBehaviour
	{
		internal abstract void Bake(int entity, EcsWorld world);
	}
}