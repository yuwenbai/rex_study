using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    // Adds a mesh collider to each game object that contains collider in its name
    public class MeshPostprocessor : AssetPostprocessor
    {
        public void OnPreprocessModel()
        {
            var modelImporter = assetImporter as ModelImporter;
            modelImporter.addCollider = false;
            modelImporter.animationCompression = ModelImporterAnimationCompression.KeyframeReduction;
            modelImporter.generateAnimations = ModelImporterGenerateAnimations.None;
            modelImporter.importAnimation = false;
            modelImporter.importMaterials = false;
            modelImporter.importTangents = ModelImporterTangents.None;
            modelImporter.globalScale = 1f;
            modelImporter.isReadable = true;
            modelImporter.meshCompression = ModelImporterMeshCompression.Low;
            modelImporter.optimizeMesh = true;
        }
        void OnPostprocessModel(GameObject g)
        {
            Apply(g.transform);
        }

        void Apply(Transform t)
        {
            if (t.name.ToLower().Contains("collider"))
            {
                t.gameObject.AddComponent<CapsuleCollider>();
            }
                

            // Recurse
            foreach (Transform child in t)
                Apply(child);
        }
    }
}
