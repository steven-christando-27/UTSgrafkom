﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
namespace UTSgrafkom
{
    static class Constants
    {
        public const string path = "../../../shader/";
    }
    internal class Windows : GameWindow
    {
<<<<<<< scan
        List<Item> listObject = new List<Item>();

        Camera camera;
        bool _firstMove = true;
        Vector2 _lastPos;
        Vector3 _objectPos = new Vector3(0, 0, 0);
        float _rotationSpeed = 1f;

=======
        float degr = 0;
        double time;
        Karakter karakterVent,karakterReactor;
        reactor reaktor1;
        ruangan ruang;
        Camera camera;
        vent coba1,coba2;
>>>>>>> master
        public Windows(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            
        }

        protected override void OnLoad()
        {

            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
<<<<<<< scan

            Karakter karakter = new Karakter(0f, 0f, 0f, new Vector3(0, 0.5f, 1));
            karakter.load(Size.X,Size.Y);
            

            //SCANNER
            Scanner scanner = new Scanner();
            scanner.load(Size.X, Size.Y);

            listObject.Add(scanner);
=======
            
            ruang = new ruangan(new Vector3(0, 0.5f, 1));
            ruang.load(Size.X, Size.Y);

            reaktor1 = new reactor(new Vector3(0, 0.5f, 1));
            reaktor1.load(Size.X, Size.Y);

            coba1 = new vent(new Vector3(0, 0, 0));
            coba1.alasVent();
            coba1.load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);

            coba2 = new vent(new Vector3(0, 0, 0));
            coba2.tutupVent();
            coba2.load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);

            karakterVent = new Karakter(0f, 0f, 0f, new Vector3(0, 0.5f, 1));
            karakterVent.karakterVent();
            karakterVent.load(Size.X, Size.Y);

            karakterReactor = new Karakter(0, 0, 0, new Vector3(0.5f, 0.5f,0));
            karakterReactor.karakterReaktor();
            karakterReactor.load(Size.X, Size.Y);
>>>>>>> master

            camera = new Camera(new Vector3(0, 0, 1), Size.X / (float)Size.Y);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
<<<<<<< scan
            /*GL.Clear(ClearBufferMask.ColorBufferBit);*/
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            //SCANNER
            foreach (Item i in listObject)
            {
                i.render(camera.GetViewMatrix(), camera.GetProjectionMatrix());
            }

=======
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            time += 15.0 * time;
            Matrix4 temp = Matrix4.Identity;
            degr = MathHelper.DegreesToRadians(0.5f);
            temp *= Matrix4.CreateRotationX(degr);
            ruang.render(1, temp, time, camera.GetViewMatrix(), camera.GetProjectionMatrix());
            reaktor1.render(args.Time, temp, camera.GetViewMatrix(), camera.GetProjectionMatrix());
            coba1.render(1, temp, args.Time, camera.GetViewMatrix(), camera.GetProjectionMatrix());
            coba2.render(1, temp, args.Time, camera.GetViewMatrix(), camera.GetProjectionMatrix());
            karakterVent.render(args.Time, temp, camera.GetViewMatrix(), camera.GetProjectionMatrix());
            karakterReactor.render(args.Time, temp, camera.GetViewMatrix(), camera.GetProjectionMatrix());
>>>>>>> master
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            var input = KeyboardState;
            float cameraSpeed = 0.5f;
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            if (input.IsKeyDown(Keys.W))
            {
                camera.Position += camera.Front * cameraSpeed * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.A))
            {
                camera.Position -= camera.Right * cameraSpeed * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.S))
            {
                camera.Position -= camera.Front * cameraSpeed * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.D))
            {
                camera.Position += camera.Right * cameraSpeed * (float)args.Time;
            }

            if (input.IsKeyDown(Keys.Space))
            {
                camera.Position += camera.Up * cameraSpeed * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.LeftControl))
            {
                camera.Position -= camera.Up * cameraSpeed * (float)args.Time;
            }

            if (input.IsKeyDown(Keys.Right))
            {
                camera.Yaw += cameraSpeed * (float)args.Time * 30.0f;
            }
            if (input.IsKeyDown(Keys.Left))
            {
                camera.Yaw -= cameraSpeed * (float)args.Time * 30.0f;
            }
            if (input.IsKeyDown(Keys.Up))
            {
                camera.Pitch += cameraSpeed * (float)args.Time * 30.0f;
            }
            if (input.IsKeyDown(Keys.Down))
            {
                camera.Pitch -= cameraSpeed * (float)args.Time * 30.0f;
            }

            var mouse = MouseState;
            var sesitivity = 0.2f;
            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;

                _lastPos = new Vector2(mouse.X, mouse.Y);
                camera.Yaw += deltaX * sesitivity;
                camera.Pitch += deltaY * sesitivity;
            }

            if (KeyboardState.IsKeyDown(Keys.N))
            {
                var axis = new Vector3(0, 1, 0);
                camera.Position -= _objectPos;
                camera.Position = Vector3.Transform(
                    camera.Position,
                    generateArbRotationMatrix(axis, _rotationSpeed).ExtractRotation());

                camera.Position += _objectPos;
                camera._front = -Vector3.Normalize(camera.Position - _objectPos);
            }
        }

        public Matrix4 generateArbRotationMatrix(Vector3 axis, float angle)
        {
            angle = MathHelper.DegreesToRadians(angle);

            var arbRotationMatrix = new Matrix4(
                (float)Math.Cos(angle) + (float)Math.Pow(axis.X, 2) * (1 - (float)Math.Cos(angle)), axis.X * axis.Y * (1 - (float)Math.Cos(angle)) - axis.Z * (float)Math.Sin(angle), axis.X * axis.Z * (1 - (float)Math.Cos(angle)) + axis.Y * (float)Math.Sin(angle), 0,
                axis.Y * axis.X * (1 - (float)Math.Cos(angle)) + axis.Z * (float)Math.Sin(angle), (float)Math.Cos(angle) + (float)Math.Pow(axis.Y, 2) * (1 - (float)Math.Cos(angle)), axis.Y * axis.Z * (1 - (float)Math.Cos(angle)) - axis.X * (float)Math.Sin(angle), 0,
                axis.Z * axis.X * (1 - (float)Math.Cos(angle)) - axis.Y * (float)Math.Sin(angle), axis.Z * axis.Y * (1 - (float)Math.Cos(angle)) + axis.X * (float)Math.Sin(angle), (float)Math.Cos(angle) + (float)Math.Pow(axis.Z, 2) * (1 - (float)Math.Cos(angle)), 0,
                0, 0, 0, 1
                );

            return arbRotationMatrix;
        }
    }
}
