using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaphsCraft.Client.Rendering;

/// <summary>
/// Renders the game. lol.
/// </summary>
public class GameRenderer
{
    public static unsafe void RenderMesh(Mesh m)
    {
        if (m == null) return;

        var gl = Game.Instance.GetGL();
        gl.BindVertexArray(m.GetVertexArrayObject());
        gl.DrawElements(Silk.NET.OpenGL.PrimitiveType.Triangles, (uint)m.indices.Length, Silk.NET.OpenGL.DrawElementsType.UnsignedInt, (void*)0);
    }

    public static unsafe void RenderMesh(Mesh m, PrimitiveType type)
    {
        if (m == null) return;

        var gl = Game.Instance.GetGL();
        gl.BindVertexArray(m.GetVertexArrayObject());
        gl.DrawElements(type, (uint)m.indices.Length, Silk.NET.OpenGL.DrawElementsType.UnsignedInt, (void*)0);
    }
}
