using System.Collections.Generic;
using Shump;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Shump
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(TransformSystemGroup))]
    public class SpawnRandomInSphereSystem : ComponentSystem
    {
        EntityQuery _group;

        protected override void OnCreateManager()
        {
            _group = GetEntityQuery(typeof(SpawnInPoint));
        }

        protected override void OnUpdate()
        {
            //Entities.ForEach((Entity e, SpawnInPoint spawner, ref LocalToWorld localToWorld) =>
            //{

            //    //var entity = PostUpdateCommands.Instantiate(spawner.Prefab);

            //    //PostUpdateCommands.SetComponent(entity, new LocalToWorld
            //    //{
            //    //    Value = float4x4.TRS(
            //    //        localToWorld.Position,
            //    //        quaternion.LookRotationSafe(new float3(0,1,0), math.up()),
            //    //        new float3(1.0f, 1.0f, 1.0f))
            //    //});

            //    //PostUpdateCommands.RemoveComponent<SpawnInPoint>(e);
            //});

            // Get a copy of the entity array.
            // Don't directly use the iterator -- we're going to remove
            // the buffer components, and it will invalidate the iterator.
            var iterator = _group.ToEntityArray(Allocator.TempJob);
            var entities = new NativeArray<Entity>(iterator.Length, Allocator.TempJob);
            iterator.CopyTo(entities);
            for (var j = 0; j < entities.Length; j++)
            {
                var spawner = EntityManager.GetSharedComponentData<SpawnInPoint>(entities[j]);
                //Change Init Position
                var localToWorld = EntityManager.GetComponentData<LocalToWorld>(entities[j]);
                spawner.Prefab.transform.position = localToWorld.Position;
                //Just create a gameObject to use the particle system
                var plane = GameObject.Instantiate(spawner.Prefab);
                // Remove the buffer component from the entity.
                EntityManager.RemoveComponent(entities[j], typeof(SpawnInPoint));
            }
            iterator.Dispose();
            entities.Dispose();
        }
    }
}
