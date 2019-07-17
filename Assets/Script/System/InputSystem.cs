using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Shump;


namespace Shump
{
    public class InputSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref MyPlane plane, ref Rotation rotation, ref Translation translation) =>
            {
                var deltaTime = Time.deltaTime;
                rotation.Value = math.mul(math.normalize(rotation.Value),
                    quaternion.AxisAngle(math.up(), plane.rotateSpeed * deltaTime));
                Debug.Log("rotation.Value:" + rotation.Value.ToString());

                float3 delta = new float3(0,0,0);
                if (Input.GetKey(KeyCode.UpArrow))
                    delta = new float3(0, 1, 0);
                else if(Input.GetKey(KeyCode.DownArrow))
                    delta = new float3(0, -1, 0);
                else if (Input.GetKey(KeyCode.LeftArrow))
                    delta = new float3(-1, 0, 0);
                else if (Input.GetKey(KeyCode.RightArrow))
                    delta = new float3(1, 0, 0);
                translation.Value = translation.Value + delta * deltaTime * plane.moveSpeed;
            });
        }
    }
}