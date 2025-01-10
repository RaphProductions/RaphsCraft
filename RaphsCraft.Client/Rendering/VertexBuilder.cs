using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RaphsCraft.Client.Rendering;

/// <summary>
/// OpenGL Immediate Mode Simulator:tm:
/// </summary>
/// <remarks>This may be slow. Please use MeshBuilder instead.</remarks>
public class VertexBuilder
{
    List<float> vertices;
    List<uint> indices;
    uint _vbo;
    uint _vao;
    uint _ebo;

    public VertexBuilder()
    {
        vertices = new List<float>();
        indices = new List<uint>();
    }

    public void Begin()
    {
        vertices.Clear();
        indices.Clear();

        var gl = Game.Instance.GetGL();
        _vbo = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);

        _vao = gl.GenVertexArray();
        gl.BindVertexArray(_vao);

        _ebo = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
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

    public unsafe void End()
    {
        var gl = Game.Instance.GetGL();
        fixed (float* buf = vertices.ToArray())
            gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Count * sizeof(float)), buf, BufferUsageARB.StaticDraw);

        fixed (uint* ind = indices.ToArray())
            gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(indices.Count * sizeof(uint)), ind, BufferUsageARB.StaticDraw);

        const uint positionLoc = 0;
        gl.EnableVertexAttribArray(positionLoc);
        gl.VertexAttribPointer(positionLoc, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), (void*)0);

        gl.BindVertexArray(0);
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);

        gl.BindVertexArray(_vao);
        gl.DrawElements(PrimitiveType.Triangles, (uint)indices.Count, DrawElementsType.UnsignedInt, (void*)0);
        gl.BindVertexArray(0);
    }
}
