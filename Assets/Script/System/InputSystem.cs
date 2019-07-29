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
                float3 delta = new float3(0,0,0);
                if (Input.GetKey(KeyCode.UpArrow))
                    delta = new float3(0, 1, 0);
                else if(Input.GetKey(KeyCode.DownArrow))
                    delta = new float3(0, -1, 0);
                else if (Input.GetKey(KeyCode.LeftArrow))
                    delta = new float3(-1, 0, 0);
                else if (Input.GetKey(KeyCode.RightArrow))
                    delta = new float3(1, 0, 0);
                var tempPos = translation.Value + delta * deltaTime * plane.moveSpeed;
                //Restrict the x value
                tempPos.x = math.clamp(tempPos.x, -6f, 6f);
                tempPos.y = math.clamp(tempPos.y, -2f, 6f);
                translation.Value = tempPos;

                //handle self rotate
                //Debug.Log(GetEulerAngle(rotation.Value).ToString());
                var rotateFactor = delta.x * 2;
                //var returnMode = false;
                if (rotateFactor.Equals(0f))
                {
                    //returnMode = true;
                    var curEulerAngle = GetEulerAngle(rotation.Value);
                    if (!curEulerAngle.y.Equals(0f))
                    {
                        rotateFactor = curEulerAngle.y > 0f ? -1f : 1f;
                    }
                }
                var tempRotation = math.mul(math.normalize(rotation.Value),
                    quaternion.AxisAngle(math.up(), plane.rotateSpeed * deltaTime * rotateFactor));
                var temp = GetEulerAngle(tempRotation);
                temp.y = math.clamp(temp.y, -0.8f, 0.8f);

                //if (returnMode && rotateFactor > 0f && )



                rotation.Value = quaternion.EulerXYZ(temp);
                //Debug.Log(GetEulerAngle(rotation.Value).ToString());
            });
        }

        float3 GetEulerAngle(quaternion qua)
        {
            float3 euler = new float3();
            float4 q = qua.value;
            euler.x = math.atan2(2 * (q.y * q.z + q.w * q.x), q.w * q.w - q.x * q.x - q.y * q.y + q.z * q.z);
            euler.y = math.asin(-2 * (q.x * q.z - q.w * q.y));
            euler.z = math.atan2(2 * (q.x * q.y + q.w * q.z), q.w * q.w + q.x * q.x - q.y * q.y - q.z * q.z);
            return euler;
        }
    }
}