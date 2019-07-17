using System.Collections.Generic;
using Shump;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Shump
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(TransformSystemGroup))]
    public class SpawnRandomInSphereSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity e, SpawnInPoint spawner, ref LocalToWorld localToWorld) =>
            {

                var entity = PostUpdateCommands.Instantiate(spawner.Prefab);

                PostUpdateCommands.SetComponent(entity, new LocalToWorld
                {
                    Value = float4x4.TRS(
                        localToWorld.Position,
                        quaternion.LookRotationSafe(new float3(0,1,0), math.up()),
                        new float3(1.0f, 1.0f, 1.0f))
                });

                PostUpdateCommands.RemoveComponent<SpawnInPoint>(e);
            });
        }
    }
}
