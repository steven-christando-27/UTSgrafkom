﻿using ConsoleApp1;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSgrafkom
{
    internal class Meja: Item
    {
        //untuk animasi
        int counter = 0;
        float degr = -0.2f;
        float degrKaki = -1.2f;
        float scaleAngel = 0.003f;

        float counterAngel = 0;
        int counterKaki = 0;
        int incrementKaki = 1;
        
        int increment = 1;
        float speed = 1f;

        Vector3 pusatTutup;
        List<Asset3d2> listObject = new List<Asset3d2>();
        Asset3d2 tutupTombol, kakiL, kakiR, kabelTombol, kabelTombol2, angel;

        public Meja()
        {
            Vector3 grey = new Vector3(168 / 255f, 212 / 255f, 215 / 255f);
            Vector3 darkGrey = new Vector3(105 / 255f, 142 / 255f, 148 / 255f);
            Vector3 darkGreen = new Vector3(49 / 255f, 206 / 255f, 102 / 255f);
            Vector3 red = new Vector3(255 / 255f, 0 / 255f, 0 / 255f);
            Vector3 darkRed = new Vector3(174 / 255f, 0 / 255f, 0 / 255f);
            Vector3 softYellow = new Vector3(226 / 255f, 198 / 152f, 87 / 255f);
            Vector3 softBlue = new Vector3(194 / 255f, 222 / 255f, 235 / 255f);
            Vector3 orange = new Vector3(248 / 255f, 180 / 255f, 0 / 255f);
            Vector3 darkOrange = new Vector3(247 / 255f, 150 / 255f, 0 / 255f);
            Vector3 cyan = new Vector3(0 / 255f, 255 / 255f, 255 / 255f);


            Asset3d2 alas = new Asset3d2(darkGrey);
            alas.tabung(0f, 0f, 0f, 0.3f, 0.3f, 0.0015f);

            //Alas luar abu-abu
            Asset3d2 tutupAbu = new Asset3d2(grey);
            tutupAbu.createEllipsoid(0, 0, 0, 0.3f, 0.3f, 0f, 30, 30);
            alas.child.Add(tutupAbu);
            
            Asset3d2 kaki1 = new Asset3d2(darkGrey);
            kaki1.tabung(0f, 0f, 0f, 0.05f, 0.05f, 0.01f);
            alas.child.Add(kaki1);
            kaki1.translate(0f, 0.2f, 0f);

            Asset3d2 kaki2 = new Asset3d2(darkGrey);
            kaki2.tabung(0f, 0f, 0f, 0.05f, 0.05f, 0.01f);
            alas.child.Add(kaki2);
            kaki2.translate(0f, -0.2f, 0f);

            Asset3d2 kaki3 = new Asset3d2(darkGrey);
            kaki3.tabung(0f, 0f, 0f, 0.05f, 0.05f, 0.01f);
            alas.child.Add(kaki3);
            kaki3.translate(0.2f, 0f, 0f);

            Asset3d2 kaki4 = new Asset3d2(darkGrey);
            kaki4.tabung(0f, 0f, 0f, 0.05f, 0.05f, 0.01f);
            alas.child.Add(kaki4);
            kaki4.translate(-0.2f, 0f, 0f);

            Asset3d2 tombol = new Asset3d2(darkRed);
            Asset3d2 alasTombol = new Asset3d2(red);
            tombol.tabung(0, 0, 0.02f, 0.03f, 0.03f, 0.001f);
            alasTombol.createEllipsoid(0, 0, 0.02f, 0.03f, 0.03f, 0f, 30, 30);
            alas.child.Add(tombol);
            alas.child.Add(alasTombol);
           
            Asset3d2 bingkaiTombol = new Asset3d2(softYellow);
            bingkaiTombol.createBlock(0, 0, 0, 0.15f, 0.15f, 0);
            bingkaiTombol.rotate(bingkaiTombol.objectCenter, Vector3.UnitZ, 120f);
            alas.child.Add(bingkaiTombol);
            bingkaiTombol.translate(0f, 0f, 0.001f);

            Asset3d2 alasTutupTombol = new Asset3d2(softBlue);
            alasTutupTombol.createBlock(0, 0, 0, 0.11f, 0.11f, 0);
            alasTutupTombol.rotate(alasTutupTombol.objectCenter, Vector3.UnitZ, 120f);
            alas.child.Add(alasTutupTombol);
            alasTutupTombol.translate(0f, 0f, 0.002f);

            tutupTombol = new Asset3d2(softBlue);
            tutupTombol.createBlock2(0, 0, 0, 0.11f, 0.11f, 0.055f);
            tutupTombol.rotate(tutupTombol.objectCenter, Vector3.UnitZ, 120);
            alas.child.Add(tutupTombol);
            tutupTombol.translate(0f, 0f, 0.03f);


            //Rotate supaya terlihat tidur
            alas.rotate(new Vector3(0, 0, 0), Vector3.UnitX, -90f);
            alas.translate(0f, -0.3f, -0.5f);
            listObject.Add(alas);
            alas.rotate(alas.objectCenter, Vector3.UnitY, -30f);

            pusatTutup = tutupTombol.objectCenter;

            //KARAKTER
            Asset3d2 badan = new Asset3d2(orange);
            badan.tabung(0, 0, 0, 0.1f, 0.1f, 0.005f);
            badan.rotate(badan.objectCenter, Vector3.UnitX, 90f);

            Asset3d2 kepala = new Asset3d2(orange);
            kepala.createEllipsoid(0, 0, 0, 0.1f, 0.09f, 0.1f, 30, 30);
            kepala.translate(0f, 0.1f, 0f);

            Asset3d2 pantat = new Asset3d2(orange);
            pantat.createEllipsoid(0, 0, 0, 0.1f, 0.08f, 0.1f, 30, 30);

            Asset3d2 google = new Asset3d2(new Vector3(147 / 255f, 193 / 255f, 213 / 255f));
            google.createEllipsoid(0, 0, 0, 0.06f, 0.03f, 0.01f, 25, 25);
            google.translate(0, 0.1f, 0.1f);

            kakiL = new Asset3d2(darkOrange);
            kakiL.tabung(0, 0, 0, 0.04f, 0.04f, 0.007f);
            kakiL.rotate(kakiL.objectCenter, Vector3.UnitX, -60f);
            kakiL.translate(-0.05f, -0.008f, 0f);

            kakiR = new Asset3d2(darkOrange);
            kakiR.tabung(0, 0, 0, 0.04f, 0.04f, 0.007f);
            kakiR.rotate(kakiR.objectCenter, Vector3.UnitX, -120f);
            kakiR.translate(0.05f, -0.008f, 0.03f);

            Asset3d2 tas = new Asset3d2(darkOrange);
            tas.createBlock(0, 0, 0, 0.18f, 0.18f, 0.07f);
            tas.translate(0, 0.05f, -0.1f);

            angel = new Asset3d2(softYellow);
            angel.createTorus(0, 0, 0, 0.06f, 0.010f, 30, 30);
            angel.translate(0, 0.25f, 0);

            kepala.child.Add(google);
            badan.child.Add(kepala);
            badan.child.Add(pantat);
            badan.child.Add(kakiL);
            badan.child.Add(kakiR);
            badan.child.Add(tas);
            badan.child.Add(angel);

            badan.rotate(badan.objectCenter, Vector3.UnitY, -90f);
            badan.translate(0.6f, -0.3f, -0.5f);
            listObject.Add(badan);


            //Tembok & Lantai
            Asset3d2 tembokBlkgAtas = new Asset3d2(new Vector3(0 / 255f, 247 / 255f, 247 / 255f));
            tembokBlkgAtas.createBlock(0, 0, 0, 2, 0.7f, 0);
            tembokBlkgAtas.translate(0,0.6f,-0.999f);
            listObject.Add(tembokBlkgAtas);

            Asset3d2 tembokBlkgBawah = new Asset3d2(new Vector3(212 / 255f, 0 / 255f, 0 / 255f));
            tembokBlkgBawah.createBlock(0, 0, 0, 2, 0.7f, 0);
            tembokBlkgBawah.translate(0, -0.1f, -0.999f);
            listObject.Add(tembokBlkgBawah);

            Asset3d2 tembokKiriAtas = new Asset3d2(cyan);
            tembokKiriAtas.createBlock(0, 0, 0, 0, 0.7f, 1);
            tembokKiriAtas.translate(-0.999f, 0.6f, -0.5f);
            listObject.Add(tembokKiriAtas);

            Asset3d2 tembokKiriBawah = new Asset3d2(new Vector3(220 / 255f, 0 / 255f, 0 / 255f));
            tembokKiriBawah.createBlock(0, 0, 0, 0, 0.7f, 1);
            tembokKiriBawah.translate(-0.999f, -0.1f, -0.5f);
            listObject.Add(tembokKiriBawah);

            Asset3d2 tembokKananAtas = new Asset3d2(cyan);
            tembokKananAtas.createBlock(0, 0, 0, 0, 0.7f, 1);
            tembokKananAtas.translate(0.999f, 0.6f, -0.5f);
            listObject.Add(tembokKananAtas);

            Asset3d2 tembokKananBawah = new Asset3d2(new Vector3(220 / 255f, 0 / 255f, 0 / 255f));
            tembokKananBawah.createBlock(0, 0, 0, 0, 0.7f, 1);
            tembokKananBawah.translate(0.999f, -0.1f, -0.5f);
            listObject.Add(tembokKananBawah);

            Asset3d2 lantai = new Asset3d2(new Vector3(220 / 255f, 180 / 255f, 100 / 255f));
            lantai.createBlock(0, 0, 0, 2f, 0, 1);
            lantai.translate(0, -0.449f, -0.5f);
            listObject.Add(lantai);


            //Kabel (Curva Berzier)
            kabelTombol = new Asset3d2(new Vector3(red));
            kabelTombol.prepareVertices();
            kabelTombol.setControlCoordinate(0f, 0f, 0f);
            kabelTombol.setControlCoordinate(-0.05f, 0f, 0.1f);
            kabelTombol.setControlCoordinate(-0.2f, 0f, 0.1f);
            kabelTombol.setControlCoordinate(-0.25f, 0f, -0.215f);
            List<Vector3> _verticesBazier = kabelTombol.createCurveBazier();
            kabelTombol.setVertices(_verticesBazier);
            kabelTombol.translate(0, -0.297f, -0.47f);
            listObject.Add(kabelTombol);

            kabelTombol2 = new Asset3d2(new Vector3(red));
            kabelTombol2.prepareVertices();
            kabelTombol2.setControlCoordinate(-0.25f, 0f, -0.215f);
            kabelTombol2.setControlCoordinate(-0.25f, -0.01f, -0.250f);
            kabelTombol2.setControlCoordinate(-0.25f, -0.15f, -0.215f);
            List<Vector3> _verticesBazier2 = kabelTombol2.createCurveBazier();
            kabelTombol2.setVertices(_verticesBazier2);
            kabelTombol2.translate(0, -0.297f, -0.47f);
            listObject.Add(kabelTombol2);
        }

        public void load(int SizeX, int SizeY)
        {
            foreach (Asset3d2 i in listObject)
            {
                i.load(SizeX, SizeY);
            }
        }

        public void Render(Matrix4 cameraView, Matrix4 cameraProjection)
        {
            for (int i = 0; i < listObject.Count; i++)
            {
                listObject[i].render(cameraView, cameraProjection);
            }

            //Animasi tutup tombol
            Vector3 temp = new Vector3(0, 0.02f, 0.011f);
            tutupTombol.rotate(pusatTutup - temp, Vector3.UnitX, degr);
            counter += increment;

            if (counter > 400)
            {
                degr *= -1;
                increment *= -1;
                
            }

            if (counter < 0)
            {
                degr *= -1;
                increment *= -1;
            }

            //Animasi kaki
            kakiR.rotate(kakiR.objectCenter, Vector3.UnitZ, degrKaki);;
            kakiL.rotate(kakiL.objectCenter, Vector3.UnitZ, -degrKaki);
            counterKaki += incrementKaki;
            if(counterKaki <= -80 || counterKaki >= 20){
                degrKaki *= -1;
                incrementKaki *= -1;
            }

            //Animasi angel
            angel.scale(1-scaleAngel, 1, 1 - scaleAngel);
            counterAngel += scaleAngel;
            if(counterAngel >= 0.2f || counterAngel <= -0.2f)
            {
                scaleAngel *= -1;
            }

        }
    }
}