using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class PlayerVisual : MonoBehaviour
    {
        private Material material;

        [SerializeField] private MeshRenderer headMeshRender;
        [SerializeField] private MeshRenderer bodyMeshRender;

        private void Awake()
        {
            //克隆材质
            material = new Material(headMeshRender.material);

            headMeshRender.material = material;
            bodyMeshRender.material = material;
        }

        public void SetColor(Color color)
        {
            material.color = color;
        }
    }
}
