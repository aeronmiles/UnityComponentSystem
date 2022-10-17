using System.Collections;
using System.Collections.Generic;
using AM.Unity.Component.System;
using UnityEngine;

[DisallowMultipleComponent]
public class MeshAlignComponent : EntityComponent
{
    public MeshFilter MeshFilter;
    public Transform AlignTransform;
    public bool AlignToNormal = false;
    public List<int> VertexIndicies = new();

    Transform m_Transform;
    private new void Awake()
    {
        base.Awake();
        m_Transform = transform;
    }

    Vector3[] m_Positions;
    Vector3[] m_Normals;
    public void Update()
    {
        int vCount = VertexIndicies.Count;
        if (MeshFilter == null || vCount == 0) return;

        Vector3 pos = Vector3.zero;
        Vector3 normal = Vector3.zero;
        Matrix4x4 localToWorld = MeshFilter.transform.localToWorldMatrix;
        m_Positions = MeshFilter.mesh.vertices;
        m_Normals = MeshFilter.mesh.normals;
        foreach (var i in VertexIndicies)
        {
            pos += localToWorld.MultiplyPoint3x4(m_Positions[i]);
            normal += localToWorld.MultiplyVector(m_Normals[i]);
        }

        m_Transform.position = pos / vCount;
        if (AlignTransform != null)
            m_Transform.rotation = AlignTransform.rotation;
        else if (AlignToNormal)
            m_Transform.rotation = Quaternion.FromToRotation(Vector3.up, normal / vCount);
    }
}
