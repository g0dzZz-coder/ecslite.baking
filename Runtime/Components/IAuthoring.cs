﻿// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

namespace Leopotam.EcsLite.Baking.Runtime.Components
{
	public interface IAuthoring
	{
		IBaker CreateBaker(EcsPackedEntityWithWorld entity);
	}
}