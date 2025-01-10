
using RaphsCraft.Client.Rendering;
using RaphsCraft.Client.Resource;
using Silk.NET.Core;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using StbImageSharp;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace RaphsCraft.Client
{
    public class Game
    {
        public Logger.Logger Logger { get; private set; }
        public static Game Instance { get; private set; }
        public IWindow Window { get; private set; }
        public MeshBuilder MeshBuilder { get; private set; }
        Mesh m;
        double fpsDisplayerTime = 0f;


        public Game()
        {
            Thread.CurrentThread.Name = "Render thread";
            Logger = new();
            Logger.Name = "game";
            Logger.Log(Client.Logger.LogType.Info, "Raph's Craft 0.0.1 (Raph's Core 0.0.1)");

            Instance = this;

            WindowOptions options = WindowOptions.Default with
            {
                Size = new Vector2D<int>(800, 600),
                Title = "Raph's Craft (OpenGL)",
                TransparentFramebuffer = true,
                VSync = false
            };
            Window = Silk.NET.Windowing.Window.Create(options);

            Window.Load += OnLoad;
            Window.Update += OnUpdate;
            Window.Render += OnRender;
            Window.FramebufferResize += OnFramebufferResize;
            Window.Run();
        }

        private void OnFramebufferResize(Vector2D<int> size)
        {
            GL gl = GetGL();
            gl.Viewport(size);
        }

        private RawImage LoadIcon()
        {
            //StbImage.stbi_set_flip_vertically_on_load(1);

            ImageResult ir = ImageResult.FromMemory(
                ResourceManager.Get(ResourceType.Textures, "raphscraft", "blocks/grass_side.png"), ColorComponents.RedGreenBlueAlpha);

            RawImage ri = new(ir.Width, ir.Height, new Memory<byte>(ir.Data));
            return ri;
        }

        private void OnLoad()
        {
            MeshBuilder = new();
            MeshBuilder.Begin();
            MeshBuilder.AddVertex3D(new(-0.5f, -0.5f, 0f));
            MeshBuilder.AddVertex3D(new(0f, 0.5f, 0f));
            MeshBuilder.AddVertex3D(new(0.5f, -0.5f, 0f));
            MeshBuilder.AddIndice(0);
            MeshBuilder.AddIndice(1);
            MeshBuilder.AddIndice(2);
            m = MeshBuilder.End();

            RawImage ri = LoadIcon();
            Window.SetWindowIcon(ref ri);

            IInputContext input = Window.CreateInput();
            for (int i = 0; i < input.Keyboards.Count; i++)
                input.Keyboards[i].KeyDown += OnKeyDown;
        }

        private void OnKeyDown(IKeyboard keyboard, Key key, int keyCode)
        {

        }

        private void OnUpdate(double deltaTime) 
        {
        }

        private void OnRender(double deltaTime)
        {
            double fps = 1.0 / deltaTime;
            Window.Title = "Raph's Craft (OGL: " + (int)fps + " FPS)";

            var gl = GetGL();
            gl.ClearColor(Color.FromArgb(128, 0, 128, 0));
            gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GameRenderer.RenderMesh(m);
        }

        public GL GetGL() => GL.GetApi(Window);
    }
}
