using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RaphsCraft.Client.Gui;
using RaphsCraft.Client.Rendering;
using RaphsCraft.Client.Resource;
using System.Globalization;

namespace RaphsCraft.Client
{
    public class Game : GameWindow
    {
        public Logger.Logger Logger { get; set; }
        public Mesh m;
        public Camera _camera;
        public static Game Instance { get; private set; }
        double _time;
        private bool _firstMove = true;
        private Vector2 _lastPos;
        private ImGuiController imgui;

        public Game() : base(new GameWindowSettings(), new NativeWindowSettings() { ClientSize = (800, 600), Title = "Raph's Craft" })
        {
            Thread.CurrentThread.Name = "Render thread";
            Logger = new();
            Logger.Name = "game";
            Logger.Log(Client.Logger.LogType.Info, "Raph's Craft 0.0.1 (Raph's Core 0.0.1)");

            Instance = this;

            Run();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            imgui = new(ClientSize.X, ClientSize.Y);

            GL.Enable(EnableCap.DepthTest); // Activer le test de profondeur

            m = new(new float[] {
                // Positions          // Texture Coords
                // Front face
                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
                -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
                // Back face
                -0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
                 0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
                 0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
                -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                // Left face
                -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                // Right face
                 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                 0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                 0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                // Top face
                -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
                 0.5f,  0.5f, -0.5f,  1.0f, 0.0f,
                -0.5f,  0.5f, -0.5f,  0.0f, 0.0f,
                // Bottom face
                -0.5f, -0.5f,  0.5f,  0.0f, 1.0f,
                 0.5f, -0.5f,  0.5f,  1.0f, 1.0f,
                 0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 0.0f
            }, new uint[] {
                // Front face
                0, 1, 2,
                2, 3, 0,
                // Back face
                4, 5, 6,
                6, 7, 4,
                // Left face
                8, 9, 10,
                10, 11, 8,
                // Right face
                12, 13, 14,
                14, 15, 12,
                // Top face
                16, 17, 18,
                18, 19, 16,
                // Bottom face
                20, 21, 22,
                22, 23, 20
            }, Texture.LoadFromFile(ResourceManager.GetPath(ResourceType.Textures, "raphscraft", "unknown.png")));

            //CursorState = CursorState.Grabbed;

            _camera = new(new(0f), Size.X / (float)Size.Y);
            _camera.Fov = 110.0f;
        }


        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);

            imgui.WindowResized(e.Width, e.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Logger.Log(Client.Logger.LogType.Info, "Exiting...");
                Close();
            }

            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
            }

            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
            }

            // Get the mouse state
            var mouse = MouseState;

            if (_firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            accumulatedTime += (float)e.Time;
            frameCount++;

            if (accumulatedTime >= 0.1f) // 100ms
            {
                float averageFrameTime = accumulatedTime / frameCount;
                UpdateFrameTimes(averageFrameTime);

                accumulatedTime = 0.0f;
                frameCount = 0;
            }

            imgui.Update(this, (float)e.Time);

            _time += 4.0 * e.Time;

            GL.ClearColor(0.396f, 0.239f, 0.839f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Shader.DEFAULT_SHADER.Use();
            var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time));
            Shader.DEFAULT_SHADER.SetMatrix4("model", model);
            Shader.DEFAULT_SHADER.SetMatrix4("view", _camera.GetViewMatrix());
            Shader.DEFAULT_SHADER.SetMatrix4("projection", _camera.GetProjectionMatrix());
            m.Render();

            ImGui.Begin("FPS");
            ImGui.PlotLines("Samples", ref _frameTimes.ToArray()[0], _frameTimes.Length);

            ImGui.Text($"FPS: {1.0f / e.Time:0.0}");
            ImGui.End();
            imgui.Render();

            ImGuiController.CheckGLError("End of frame");

            SwapBuffers();
        }

        private float accumulatedTime = 0.0f;
        private int frameCount = 0;
        private float[] _frameTimes = new float[100] ;
        private int _frameTimeIndex = 0;

        private void UpdateFrameTimes(float frameTime)
        {
            _frameTimes[_frameTimeIndex] = frameTime;
            _frameTimeIndex = (_frameTimeIndex + 1) % _frameTimes.Length;
        }


    }
}
