﻿// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using System.Runtime.CompilerServices;

namespace Leopotam.EcsLite.Baking.Runtime.Components
{
	public static class PoolExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Replace<TComponent, TPool>(this TPool self, int entity, TComponent value)
			where TPool : IEcsPool
		{
			if (self.Has(entity))
			{
				self.Del(entity);
			}

			self.AddRaw(entity, value);
		}
	}
}