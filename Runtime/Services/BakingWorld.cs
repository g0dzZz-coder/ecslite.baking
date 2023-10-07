// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

namespace Leopotam.EcsLite.Baking.Runtime.Services
{
    internal static class BakingWorld
    {
        public static EcsWorld World;

        public static void Initialize(EcsWorld world) => World = world;

        public static void Dispose() => World = null;
    }
}
