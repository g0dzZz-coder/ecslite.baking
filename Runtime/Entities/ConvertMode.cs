// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using UnityEngine;

namespace Leopotam.EcsLite.Baking.Runtime.Entities
{
	internal enum ConvertMode
	{
		[InspectorName("Convert and Inject")]
		CONVERT_AND_INJECT,

		[InspectorName("Convert and Destroy")]
		CONVERT_AND_DESTROY,

		[InspectorName("Convert and Save")]
		CONVERT_AND_SAVE
	}
}