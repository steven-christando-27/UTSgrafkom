﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace UTSgrafkom
{
    internal class Karakter : Asset3d
    {
        float degr = 0;
        double time;
        List<Asset3d> listObject = new List<Asset3d>();
        Asset3d kepala,pantat,badan,google,kakiKanan,kakiKiri;
        Vector3 color;
        float x, y, z;
        static class Constants
        {
            public const string path = "D:../../../shader/";
        }
        public Karakter(float x, float y, float z, Vector3 color) : base(color)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.color = color;
        }

        public void karakterVent()
        {
            badan = new Asset3d(new Vector3(this.color));
            badan.tabung(0, 0, 0, 0.05f, 0.05f, 0.005f);
            badan.rotate(badan.objectCenter, Vector3.UnitX, 90f);
            kepala = new Asset3d(new Vector3(this.color));
            kepala.createSphere(0, 0.1f, 0, 0.05f, 25, 25);
            google = new Asset3d(new Vector3(0.5f, 0.5f, 0.5f));
            google.createEllipsoid(-0.0375f, 0.1f, 0, 0.025f, 0.01f, 0.025f, 25, 25);
            badan.child.Add(kepala);
            kepala.child.Add(google);
            kepala.rotate(kepala.objectCenter, Vector3.UnitY, 75f);
            badan.translate(-1.25f, -0.4255f, -0.9f);
            listObject.Add(kepala);
            listObject.Add(badan);
            listObject.Add(google);
        }

        public void karakterReaktor()
        {
            kepala = new Asset3d(this.color);
            kepala.createSphere(0, -0.1f, 0, 0.1f, 25, 25);

            pantat = new Asset3d(this.color);
            pantat.createSphere(0, -0.3f, 0, 0.1f, 25, 25);
            
            badan = new Asset3d(this.color);
            badan.tabung(0, -0.3f, 0, 0.1f, 0.1f, 0.01f);
            badan.rotate(badan.objectCenter, Vector3.UnitX, 90f);

            google = new Asset3d(new Vector3(0.5f,0.5f,0.5f));
            google.createEllipsoid(-0.1f, -0.1f, 0, 0.025f, 0.025f, 0.04f, 25, 25);

            badan.child.Add(kepala);
            kepala.child.Add(google);
            kepala.rotate(kepala.objectCenter, Vector3.UnitY, 45f);
            badan.child.Add(pantat);
            badan.translate(-1.7f, 0, -0.5f);

            listObject.Add(badan);
            listObject.Add(kepala);
            listObject.Add(pantat);
            listObject.Add(google);
        }

        public void karakterScanner()
        {
            
        }

        public void mayatKarakter()
        {

        }

        public void load(float SizeX,float SizeY)
        {
            foreach (Asset3d i in listObject)
            {
                i.load(Constants.path + "shader.vert", Constants.path + "shader.frag", SizeX, SizeY);
                
            }
        }

        public void render(double times, Matrix4 temps, Matrix4 cameraView, Matrix4 cameraProjection)
        {
            time += 15.0 * times;
            Matrix4 temp = Matrix4.Identity;
            for (int i = 0; i < listObject.Count; i++)
            {
                listObject[i].render(1, temp, time, cameraView, cameraProjection);
            }
        }

    }
}
