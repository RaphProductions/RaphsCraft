using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RaphsCraft.Client.Rendering;

/// <summary>
/// Uses OpenGL immediate mode styled-functions to create a mesh, that can be rendered to the scene using GameRenderer.RenderMesh()
/// </summary>
/// <remarks>This should be a singleton.</remarks>
public class MeshBuilder
{
    List<float> vertices;
    List<uint> indices;

    public MeshBuilder()
    {
        vertices = new List<float>();
        indices = new List<uint>();
    }

    public void Begin()
    {
        vertices.Clear();
        indices.Clear();
    }

    public void AddVertex3D(Vector3 pos)
    {
        vertices.Add(pos.X);
        vertices.Add(pos.Y);
        vertices.Add(pos.Z);
    }

    public void AddIndice(uint i)
    {
        this.indices.Add(i);
    }

    public Mesh End()
    {
        return new Mesh(
            vertices.ToArray(),
            indices.ToArray());
    }
}