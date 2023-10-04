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