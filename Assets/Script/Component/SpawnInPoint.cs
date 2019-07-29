using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Shump
{
    [Serializable]
    public struct SpawnInPoint : ISharedComponentData, IEquatable<SpawnInPoint>
    {
        public GameObject Prefab;

        public bool Equals(SpawnInPoint other)
        {
            return Prefab != null && Prefab.Equals(other.Prefab);
        }

        public override int GetHashCode()
        {
            //int hash = MaxVoxelCount.GetHashCode();

            //if (!ReferenceEquals(RendererSettings, null))
            //    hash ^= RendererSettings.GetHashCode();

            return Prefab.GetHashCode();
        }
    }

    namespace Authoring
    {
        [RequiresEntityConversion]
        public class SpawnInPoint : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
        {
            public GameObject Prefab;

            // Lets you convert the editor data representation to the entity optimal runtime representation

            public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
            {
                var spawnerData = new Shump.SpawnInPoint
                {
                    // The referenced prefab will be converted due to DeclareReferencedPrefabs.
                    // So here we simply map the game object to an entity reference to that prefab.
                    //Prefab = conversionSystem.GetPrimaryEntity(Prefab),
                    Prefab = Prefab
                };
                dstManager.AddSharedComponentData(entity, spawnerData);
            }

            // Referenced prefabs have to be declared so that the conversion system knows about them ahead of time
            public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
            {
                referencedPrefabs.Add(Prefab);
            }
        }
    }
}


