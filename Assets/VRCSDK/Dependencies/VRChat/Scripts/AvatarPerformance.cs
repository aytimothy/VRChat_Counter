using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace VRCSDK2
{
    public enum PerformanceRating
    {
        None = 0,
        VeryGood = 1,
        Good = 2,
        Medium = 3,
        Bad = 4,
        VeryBad = 5
    }

    public enum AvatarPerformanceCategory
    {
        None,

        Overall,

        PolyCount,
        AABB,
        SkinnedMeshCount,
        MeshCount,
        MaterialCount,
        DynamicBoneComponentCount,
        DynamicBoneAffectedTransformCount,
        DynamicBoneColliderCount,
        DynamicBoneCollisionCheckCount,
        AnimatorCount,
        BoneCount,
        LightCount,
        ParticleSystemCount,
        ParticleTotalCount,
        ParticleTotalActiveMeshPolyCount,
        ParticleTrailsEnabled,
        ParticleCollisionEnabled,
        TrailRendererCount,
        LineRendererCount,
        ClothCount,
        ClothMaxVertices,
        PhysicsColliderCount,
        PhysicsRigidbodyCount,
        AudioSourceCount,

        AvatarPerformanceCategoryCount
    }

    public enum PerformanceInfoDisplayLevel
    {
        None,

        Verbose,
        Info,
        Warning,
        Error
    }

    public class AvatarPerformanceStats
    {
        public int PolyCount;
        public Bounds AABB;
        public int SkinnedMeshCount;
        public int MeshCount;
        public int MaterialCount;
        public int AnimatorCount;
        public int BoneCount;
        public int LightCount;
        public int ParticleSystemCount;
        public int ParticleTotalCount;
        public int ParticleTotalActiveMeshPolyCount;
        public bool ParticleTrailsEnabled;
        public bool ParticleCollisionEnabled;
        public int TrailRendererCount;
        public int LineRendererCount;
        public int DynamicBoneComponentCount;
        public int DynamicBoneAffectedTransformCount;
        public int DynamicBoneColliderCount;
        public int DynamicBoneCollisionCheckCount;          // number of collider affected transforms X number of colliders
        public int ClothCount;
        public int ClothMaxVertices;
        public int PhysicsColliderCount;
        public int PhysicsRigidbodyCount;
        public int AudioSourceCount;

        public string AvatarName;

        public static AvatarPerformanceStats VeryGoodPerformanceStatLimits = new AvatarPerformanceStats()
        {
            PolyCount = 32000,
            AABB = new Bounds(Vector3.zero, new Vector3(2.5f, 2.5f, 2.5f)),
            SkinnedMeshCount = 1,
            MeshCount = 4,
            MaterialCount = 4,
            AnimatorCount = 1,
            BoneCount = 75,
            LightCount = 0,
            ParticleSystemCount = 0,
            ParticleTotalCount = 0,
            ParticleTotalActiveMeshPolyCount = 0,
            ParticleTrailsEnabled = false,
            ParticleCollisionEnabled = false,
            TrailRendererCount = 1,
            LineRendererCount = 1,
            DynamicBoneComponentCount = 0,
            DynamicBoneAffectedTransformCount = 0,
            DynamicBoneColliderCount = 0,
            DynamicBoneCollisionCheckCount = 0,
            ClothCount = 0,
            ClothMaxVertices = 0,
            PhysicsColliderCount = 0,
            PhysicsRigidbodyCount = 0,
            AudioSourceCount = 1
        };

        public static AvatarPerformanceStats GoodPeformanceStatLimits = new AvatarPerformanceStats()
        {
            PolyCount = 70000,
            AABB = new Bounds(Vector3.zero, new Vector3(4f, 4f, 4f)),
            SkinnedMeshCount = 2,
            MeshCount = 8,
            MaterialCount = 8,
            AnimatorCount = 4,
            BoneCount = 150,
            LightCount = 0,
            ParticleSystemCount = 4,
            ParticleTotalCount = 1000,
            ParticleTotalActiveMeshPolyCount = 2000,
            ParticleTrailsEnabled = false,
            ParticleCollisionEnabled = false,
            TrailRendererCount = 2,
            LineRendererCount = 2,
            DynamicBoneComponentCount = 4,
            DynamicBoneAffectedTransformCount = 16,
            DynamicBoneColliderCount = 0,
            DynamicBoneCollisionCheckCount = 0,
            ClothCount = 1,
            ClothMaxVertices = 100,
            PhysicsColliderCount = 1,
            PhysicsRigidbodyCount = 1,
            AudioSourceCount = 4
        };

        public static AvatarPerformanceStats MediumPeformanceStatLimits = new AvatarPerformanceStats()
        {
            PolyCount = 70000,
            AABB = new Bounds(Vector3.zero, new Vector3(5f, 6f, 5f)),
            SkinnedMeshCount = 8,
            MeshCount = 16,
            MaterialCount = 16,
            AnimatorCount = 16,
            BoneCount = 256,
            LightCount = 0,
            ParticleSystemCount = 8,
            ParticleTotalCount = 2500,
            ParticleTotalActiveMeshPolyCount = 5000,
            ParticleTrailsEnabled = true,
            ParticleCollisionEnabled = true,
            TrailRendererCount = 4,
            LineRendererCount = 4,
            DynamicBoneComponentCount = 16,
            DynamicBoneAffectedTransformCount = 32,
            DynamicBoneColliderCount = 4,
            DynamicBoneCollisionCheckCount = 8,
            ClothCount = 1,
            ClothMaxVertices = 250,
            PhysicsColliderCount = 8,
            PhysicsRigidbodyCount = 8,
            AudioSourceCount = 8
        };

        public static AvatarPerformanceStats BadPeformanceStatLimits = new AvatarPerformanceStats()
        {
            PolyCount = 70000,
            AABB = new Bounds(Vector3.zero, new Vector3(5f, 6f, 5f)),
            SkinnedMeshCount = 16,
            MeshCount = 24,
            MaterialCount = 32,
            AnimatorCount = 32,
            BoneCount = 400,
            LightCount = 1,
            ParticleSystemCount = 16,
            ParticleTotalCount = 5000,
            ParticleTotalActiveMeshPolyCount = 10000,
            ParticleTrailsEnabled = true,
            ParticleCollisionEnabled = true,
            TrailRendererCount = 8,
            LineRendererCount = 8,
            DynamicBoneComponentCount = 32,
            DynamicBoneAffectedTransformCount = 256,
            DynamicBoneColliderCount = 32,
            DynamicBoneCollisionCheckCount = 256,
            ClothCount = 2,
            ClothMaxVertices = 500,
            PhysicsColliderCount = 8,
            PhysicsRigidbodyCount = 8,
            AudioSourceCount = 8
        };

        private PerformanceRating[] _performanceRatingCache;

        public AvatarPerformanceStats()
        {
            _performanceRatingCache = new PerformanceRating[(int)AvatarPerformanceCategory.AvatarPerformanceCategoryCount];
        }

        public void GetSDKPerformanceInfoText(out string text, out PerformanceInfoDisplayLevel displayLevel, AvatarPerformanceCategory perfCategory)
        {
            PerformanceRating rating = GetPerformanceRatingForCategory(perfCategory);
            GetSDKPerformanceInfoText(out text, out displayLevel, perfCategory, rating);
        }

        public PerformanceRating GetPerformanceRatingForCategory(AvatarPerformanceCategory perfCategory)
        {
            if (_performanceRatingCache[(int)perfCategory] == PerformanceRating.None)
                _performanceRatingCache[(int)perfCategory] = CalculatePerformanceRatingForCategory(perfCategory);
            return _performanceRatingCache[(int)perfCategory];
        }

        private PerformanceRating CalculatePerformanceRatingForCategory(AvatarPerformanceCategory perfCategory)
        {
            switch (perfCategory)
            {
                case AvatarPerformanceCategory.Overall:
                    {
                        int maxRating = (int)PerformanceRating.None;

                        foreach (AvatarPerformanceCategory c in Enum.GetValues(typeof(AvatarPerformanceCategory)))
                        {
                            if (c == AvatarPerformanceCategory.None ||
                                c == AvatarPerformanceCategory.Overall ||
                                c == AvatarPerformanceCategory.AvatarPerformanceCategoryCount)
                            {
                                continue;
                            }

                            int r = (int)GetPerformanceRatingForCategory(c);
                            if (r > maxRating)
                                maxRating = r;
                        }

                        return (PerformanceRating)maxRating;
                    }
                case AvatarPerformanceCategory.PolyCount:
                    return CalculatePerformanceRating((x, y) => x.PolyCount - y.PolyCount);
                case AvatarPerformanceCategory.AABB:
                    return CalculatePerformanceRating((x, y) => (ApproxLessOrEqual(x.AABB.extents.x, y.AABB.extents.x) && ApproxLessOrEqual(x.AABB.extents.y, y.AABB.extents.y) && ApproxLessOrEqual(x.AABB.extents.z, y.AABB.extents.z)) ? -1 : 1);
                case AvatarPerformanceCategory.SkinnedMeshCount:
                    return CalculatePerformanceRating((x, y) => x.SkinnedMeshCount - y.SkinnedMeshCount);
                case AvatarPerformanceCategory.MeshCount:
                    return CalculatePerformanceRating((x, y) => x.MeshCount - y.MeshCount);
                case AvatarPerformanceCategory.MaterialCount:
                    return CalculatePerformanceRating((x, y) => x.MaterialCount - y.MaterialCount);
                case AvatarPerformanceCategory.AnimatorCount:
                    return CalculatePerformanceRating((x, y) => x.AnimatorCount - y.AnimatorCount);
                case AvatarPerformanceCategory.BoneCount:
                    return CalculatePerformanceRating((x, y) => x.BoneCount - y.BoneCount);
                case AvatarPerformanceCategory.LightCount:
                    return CalculatePerformanceRating((x, y) => x.LightCount - y.LightCount);
                case AvatarPerformanceCategory.ParticleSystemCount:
                    return CalculatePerformanceRating((x, y) => x.ParticleSystemCount - y.ParticleSystemCount);
                case AvatarPerformanceCategory.ParticleTotalCount:
                    return CalculatePerformanceRating((x, y) => x.ParticleTotalCount - y.ParticleTotalCount);
                case AvatarPerformanceCategory.ParticleTotalActiveMeshPolyCount:
                    return CalculatePerformanceRating((x, y) => x.ParticleTotalActiveMeshPolyCount - y.ParticleTotalActiveMeshPolyCount);
                case AvatarPerformanceCategory.ParticleTrailsEnabled:
                    return CalculatePerformanceRating((x, y) =>
                    {
                        if (x.ParticleTrailsEnabled == y.ParticleTrailsEnabled)
                            return 0;
                        return x.ParticleTrailsEnabled ? 1 : -1;
                    });
                case AvatarPerformanceCategory.ParticleCollisionEnabled:
                    return CalculatePerformanceRating((x, y) =>
                    {
                        if (x.ParticleCollisionEnabled == y.ParticleCollisionEnabled)
                            return 0;
                        return x.ParticleCollisionEnabled ? 1 : -1;
                    });
                case AvatarPerformanceCategory.TrailRendererCount:
                    return CalculatePerformanceRating((x, y) => x.TrailRendererCount - y.TrailRendererCount);
                case AvatarPerformanceCategory.LineRendererCount:
                    return CalculatePerformanceRating((x, y) => x.LineRendererCount - y.LineRendererCount);
                case AvatarPerformanceCategory.DynamicBoneComponentCount:
                    return CalculatePerformanceRating((x, y) => x.DynamicBoneComponentCount - y.DynamicBoneComponentCount);
                case AvatarPerformanceCategory.DynamicBoneAffectedTransformCount:
                    return CalculatePerformanceRating((x, y) => x.DynamicBoneAffectedTransformCount - y.DynamicBoneAffectedTransformCount);
                case AvatarPerformanceCategory.DynamicBoneColliderCount:
                    return CalculatePerformanceRating((x, y) => x.DynamicBoneColliderCount - y.DynamicBoneColliderCount);
                case AvatarPerformanceCategory.DynamicBoneCollisionCheckCount:
                    return CalculatePerformanceRating((x, y) => x.DynamicBoneCollisionCheckCount - y.DynamicBoneCollisionCheckCount);
                case AvatarPerformanceCategory.ClothCount:
                    return CalculatePerformanceRating((x, y) => x.ClothCount - y.ClothCount);
                case AvatarPerformanceCategory.ClothMaxVertices:
                    return CalculatePerformanceRating((x, y) => x.ClothMaxVertices - y.ClothMaxVertices);
                case AvatarPerformanceCategory.PhysicsColliderCount:
                    return CalculatePerformanceRating((x, y) => x.PhysicsColliderCount - y.PhysicsColliderCount);
                case AvatarPerformanceCategory.PhysicsRigidbodyCount:
                    return CalculatePerformanceRating((x, y) => x.PhysicsRigidbodyCount - y.PhysicsRigidbodyCount);
                case AvatarPerformanceCategory.AudioSourceCount:
                    return CalculatePerformanceRating((x, y) => x.AudioSourceCount - y.AudioSourceCount);
                default:
                    return PerformanceRating.None;
            }
        }

        private PerformanceRating CalculatePerformanceRating(System.Comparison<AvatarPerformanceStats> compareFn)
        {
            if (compareFn(this, VeryGoodPerformanceStatLimits) <= 0)
                return PerformanceRating.VeryGood;
            if (compareFn(this, GoodPeformanceStatLimits) <= 0)
                return PerformanceRating.Good;
            if (compareFn(this, MediumPeformanceStatLimits) <= 0)
                return PerformanceRating.Medium;
            if (compareFn(this, BadPeformanceStatLimits) <= 0)
                return PerformanceRating.Bad;
            return PerformanceRating.VeryBad;
        }

        public void CalculateAllPerformanceRatings()
        {
            for (int i = 0; i < _performanceRatingCache.Length; i++)
            {
                _performanceRatingCache[i] = PerformanceRating.None;
            }

            foreach (AvatarPerformanceCategory perfCategory in Enum.GetValues(typeof(AvatarPerformanceCategory)))
            {
                if (perfCategory == AvatarPerformanceCategory.None ||
                    perfCategory == AvatarPerformanceCategory.AvatarPerformanceCategoryCount)
                {
                    continue;
                }

                if (_performanceRatingCache[(int)perfCategory] == PerformanceRating.None)
                    _performanceRatingCache[(int)perfCategory] = CalculatePerformanceRatingForCategory(perfCategory);
            }
        }

        public void GetSDKPerformanceInfoText(out string text, out PerformanceInfoDisplayLevel displayLevel, AvatarPerformanceCategory perfCategory, PerformanceRating rating)
        {
            text = "";
            displayLevel = PerformanceInfoDisplayLevel.None;

            switch (perfCategory)
            {
                case AvatarPerformanceCategory.Overall:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Info;
                            text = string.Format("Overall Performance: {0}", GetPerformanceRatingDisplayName(rating));
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Overall Performance: {0} - This avatar may not perform well on many systems. See additional warnings for suggestions on how to improve performance. Click 'Avatar Optimization Tips' below for more information.", GetPerformanceRatingDisplayName(rating));
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Overall Performance: {0} - This avatar does not meet minimum performance requirements for VRChat. See additional warnings for suggestions on how to improve performance. Click 'Avatar Optimization Tips' below for more information.", GetPerformanceRatingDisplayName(rating));
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.PolyCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            displayLevel = PerformanceInfoDisplayLevel.Info;
                            text = string.Format("Polygons: {0}", PolyCount);
                            break;
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Info;
                            text = string.Format("Polygons: {0} (Recommended: {1})", PolyCount, VeryGoodPerformanceStatLimits.PolyCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Polygons: {0} - Please try to reduce your avatar poly count to less than {1} (Recommended: {2})", PolyCount, GoodPeformanceStatLimits.PolyCount, VeryGoodPerformanceStatLimits.PolyCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Error;
                            text = string.Format("Polygons: {0} - This avatar has too many polygons. It must have less than {1} and should have less than {2}", PolyCount, BadPeformanceStatLimits.PolyCount, VeryGoodPerformanceStatLimits.PolyCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.AABB:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Bounding box (AABB) size: {0}", AABB.size.ToString());
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("This avatar's bounding box (AABB) is too large on at least one axis. Current size: {0}, Maximum size: {1}", AABB.size.ToString(), BadPeformanceStatLimits.AABB.size.ToString());
                            break;
                    }

                    break;
                case AvatarPerformanceCategory.SkinnedMeshCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Skinned Mesh Renderers: {0}", SkinnedMeshCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Skinned Mesh Renderers: {0} (Recommended: {1}) - Combine multiple skinned meshes for optimal performance.", SkinnedMeshCount, VeryGoodPerformanceStatLimits.SkinnedMeshCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Skinned Mesh Renderers: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many skinned meshes. Combine multiple skinned meshes for optimal performance.", SkinnedMeshCount, BadPeformanceStatLimits.SkinnedMeshCount, VeryGoodPerformanceStatLimits.SkinnedMeshCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.MeshCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Mesh Renderers: {0}", MeshCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Mesh Renderers: {0} (Recommended: {1}) - Combine multiple meshes for optimal performance.", MeshCount, VeryGoodPerformanceStatLimits.MeshCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Mesh Renderers: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many meshes. Combine multiple meshes for optimal performance.", MeshCount, BadPeformanceStatLimits.MeshCount, VeryGoodPerformanceStatLimits.MeshCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.MaterialCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Unique Materials: {0}", MaterialCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Unique Materials: {0} (Recommended: {1}) - Use fewer materials, or combine materials and textures for optimal performance.", MaterialCount, VeryGoodPerformanceStatLimits.MaterialCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Unique Materials: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many materials. Use fewer materials, or combine materials and textures for optimal performance.", MaterialCount, BadPeformanceStatLimits.MaterialCount, VeryGoodPerformanceStatLimits.MaterialCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.AnimatorCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Animator Count: {0}", AnimatorCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Animator Count: {0} (Recommended: {1}) - Avoid using extra Animators for optimal performance.", AnimatorCount, VeryGoodPerformanceStatLimits.AnimatorCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Animator Count: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many Animators. Avoid using extra Animators for optimal performance.", AnimatorCount, BadPeformanceStatLimits.AnimatorCount, VeryGoodPerformanceStatLimits.AnimatorCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.BoneCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Bones: {0}", BoneCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Bones: {0} (Recommended: {1}) - Reduce number of bones for optimal performance.", BoneCount, VeryGoodPerformanceStatLimits.BoneCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Bones: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many bones. Reduce number of bones for optimal performance.", BoneCount, BadPeformanceStatLimits.BoneCount, VeryGoodPerformanceStatLimits.BoneCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.LightCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Lights: {0}", LightCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Lights: {0} (Recommended: {1}) - Avoid use of dynamic lights for optimal performance.", LightCount, VeryGoodPerformanceStatLimits.LightCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Lights: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many dynamic lights. Avoid use of dynamic lights for optimal performance.", LightCount, BadPeformanceStatLimits.LightCount, VeryGoodPerformanceStatLimits.LightCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.ParticleSystemCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Particle Systems: {0}", ParticleSystemCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Particle Systems: {0} (Recommended: {1}) - Reduce number of particle systems for better performance.", ParticleSystemCount, VeryGoodPerformanceStatLimits.ParticleSystemCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Particle Systems: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many particle systems. Reduce number of particle systems for better performance.", ParticleSystemCount, BadPeformanceStatLimits.ParticleSystemCount, VeryGoodPerformanceStatLimits.ParticleSystemCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.ParticleTotalCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Total Combined Max Particle Count: {0}", ParticleTotalCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Total Combined Max Particle Count: {0} (Recommended: {1}) - Reduce 'Max Particles' across all particle systems for better performance.", ParticleTotalCount, VeryGoodPerformanceStatLimits.ParticleTotalCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Total Combined Max Particle Count: {0} (Maximum: {1}, Recommended: {2}) - This avatar uses too many particles. Reduce 'Max Particles' across all particle systems for better performance.", ParticleTotalCount, BadPeformanceStatLimits.ParticleTotalCount, VeryGoodPerformanceStatLimits.ParticleTotalCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.ParticleTotalActiveMeshPolyCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Mesh Particle Active Total Poly Count: {0}", ParticleTotalActiveMeshPolyCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Mesh Particle Active Total Poly Count: {0} (Recommended: {1}) - Reduce number of polys in particle meshes, and reduce 'Max Particles' for better performance.", ParticleTotalActiveMeshPolyCount, VeryGoodPerformanceStatLimits.ParticleTotalActiveMeshPolyCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Mesh Particle Active Total Poly Count: {0} (Maximum: {1}, Recommended: {2}) - This avatar uses too many mesh particle polygons. Reduce number of polys in particle meshes, and reduce 'Max Particles' for better performance.", ParticleTotalActiveMeshPolyCount, BadPeformanceStatLimits.ParticleTotalCount, VeryGoodPerformanceStatLimits.ParticleTotalActiveMeshPolyCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.ParticleTrailsEnabled:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Particle Trails Enabled: {0}", ParticleTrailsEnabled);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Particle Trails Enabled: {0} (Recommended: {1}) - Avoid particle trails for better performance.", ParticleTrailsEnabled, VeryGoodPerformanceStatLimits.ParticleTrailsEnabled);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.ParticleCollisionEnabled:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Particle Collision Enabled: {0}", ParticleCollisionEnabled);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Particle Collision Enabled: {0} (Recommended: {1}) - Avoid particle collision for better performance.", ParticleCollisionEnabled, VeryGoodPerformanceStatLimits.ParticleCollisionEnabled);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.TrailRendererCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Trail Renderers: {0}", TrailRendererCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Trail Renderers: {0} (Recommended: {1}) - Reduce number of TrailRenderers for better performance.", TrailRendererCount, VeryGoodPerformanceStatLimits.TrailRendererCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Trail Renderers: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many TrailRenderers. Reduce number of TrailRenderers for better performance.", TrailRendererCount, BadPeformanceStatLimits.TrailRendererCount, VeryGoodPerformanceStatLimits.TrailRendererCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.LineRendererCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Line Renderers: {0}", LineRendererCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Line Renderers: {0} (Recommended: {1}) - Reduce number of LineRenderers for better performance.", LineRendererCount, VeryGoodPerformanceStatLimits.LineRendererCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Line Renderers: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many LineRenderers. Reduce number of LineRenderers for better performance.", LineRendererCount, BadPeformanceStatLimits.LineRendererCount, VeryGoodPerformanceStatLimits.LineRendererCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.DynamicBoneComponentCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Dynamic Bone Components: {0}", DynamicBoneComponentCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Dynamic Bone Components: {0} (Recommended: {1}) - Reduce number of DynamicBone components for better performance.", DynamicBoneComponentCount, VeryGoodPerformanceStatLimits.DynamicBoneComponentCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Dynamic Bone Components: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many DynamicBone components. Reduce number of DynamicBone components for better performance.", DynamicBoneComponentCount, BadPeformanceStatLimits.DynamicBoneComponentCount, VeryGoodPerformanceStatLimits.DynamicBoneComponentCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.DynamicBoneAffectedTransformCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Dynamic Bone Affected Transform Count: {0}", DynamicBoneAffectedTransformCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Dynamic Bone Affected Transform Count: {0} (Recommended: {1}) - Reduce number of transforms in hierarchy under DynamicBone components for better performance.", DynamicBoneAffectedTransformCount, VeryGoodPerformanceStatLimits.DynamicBoneAffectedTransformCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Dynamic Bone Affected Transform Count: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many transforms affected by DynamicBone. Reduce number of transforms in hierarchy under DynamicBone components for better performance.", DynamicBoneAffectedTransformCount, BadPeformanceStatLimits.DynamicBoneAffectedTransformCount, VeryGoodPerformanceStatLimits.DynamicBoneAffectedTransformCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.DynamicBoneColliderCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Dynamic Bone Collider Count: {0}", DynamicBoneColliderCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Dynamic Bone Collider Count: {0} (Recommended: {1}) - Avoid use of DynamicBoneColliders for better performance.", DynamicBoneColliderCount, VeryGoodPerformanceStatLimits.DynamicBoneColliderCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Dynamic Bone Collider Count: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many DynamicBoneColliders. Avoid use of DynamicBoneColliders for better performance.", DynamicBoneColliderCount, BadPeformanceStatLimits.DynamicBoneColliderCount, VeryGoodPerformanceStatLimits.DynamicBoneColliderCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.DynamicBoneCollisionCheckCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Dynamic Bone Collision Check Count: {0}", DynamicBoneCollisionCheckCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Dynamic Bone Collision Check Count: {0} (Recommended: {1}) - Avoid use of DynamicBoneColliders for better performance.", DynamicBoneCollisionCheckCount, VeryGoodPerformanceStatLimits.DynamicBoneCollisionCheckCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Dynamic Bone Collision Check Count: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many DynamicBoneColliders. Avoid use of DynamicBoneColliders for better performance.", DynamicBoneCollisionCheckCount, BadPeformanceStatLimits.DynamicBoneCollisionCheckCount, VeryGoodPerformanceStatLimits.DynamicBoneCollisionCheckCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.ClothCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Cloth Component Count: {0}", ClothCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Cloth Component Count: {0} (Recommended: {1}) - Avoid use of cloth for optimal performance.", ClothCount, VeryGoodPerformanceStatLimits.ClothCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Cloth Component Count: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many Cloth components. Avoid use of cloth for optimal performance.", ClothCount, BadPeformanceStatLimits.ClothCount, VeryGoodPerformanceStatLimits.ClothCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.ClothMaxVertices:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Cloth Total Vertex Count: {0}", ClothMaxVertices);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Cloth Total Vertex Count: {0} (Recommended: {1}) - Reduce number of vertices in cloth meshes for improved performance.", ClothMaxVertices, VeryGoodPerformanceStatLimits.ClothMaxVertices);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Cloth Total Vertex Count: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many vertices in cloth meshes. Reduce number of vertices in cloth meshes for improved performance.", ClothMaxVertices, BadPeformanceStatLimits.ClothMaxVertices, VeryGoodPerformanceStatLimits.ClothMaxVertices);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.PhysicsColliderCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Physics Collider Count: {0}", PhysicsColliderCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Physics Collider Count: {0} (Recommended: {1}) - Avoid use of colliders for optimal performance.", PhysicsColliderCount, VeryGoodPerformanceStatLimits.PhysicsColliderCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Physics Collider Count: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many colliders. Avoid use of colliders for optimal performance.", PhysicsColliderCount, BadPeformanceStatLimits.PhysicsColliderCount, VeryGoodPerformanceStatLimits.PhysicsColliderCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.PhysicsRigidbodyCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Physics Rigidbody Count: {0}", PhysicsRigidbodyCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Physics Rigidbody Count: {0} (Recommended: {1}) - Avoid use of rigidbodies for optimal performance.", PhysicsRigidbodyCount, VeryGoodPerformanceStatLimits.PhysicsRigidbodyCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Physics Rigidbody Count: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many rigidbodies. Avoid use of rigidbodies for optimal performance.", PhysicsRigidbodyCount, BadPeformanceStatLimits.PhysicsRigidbodyCount, VeryGoodPerformanceStatLimits.PhysicsRigidbodyCount);
                            break;
                    }
                    break;
                case AvatarPerformanceCategory.AudioSourceCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                        case PerformanceRating.Good:
                            displayLevel = PerformanceInfoDisplayLevel.Verbose;
                            text = string.Format("Audio Sources: {0}", AudioSourceCount);
                            break;
                        case PerformanceRating.Medium:
                        case PerformanceRating.Bad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Audio Sources: {0} (Recommended: {1}) - Reduce number of audio sources for better performance.", AudioSourceCount, VeryGoodPerformanceStatLimits.AudioSourceCount);
                            break;
                        case PerformanceRating.VeryBad:
                            displayLevel = PerformanceInfoDisplayLevel.Warning;
                            text = string.Format("Audio Sources: {0} (Maximum: {1}, Recommended: {2}) - This avatar has too many audio sources. Reduce number of audio sources for better performance.", AudioSourceCount, BadPeformanceStatLimits.AudioSourceCount, VeryGoodPerformanceStatLimits.AudioSourceCount);
                            break;
                    }
                    break;
                default:
                    text = "";
                    displayLevel = PerformanceInfoDisplayLevel.None;
                    break;
            }
        }

        public string GetStatTextForCategory(AvatarPerformanceCategory perfCategory, PerformanceRating rating)
        {
            switch (perfCategory)
            {
                case AvatarPerformanceCategory.Overall:
                    return string.Format("Overall Performance: {0}", GetPerformanceRatingDisplayName(rating));
                case AvatarPerformanceCategory.PolyCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Polygons: {0}", PolyCount);
                        default:
                            return string.Format("Polygons: {0} ({1})", PolyCount, VeryGoodPerformanceStatLimits.PolyCount);
                    }
                case AvatarPerformanceCategory.AABB:
                    switch (rating)
                    {
                        case PerformanceRating.VeryBad:
                            return string.Format("Bounds Size: {0} ({1})", AABB.size.ToString(), BadPeformanceStatLimits.AABB.size.ToString());
                        default:
                            return string.Format("Bounds Size: {0}", AABB.size.ToString());
                    }

                case AvatarPerformanceCategory.SkinnedMeshCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Skinned Meshes: {0}", SkinnedMeshCount);
                        default:
                            return string.Format("Skinned Meshes: {0} ({1})", SkinnedMeshCount, VeryGoodPerformanceStatLimits.SkinnedMeshCount);
                    }
                case AvatarPerformanceCategory.MeshCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Meshes: {0}", MeshCount);
                        default:
                            return string.Format("Meshes: {0} ({1})", MeshCount, VeryGoodPerformanceStatLimits.MeshCount);

                    }
                case AvatarPerformanceCategory.MaterialCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Unique Materials: {0}", MaterialCount);
                        default:
                            return string.Format("Unique Materials: {0} ({1})", MaterialCount, VeryGoodPerformanceStatLimits.MaterialCount);
                    }
                case AvatarPerformanceCategory.AnimatorCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Animators: {0}", AnimatorCount);
                        default:
                            return string.Format("Animators: {0} ({1})", AnimatorCount, VeryGoodPerformanceStatLimits.AnimatorCount);
                    }
                case AvatarPerformanceCategory.BoneCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Bones: {0}", BoneCount);
                        default:
                            return string.Format("Bones: {0} ({1})", BoneCount, VeryGoodPerformanceStatLimits.BoneCount);
                    }
                case AvatarPerformanceCategory.LightCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Lights: {0}", LightCount);
                        default:
                            return string.Format("Lights: {0} ({1})", LightCount, VeryGoodPerformanceStatLimits.LightCount);
                    }
                case AvatarPerformanceCategory.ParticleSystemCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Particle Systems: {0}", ParticleSystemCount);
                        default:
                            return string.Format("Particle Systems: {0} ({1})", ParticleSystemCount, VeryGoodPerformanceStatLimits.ParticleSystemCount);
                    }
                case AvatarPerformanceCategory.ParticleTotalCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Total Particles Active: {0}", ParticleTotalCount);
                        default:
                            return string.Format("Total Particles Active: {0} ({1})", ParticleTotalCount, VeryGoodPerformanceStatLimits.ParticleTotalCount);
                    }
                case AvatarPerformanceCategory.ParticleTotalActiveMeshPolyCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Mesh Particle Active Polys: {0}", ParticleTotalActiveMeshPolyCount);
                        default:
                            return string.Format("Mesh Particle Active Polys: {0} ({1})", ParticleTotalActiveMeshPolyCount, VeryGoodPerformanceStatLimits.ParticleTotalActiveMeshPolyCount);
                    }
                case AvatarPerformanceCategory.ParticleTrailsEnabled:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Particle Trails Enabled: {0}", ParticleTrailsEnabled);
                        default:
                            return string.Format("Particle Trails Enabled: {0} ({1})", ParticleTrailsEnabled, VeryGoodPerformanceStatLimits.ParticleTrailsEnabled);
                    }
                case AvatarPerformanceCategory.ParticleCollisionEnabled:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Particle Collision Enabled: {0}", ParticleCollisionEnabled);
                        default:
                            return string.Format("Particle Collision Enabled: {0} ({1})", ParticleCollisionEnabled, VeryGoodPerformanceStatLimits.ParticleCollisionEnabled);
                    }
                case AvatarPerformanceCategory.TrailRendererCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Trail Renderers: {0}", TrailRendererCount);
                        default:
                            return string.Format("Trail Renderers: {0} ({1})", TrailRendererCount, VeryGoodPerformanceStatLimits.TrailRendererCount);
                    }
                case AvatarPerformanceCategory.LineRendererCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Line Renderers: {0}", LineRendererCount);
                        default:
                            return string.Format("Line Renderers: {0} ({1})", LineRendererCount, VeryGoodPerformanceStatLimits.LineRendererCount);
                    }
                case AvatarPerformanceCategory.DynamicBoneComponentCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Dynamic Bone Components: {0}", DynamicBoneComponentCount);
                        default:
                            return string.Format("Dynamic Bone Components: {0} ({1})", DynamicBoneComponentCount, VeryGoodPerformanceStatLimits.DynamicBoneComponentCount);
                    }
                case AvatarPerformanceCategory.DynamicBoneAffectedTransformCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Dynamic Bone Transforms: {0}", DynamicBoneAffectedTransformCount);
                        default:
                            return string.Format("Dynamic Bone Transforms: {0} ({1})", DynamicBoneAffectedTransformCount, VeryGoodPerformanceStatLimits.DynamicBoneAffectedTransformCount);
                    }
                case AvatarPerformanceCategory.DynamicBoneColliderCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Dynamic Bone Colliders: {0}", DynamicBoneColliderCount);
                        default:
                            return string.Format("Dynamic Bone Colliders: {0} ({1})", DynamicBoneColliderCount, VeryGoodPerformanceStatLimits.DynamicBoneColliderCount);
                    }
                case AvatarPerformanceCategory.DynamicBoneCollisionCheckCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Dynamic Bone Collision Check Count: {0}", DynamicBoneCollisionCheckCount);
                        default:
                            return string.Format("Dynamic Bone Collision Check Count: {0} ({1})", DynamicBoneCollisionCheckCount, VeryGoodPerformanceStatLimits.DynamicBoneCollisionCheckCount);
                    }
                case AvatarPerformanceCategory.ClothCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Cloths: {0}", ClothCount);
                        default:
                            return string.Format("Cloths: {0} ({1})", ClothCount, VeryGoodPerformanceStatLimits.ClothCount);
                    }
                case AvatarPerformanceCategory.ClothMaxVertices:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Total Cloth Vertices: {0}", ClothMaxVertices);
                        default:
                            return string.Format("Total Cloth Vertices: {0} ({1})", ClothMaxVertices, VeryGoodPerformanceStatLimits.ClothMaxVertices);
                    }
                case AvatarPerformanceCategory.PhysicsColliderCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Physics Colliders: {0}", PhysicsColliderCount);
                        default:
                            return string.Format("Physics Colliders: {0} ({1})", PhysicsColliderCount, VeryGoodPerformanceStatLimits.PhysicsColliderCount);
                    }
                case AvatarPerformanceCategory.PhysicsRigidbodyCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Physics Rigidbodies: {0}", PhysicsRigidbodyCount);
                        default:
                            return string.Format("Physics Rigidbodies: {0} ({1})", PhysicsRigidbodyCount, VeryGoodPerformanceStatLimits.PhysicsRigidbodyCount);
                    }
                case AvatarPerformanceCategory.AudioSourceCount:
                    switch (rating)
                    {
                        case PerformanceRating.VeryGood:
                            return string.Format("Audio Sources: {0}", AudioSourceCount);
                        default:
                            return string.Format("Audio Sources: {0} ({1})", AudioSourceCount, VeryGoodPerformanceStatLimits.AudioSourceCount);
                    }
                default:
                    return "#AvatarPerformanceCategory - " + perfCategory;
            }
        }

        public static string GetPerformanceRatingDisplayName(PerformanceRating rating)
        {
            switch (rating)
            {
                case PerformanceRating.VeryGood:
                    return "Excellent";
                case PerformanceRating.Good:
                    return "Good";
                case PerformanceRating.Medium:
                    return "Medium";
                case PerformanceRating.Bad:
                    return "Poor";
                case PerformanceRating.VeryBad:
                    return "Very Poor";
                default:
                    return "None";
            }
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("AvatarName: {0}\n", AvatarName);
            sb.AppendFormat("Overall Performance: {0}\n", GetPerformanceRatingForCategory(AvatarPerformanceCategory.Overall));
            sb.AppendFormat("PolyCount: {0}\n", PolyCount);
            sb.AppendFormat("Bounds: {0}\n", AABB.ToString());
            sb.AppendFormat("SkinnedMeshCount: {0}\n", SkinnedMeshCount);
            sb.AppendFormat("MeshCount: {0}\n", MeshCount);
            sb.AppendFormat("MaterialCount: {0}\n", MaterialCount);
            sb.AppendFormat("AnimatorCount: {0}\n", AnimatorCount);
            sb.AppendFormat("BoneCount: {0}\n", BoneCount);
            sb.AppendFormat("LightCount: {0}\n", LightCount);
            sb.AppendFormat("ParticleSystemCount: {0}\n", ParticleSystemCount);
            sb.AppendFormat("ParticleTotalCount: {0}\n", ParticleTotalCount);
            sb.AppendFormat("ParticleTotalActiveMeshPolyCount: {0}\n", ParticleTotalActiveMeshPolyCount);
            sb.AppendFormat("ParticleTrailsEnabled: {0}\n", ParticleTrailsEnabled);
            sb.AppendFormat("ParticleCollisionEnabled: {0}\n", ParticleCollisionEnabled);
            sb.AppendFormat("TrailRendererCount: {0}\n", TrailRendererCount);
            sb.AppendFormat("LineRendererCount: {0}\n", LineRendererCount);
            sb.AppendFormat("DynamicBoneComponentCount: {0}\n", DynamicBoneComponentCount);
            sb.AppendFormat("DynamicBoneAffectedTransformCount: {0}\n", DynamicBoneAffectedTransformCount);
            sb.AppendFormat("DynamicBoneColliderCount: {0}\n", DynamicBoneColliderCount);
            sb.AppendFormat(": {0}\n", DynamicBoneCollisionCheckCount);
            sb.AppendFormat("ClothCount: {0}\n", ClothCount);
            sb.AppendFormat("ClothMaxVertices: {0}\n", ClothMaxVertices);
            sb.AppendFormat("PhysicsColliderCount: {0}\n", PhysicsColliderCount);
            sb.AppendFormat("PhysicsRigidbodyCount: {0}\n", PhysicsRigidbodyCount);

            return sb.ToString();
        }

        static bool ApproxLessOrEqual(float x1, float x2)
        {
            float r = x1 - x2;
            return r < 0.0f || Mathf.Approximately(r, 0.0f);
        }
    }

    public static class AvatarPerformance
    {
        static System.Type _dynamicBoneType = null;
        static System.Type _dynamicBoneColliderType = null;
        static System.Reflection.FieldInfo _dynamicBoneRootFieldInfo = null;
        static System.Reflection.FieldInfo _dynamicBoneExclusionsFieldInfo = null;
        static System.Reflection.FieldInfo _dynamicBoneCollidersFieldInfo = null;
        static bool _searchedOptionalTypes = false;

        private readonly static object initLock = new object();

        public static AvatarPerformanceStats CalculatePerformanceStats(string avatarName, GameObject avatarObject)
        {
            AvatarPerformanceStats stats = null;
            IEnumerator enumerator = CalculatePerformanceStatsEnumerator(avatarName, avatarObject, (s) => stats = s);
            while (enumerator.MoveNext()) ;
            return stats;
        }

        public static IEnumerator CalculatePerformanceStatsEnumerator(string avatarName, GameObject avatarObject, System.Action<AvatarPerformanceStats> onDone)
        {
            AvatarPerformanceStats perfStats = new AvatarPerformanceStats();

            lock (initLock)
            {
                if (!_searchedOptionalTypes)
                {
                    FindOptionalTypes();
                    _searchedOptionalTypes = true;
                }
            }

            perfStats.AvatarName = avatarName;

            List<Renderer> _rendererBuffer = new List<Renderer>(16);
            avatarObject.GetComponentsInChildren<Renderer>(_rendererBuffer);

            // polys / bounds
            int polycount;
            Bounds bounds;
            AnalyzeGeometry(avatarObject, _rendererBuffer, out bounds, out polycount);

            perfStats.PolyCount = polycount;
            perfStats.AABB = bounds;

            yield return null;

            // renderers
            AnalyzeRenderers(_rendererBuffer, perfStats);

            yield return null;

            // animators
            {
                int animatorCount = 0;

                List<Animator> _animatorBuffer = new List<Animator>(16);
                avatarObject.GetComponentsInChildren<Animator>(_animatorBuffer);
                animatorCount += _animatorBuffer.Count;

                yield return null;

                List<Animation> _animationBuffer = new List<Animation>(16);
                avatarObject.GetComponentsInChildren<Animation>(_animationBuffer);
                animatorCount += _animationBuffer.Count;

                perfStats.AnimatorCount = animatorCount;
            }

            yield return null;

            // lights
            {
                List<Light> _lightBuffer = new List<Light>(16);
                avatarObject.GetComponentsInChildren<Light>(_lightBuffer);
                perfStats.LightCount = _lightBuffer.Count;
            }

            yield return null;

            // dynamic bone
            AnalyzeDynamicBone(avatarObject,
                out perfStats.DynamicBoneComponentCount,
                out perfStats.DynamicBoneAffectedTransformCount,
                out perfStats.DynamicBoneColliderCount,
                out perfStats.DynamicBoneCollisionCheckCount);

            yield return null;

            // cloth
            {
                int clothVerts = 0;

                List<Cloth> _clothBuffer = new List<Cloth>(16);
                avatarObject.GetComponentsInChildren<Cloth>(_clothBuffer);
                perfStats.ClothCount = _clothBuffer.Count;

                foreach (var c in _clothBuffer)
                {
                    if (c == null)
                        continue;

                    var verts = c.vertices;
                    if (verts == null)
                        continue;

                    clothVerts += verts.Length;
                }

                perfStats.ClothMaxVertices = clothVerts;
            }

            yield return null;

            // physics
            {
                List<Collider> _colliderBuffer = new List<Collider>(16);
                avatarObject.GetComponentsInChildren<Collider>(_colliderBuffer);
                perfStats.PhysicsColliderCount = _colliderBuffer.Count;

                yield return null;

                List<Rigidbody> _rigidbodyBuffer = new List<Rigidbody>(16);
                avatarObject.GetComponentsInChildren<Rigidbody>(_rigidbodyBuffer);
                perfStats.PhysicsRigidbodyCount = _rigidbodyBuffer.Count;
            }

            yield return null;

            // audio 
            {
                List<AudioSource> _audioSourceBuffer = new List<AudioSource>(16);
                avatarObject.GetComponentsInChildren<AudioSource>(_audioSourceBuffer);
                perfStats.AudioSourceCount = _audioSourceBuffer.Count;
            }

            yield return null;

            // cache performance ratings
            perfStats.CalculateAllPerformanceRatings();

            onDone(perfStats);
        }

        static int CountPolygons(Renderer r)
        {
            int result = 0;

            // SkinnedMeshRenderer
            {
                SkinnedMeshRenderer smr = r as SkinnedMeshRenderer;
                if (smr != null)
                {
                    if (smr.sharedMesh == null)
                        return 0;

                    result += CountMeshPolys(smr.sharedMesh);
                }
            }

            // MeshRenderer
            {
                MeshRenderer mr = r as MeshRenderer;
                if (mr != null)
                {
                    var mf = mr.GetComponent<MeshFilter>();
                    if (mf == null || mf.sharedMesh == null)
                        return 0;

                    result += CountMeshPolys(mf.sharedMesh);
                }
            }

            return result;
        }

        static int CountMeshPolys(Mesh sourceMesh)
        {
            bool copiedMesh = false;
            Mesh mesh = null;
            if (sourceMesh.isReadable)
            {
                mesh = sourceMesh;
            }
            else
            {
                mesh = UnityEngine.Object.Instantiate<Mesh>(sourceMesh);
                copiedMesh = true;
            }

            int count = 0;
            for (int i = 0; i < mesh.subMeshCount; ++i)
                count += mesh.GetTriangles(i).Length / 3;

            if (copiedMesh)
                UnityEngine.Object.Destroy(mesh);

            return count;
        }

        static void AnalyzeGeometry(GameObject go, List<Renderer> renderers, out Bounds bounds, out int polycount)
        {
            polycount = 0;
            bounds = new Bounds(go.transform.position, Vector3.zero);

            List<Renderer> _rendererIgnoreBuffer = new List<Renderer>(16);
            List<LODGroup> _lodBuffer = new List<LODGroup>(16);

            go.GetComponentsInChildren<LODGroup>(_lodBuffer);
            foreach (var lod in _lodBuffer)
            {
                LOD[] options = lod.GetLODs();

                int highestLodPolies = 0;
                foreach (LOD l in options)
                {
                    int thisLodPolies = 0;
                    foreach (Renderer r in l.renderers)
                    {
                        _rendererIgnoreBuffer.Add(r);
                        thisLodPolies += CountPolygons(r);
                    }
                    if (thisLodPolies > highestLodPolies)
                        highestLodPolies = thisLodPolies;
                }

                polycount += highestLodPolies;
            }

            foreach (var r in renderers)
            {
                if ((r as ParticleSystemRenderer) == null)
                    bounds.Encapsulate(r.bounds);

                if (_rendererIgnoreBuffer.Contains(r) == false)
                    polycount += CountPolygons(r);
            }

            bounds.center -= go.transform.position;

            _rendererIgnoreBuffer.Clear();
            _lodBuffer.Clear();
        }

        static void AnalyzeRenderers(List<Renderer> renderers, AvatarPerformanceStats perfStats)
        {
            int SkinnedMeshCount = 0;
            int MeshCount = 0;
            int ParticleSystemCount = 0;
            int ParticleTotalCount = 0;
            int ParticleTotalActiveMeshPolyCount = 0;
            bool ParticleTrailsEnabled = false;
            bool ParticleCollisionEnabled = false;
            int TrailRendererCount = 0;
            int LineRendererCount = 0;
            int MaterialCount = 0;
            int BoneCount = 0;

            List<Transform> _transformIgnoreBuffer = new List<Transform>(16);
            List<Material> _materialIgnoreBuffer = new List<Material>(16); 

            foreach (var r in renderers)
            {
                SkinnedMeshRenderer smr = r as SkinnedMeshRenderer;
                if (smr != null)
                {
                    if (smr.sharedMesh == null)
                        continue;

                    SkinnedMeshCount++;

                    // bone count
                    var bones = smr.bones;
                    foreach (var bone in bones)
                    {
                        if (bone == null || _transformIgnoreBuffer.Contains(bone))
                            continue;

                        _transformIgnoreBuffer.Add(bone);
                        BoneCount++;
                    }
                }

                MeshRenderer mr = r as MeshRenderer;
                if (mr != null)
                {
                    var mf = mr.GetComponent<MeshFilter>();
                    if (mf == null || mf.sharedMesh == null)
                        continue;

                    MeshCount++;
                }

                ParticleSystemRenderer pr = r as ParticleSystemRenderer;
                if (pr != null)
                {
                    var ps = pr.GetComponent<ParticleSystem>();

                    int particleCount = ps.main.maxParticles;
                    if (particleCount == 0)
                        continue;

                    ParticleSystemCount++;
                    ParticleTotalCount += particleCount;

                    // mesh particles
                    if (pr.renderMode == ParticleSystemRenderMode.Mesh && pr.meshCount > 0)
                    {
                        int highestPolyCount = 0;

                        Mesh[] meshes = new Mesh[pr.meshCount];
                        int meshCount = pr.GetMeshes(meshes);
                        for (int mi = 0; mi < meshCount; mi++)
                        {
                            var m = meshes[mi];
                            if (m != null)
                            {
                                int polyCount = CountMeshPolys(m);
                                if (polyCount > highestPolyCount)
                                    highestPolyCount = polyCount;
                            }
                        }

                        int maxActivePolys = particleCount * highestPolyCount;
                        ParticleTotalActiveMeshPolyCount += maxActivePolys;
                    }

                    // trail
                    ParticleTrailsEnabled = ParticleTrailsEnabled || ps.trails.enabled;
                    if (ps.trails.enabled)
                    {
                        var m = pr.trailMaterial;
                        if (m != null && !_materialIgnoreBuffer.Contains(m))
                        {
                            _materialIgnoreBuffer.Add(m);
                            MaterialCount++;
                        }
                    }

                    // collision
                    ParticleCollisionEnabled = ParticleCollisionEnabled || ps.collision.enabled;
                }

                TrailRenderer tr = r as TrailRenderer;
                if (tr != null)
                {
                    TrailRendererCount++;
                }

                LineRenderer lr = r as LineRenderer;
                if (lr != null)
                {
                    LineRendererCount++;
                }

                // unique materials count
                for (int i = 0; i < r.sharedMaterials.Length; i++)
                {
                    var m = r.sharedMaterials[i];
                    if (m == null || _materialIgnoreBuffer.Contains(m))
                        continue;

                    _materialIgnoreBuffer.Add(m);
                    MaterialCount++;
                }
            }

            perfStats.SkinnedMeshCount = SkinnedMeshCount;
            perfStats.MeshCount = MeshCount;
            perfStats.ParticleSystemCount = ParticleSystemCount;
            perfStats.ParticleTotalCount = ParticleTotalCount;
            perfStats.ParticleTotalActiveMeshPolyCount = ParticleTotalActiveMeshPolyCount;
            perfStats.ParticleTrailsEnabled = ParticleTrailsEnabled;
            perfStats.ParticleCollisionEnabled = ParticleCollisionEnabled;
            perfStats.TrailRendererCount = TrailRendererCount;
            perfStats.LineRendererCount = LineRendererCount;
            perfStats.MaterialCount = MaterialCount;
            perfStats.BoneCount = BoneCount;
        }

        static void AnalyzeDynamicBone(GameObject avatarObject, out int dynamicBoneComponentCount, out int dynamicBoneAffectedTransformCount, out int dynamicBoneColliderCount, out int dynamicBoneCollisionChecks)
        {
            dynamicBoneComponentCount = 0;
            dynamicBoneAffectedTransformCount = 0;
            dynamicBoneColliderCount = 0;
            dynamicBoneCollisionChecks = 0;

            if (_dynamicBoneType != null)
            {
                Component[] dynamicBones = avatarObject.GetComponentsInChildren(_dynamicBoneType, false);
                dynamicBoneComponentCount = dynamicBones.Length;

                List<object> colliders = new List<object>();

                for (int i = 0; i < dynamicBones.Length; i++)
                {
                    var dynamicBone = dynamicBones[i];

                    int affectedTransforms = 0;

                    Transform root = _dynamicBoneRootFieldInfo.GetValue(dynamicBone) as Transform;
                    if (root != null)
                    {
                        List<Transform> exclusions = _dynamicBoneExclusionsFieldInfo.GetValue(dynamicBone) as List<Transform>;

                        // root can't be excluded
                        exclusions.RemoveAll(t => t == root);

                        // count number of affected transforms in hierarchy
                        affectedTransforms = CountTransformsRecursively(root, exclusions);
                    }

                    int colliderListEntryCount = 0;

                    IList colliderList = _dynamicBoneCollidersFieldInfo.GetValue(dynamicBone) as IList;
                    if (colliderList != null)
                    {
                        colliderListEntryCount = colliderList.Cast<object>().Where(o => o != null).Count();
                        colliders.AddRange(colliderList.Cast<object>().Where(o => o != null));
                    }

                    dynamicBoneAffectedTransformCount += affectedTransforms;
                    dynamicBoneCollisionChecks += affectedTransforms * colliderListEntryCount;
                }

                dynamicBoneColliderCount = colliders.Distinct().Count();
            }
        }

        static void FindOptionalTypes()
        {
            FindDynamicBoneTypes();
        }

        static void FindDynamicBoneTypes()
        {
            if (_dynamicBoneType != null)
                return;

            System.Type dyBoneType = Validation.GetTypeFromName("DynamicBone");
            if (dyBoneType == null)
                return;

            System.Type dyBoneColliderType = Validation.GetTypeFromName("DynamicBoneCollider");
            if (dyBoneColliderType == null)
                return;

            var rootFieldInfo = dyBoneType.GetField("m_Root", BindingFlags.Public | BindingFlags.Instance);
            if (rootFieldInfo == null || rootFieldInfo.FieldType != typeof(Transform))
                return;

            var exclusionsFieldInfo = dyBoneType.GetField("m_Exclusions", BindingFlags.Public | BindingFlags.Instance);
            if (exclusionsFieldInfo == null || exclusionsFieldInfo.FieldType != typeof(List<Transform>))
                return;

            var collidersFieldInfo = dyBoneType.GetField("m_Colliders", BindingFlags.Public | BindingFlags.Instance);
            if (collidersFieldInfo == null || collidersFieldInfo.FieldType.GetGenericTypeDefinition() != typeof(List<>) || collidersFieldInfo.FieldType.GetGenericArguments().Single() != dyBoneColliderType)
                return;

            _dynamicBoneType = dyBoneType;
            _dynamicBoneColliderType = dyBoneColliderType;
            _dynamicBoneRootFieldInfo = rootFieldInfo;
            _dynamicBoneExclusionsFieldInfo = exclusionsFieldInfo;
            _dynamicBoneCollidersFieldInfo = collidersFieldInfo;
        }

        static int CountTransformsRecursively(Transform root, List<Transform> exclusions)
        {
            if (root == null || (exclusions != null && exclusions.Contains(root)))
                return 0;

            int count = 1;
            foreach (Transform child in root)
            {
                count += CountTransformsRecursively(child, exclusions);
            }

            return count;
        }
    }
}
