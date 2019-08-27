//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace JEM.UnityEngine.Extension
{
    /// <summary>
    ///     Set of utility extensions to BoxCollider class.
    /// </summary>
    public static class JEMExtensionBoxCollider
    {
        /// <summary>
        ///     Gets random point from box collider bounds.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static Vector3 GetRandomPoint([NotNull] this BoxCollider boxCollider, bool forNavMesh = false)
        {
            if (boxCollider == null) throw new ArgumentNullException(nameof(boxCollider));

            var p = boxCollider.transform.position;
            var c = boxCollider.center;
            var s = boxCollider.size;

            if (!forNavMesh)
            {
                return new Vector3(Random.Range(p.x + c.x - s.x / 2, p.x + c.x + s.x / 2),
                    Random.Range(p.y + c.y - s.y / 2, p.y + c.y + s.y / 2),
                    Random.Range(p.z + c.z - s.z / 2, p.z + c.z + s.z / 2));
            }

            var timeOut = DateTime.Now;
            var point = new Vector3(-99f, -999f, -99f);
            var path = new NavMeshPath();
            while (NavMesh.CalculatePath(p, point, NavMesh.AllAreas, path) == false)
            {
                point = new Vector3(Random.Range(p.x + c.x - s.x / 2, p.x + c.x + s.x / 2),
                    p.y,
                    Random.Range(p.z + c.z - s.z / 2, p.z + c.z + s.z / 2));

                if ((DateTime.Now - timeOut).Seconds > 3)
                    throw new NullReferenceException(
                        "Unable to generate random point from BoxCollider. GetRandomPoint timeout.");
            }

            return point;
        }
    }
}