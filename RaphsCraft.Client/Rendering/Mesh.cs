using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaphsCraft.Client.Rendering;

public class Mesh : IDisposable
{
    private int _vao;
    private int _vbo;
    private int _ebo;
    private float[] vertices;
    private uint[] indices;
    private Texture? t;

    public Mesh(float[] vertices)
    {
        this.vertices = vertices;

        _vbo = GL.GenBuffer();
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
    }

    public Mesh(float[] vertices, uint[] indices, Texture? t = null)
    {
        this.vertices = vertices;
        this.indices = indices;

        _vbo = GL.GenBuffer();
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        if (t != null)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        else
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        if (t != null)
        {
            int texCoordLocation = Shader.DEFAULT_SHADER.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        }
        GL.EnableVertexAttribArray(0);

        _ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
    }


    public void Render()
    {
        t?.Use(TextureUnit.Texture0);

        GL.BindVertexArray(_vao);

        if (_ebo == 0)
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length / 3);
        else
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
    }

    public void Dispose()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.DeleteBuffer(_vbo);
    }
}
