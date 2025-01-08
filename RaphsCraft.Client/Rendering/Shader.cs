﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RaphsCraft.Client.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaphsCraft.Client.Rendering;

public class Shader : IDisposable
{
    int Handle;
    int VertexShader;
    int FragmentShader;

    private readonly Dictionary<string, int> _uniformLocations;
    public static Shader DEFAULT_SHADER { get; private set; }

    static Shader() {
        DEFAULT_SHADER = new(ResourceManager.GetString(ResourceType.Shaders, "raphscraft", "DefaultVertex.v4"),
                    ResourceManager.GetString(ResourceType.Shaders, "raphscraft", "DefaultFragment.f4"));
    }

    ~Shader()
    {
        if (disposedValue == false)
        {
            Game.Instance.Logger.Log(Logger.LogType.Warning, "GPU resource leak! (pls call Shader.Dispose)");
        }
    }

    public Shader(string vertex, string frag)
    {
        VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader, vertex);

        FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader, frag);

        GL.CompileShader(VertexShader);

        GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(VertexShader);
            Console.WriteLine(infoLog);
        }

        GL.CompileShader(FragmentShader);

        GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(FragmentShader);
            Console.WriteLine(infoLog);
        }

        Handle = GL.CreateProgram();

        GL.AttachShader(Handle, VertexShader);
        GL.AttachShader(Handle, FragmentShader);

        GL.LinkProgram(Handle);

        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(Handle);
            Console.WriteLine(infoLog);
        }

        // The individial shader files isn't necessary anymore, as they are in the final program.
        GL.DetachShader(Handle, VertexShader);
        GL.DetachShader(Handle, FragmentShader);
        GL.DeleteShader(FragmentShader);
        GL.DeleteShader(VertexShader);


        GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

        // Next, allocate the dictionary to hold the locations.
        _uniformLocations = new Dictionary<string, int>();

        // Loop over all the uniforms,
        for (var i = 0; i < numberOfUniforms; i++)
        {
            // get the name of this uniform,
            var key = GL.GetActiveUniform(Handle, i, out _, out _);

            // get the location,
            var location = GL.GetUniformLocation(Handle, key);

            // and then add it to the dictionary.
            _uniformLocations.Add(key, location);
        }
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(Handle, attribName);
    }

    public void SetInt(string name, int data)
    {
        GL.UseProgram(Handle);
        GL.Uniform1(_uniformLocations[name], data);
    }

    public void SetFloat(string name, float data)
    {
        GL.UseProgram(Handle);
        GL.Uniform1(_uniformLocations[name], data);
    }

    public void SetMatrix4(string name, Matrix4 data)
    {
        GL.UseProgram(Handle);
        GL.UniformMatrix4(_uniformLocations[name], true, ref data);
    }

    public void SetVector3(string name, Vector3 data)
    {
        GL.UseProgram(Handle);
        GL.Uniform3(_uniformLocations[name], data);
    }

    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            GL.DeleteProgram(Handle);

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
