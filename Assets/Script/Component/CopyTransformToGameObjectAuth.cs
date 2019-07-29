using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class CopyTransformToGameObjectAuth : MonoBehaviour, IConvertGameObjectToEntity
{
    /// <inheritdoc />
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, default(CopyTransformToGameObject));
    }
}