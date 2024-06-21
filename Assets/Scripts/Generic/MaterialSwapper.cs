using System;
using UnityEngine;

namespace Generic
{
    public class MaterialSwapper : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Material[] materials;
        [SerializeField] private float swapInterval;
        private float swapTimer;
        private int currentMaterialIndex;

        private void Update()
        {
            swapTimer += Time.deltaTime;
            if (!(swapTimer >= swapInterval)) return;
            SwapMaterial();
            swapTimer = 0;
        }

        private void SwapMaterial()
        {
            currentMaterialIndex = (currentMaterialIndex + 1) % materials.Length;
            meshRenderer.material = materials[currentMaterialIndex];
        }
    }
}