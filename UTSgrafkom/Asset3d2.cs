﻿using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Asset3d2
    {
        private readonly string path = "../../../shader/";

        public List<Vector3> vertices = new List<Vector3>();
        private List<uint> indices = new List<uint>();

        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;

        private Shader _shader;

        private Matrix4 model = Matrix4.Identity;   // Model Matrix      ==> Matrix ini yang akan berubah saat terjadi transformasi
        private Matrix4 view;                       // View Matrix       ==> Matrix ini menentukan arah pandang 'kamera'
        private Matrix4 projection;                 // Projection Matrix ==> Matrix ini menentukan jenis projection, kamera game cenderung menggunakan kamera perspective.

        private Vector3 color;                      // Warna objek, dikirim ke shader lewat uniform.

        public List<Vector3> _euler = new List<Vector3>();  // Sudut lokal, relatif terhadap objek yang bersangkutan.
        public Vector3 objectCenter = Vector3.Zero;         // Titik tengah objek

        public List<Asset3d2> child = new List<Asset3d2>();   // Sistem Hierarchical Object ==> List untuk menampung objek - objek child.
        
        //bezier
        private int index;
        private float[] verticesCurve;
        private int[] _pascal;

        public Asset3d2(Vector3 color)
        {
            this.color = color;
            _euler.Add(Vector3.UnitX); // Masukkan sudut Euler di Constructor.
            _euler.Add(Vector3.UnitY);
            _euler.Add(Vector3.UnitZ);
        }

        public void load(int sizeX, int sizeY)
        {
            _vertexBufferObject = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            if (indices.Count != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), BufferUsageHint.StaticDraw);
            }

            view = Matrix4.CreateTranslation(0, 0, -8.0f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), sizeX / (float)sizeY, 0.01f, 100f);

            _shader = new Shader(path + "shader.vert", path + "shader.frag");
            _shader.Use();

            foreach (var i in child)
            {
                i.load(sizeX, sizeY);
            }
        }

        public void render(Matrix4 cameraView, Matrix4 cameraProjection)
        {
            _shader.Use();
            GL.BindVertexArray(_vertexArrayObject);

            _shader.SetVector3("ourColor", color);

            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", cameraView);
            _shader.SetMatrix4("projection", cameraProjection);

            if (indices.Count != 0)
            {
                GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                GL.DrawArrays(PrimitiveType.LineStrip, 0, vertices.Count);
            }

            foreach (var i in child)
            {
                i.render(cameraView, cameraProjection);
            }
        }

        /// <summary>
        /// Berfungsi untuk me-reset sudut euler (sudut relatif terhadap objek)
        /// </summary>
        public void resetEuler()
        {
            _euler.Clear();
            _euler.Add(Vector3.UnitX);
            _euler.Add(Vector3.UnitY);
            _euler.Add(Vector3.UnitZ);
        }

        #region solidObjects

        public void createCuboid(float x_, float y_, float z_, float length)
        {
            var tempVertices = new List<Vector3>();
            Vector3 temp_vector;

            //Titik 1
            temp_vector.X = x_ - length / 8.0f;
            temp_vector.Y = y_ + length / 8.0f;
            temp_vector.Z = z_ - length / 8.0f;
            tempVertices.Add(temp_vector);

            //Titik 2
            temp_vector.X = x_ + length / 8.0f;
            temp_vector.Y = y_ + length / 8.0f;
            temp_vector.Z = z_ - length / 8.0f;
            tempVertices.Add(temp_vector);

            //Titik 3
            temp_vector.X = x_ - length / 8.0f;
            temp_vector.Y = y_ - length / 8.0f;
            temp_vector.Z = z_ - length / 8.0f;
            tempVertices.Add(temp_vector);

            //Titik 4
            temp_vector.X = x_ + length / 8.0f;
            temp_vector.Y = y_ - length / 8.0f;
            temp_vector.Z = z_ - length / 8.0f;
            tempVertices.Add(temp_vector);

            //Titik 5
            temp_vector.X = x_ - length / 8.0f;
            temp_vector.Y = y_ + length / 8.0f;
            temp_vector.Z = z_ + length / 8.0f;
            tempVertices.Add(temp_vector);

            //Titik 6
            temp_vector.X = x_ + length / 8.0f;
            temp_vector.Y = y_ + length / 8.0f;
            temp_vector.Z = z_ + length / 8.0f;
            tempVertices.Add(temp_vector);

            //Titik 7
            temp_vector.X = x_ - length / 8.0f;
            temp_vector.Y = y_ - length / 8.0f;
            temp_vector.Z = z_ + length / 8.0f;
            tempVertices.Add(temp_vector);

            //Titik 8
            temp_vector.X = x_ + length / 8.0f;
            temp_vector.Y = y_ - length / 8.0f;
            temp_vector.Z = z_ + length / 8.0f;
            tempVertices.Add(temp_vector);

            var tempIndices = new List<uint>
            {
				//Back
				1, 2, 0,
                2, 1, 3,
				
				//Top
				5, 0, 4,
                0, 5, 1,

				//Right
				5, 3, 1,
                3, 5, 7,

				//Left
				0, 6, 4,
                6, 0, 2,

				//Front
				4, 7, 5,
                7, 4, 6,

				//Bottom
				3, 6, 2,
                6, 3, 7
            };
            vertices = tempVertices;
            indices = tempIndices;
        }

        public void createBlock(float x_, float y_, float z_, float length, float height, float depth)
        {
            var tempVertices = new List<Vector3>();
            objectCenter = new Vector3(x_, y_, z_);
            Vector3 temp_vector;

            //Titik 1
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + height / 2.0f;
            temp_vector.Z = z_ - depth / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 2
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + height / 2.0f;
            temp_vector.Z = z_ - depth / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 3
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - height / 2.0f;
            temp_vector.Z = z_ - depth / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 4
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - height / 2.0f;
            temp_vector.Z = z_ - depth / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 5
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + height / 2.0f;
            temp_vector.Z = z_ + depth / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 6
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + height / 2.0f;
            temp_vector.Z = z_ + depth / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 7
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - height / 2.0f;
            temp_vector.Z = z_ + depth / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 8
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - height / 2.0f;
            temp_vector.Z = z_ + depth / 2.0f;
            tempVertices.Add(temp_vector);

            var tempIndices = new List<uint>
            {
                //Back
                1, 2, 0,
                2, 1, 3,
                //Top
                5, 0, 4,
                0, 5, 1,
                //Right
                5, 3, 1,
                3, 5, 7,
                //Left
                0, 6, 4,
                6, 0, 2,
                //Front
                4, 7, 5,
                7, 4, 6,
                //Bottom
                3, 6, 2,
                6, 3, 7
            };
            vertices = tempVertices;
            indices = tempIndices;
        }


        public void createBlock2(float x_, float y_, float z_, float length, float height, float depth)
        {
            var tempVertices = new List<Vector3>();
            objectCenter = new Vector3(x_, y_, z_);
            Vector3 temp_vector;

            //Titik 1
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + height / 2.0f;
            temp_vector.Z = z_ - depth / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 2
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + height / 2.0f;
            temp_vector.Z = z_ - depth / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 3
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - height / 2.0f;
            temp_vector.Z = z_ - depth / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 4
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - height / 2.0f;
            temp_vector.Z = z_ - depth / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 5
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + height / 2.0f;
            temp_vector.Z = z_ + depth / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 6
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + height / 2.0f;
            temp_vector.Z = z_ + depth / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 7
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - height / 2.0f;
            temp_vector.Z = z_ + depth / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 8
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - height / 2.0f;
            temp_vector.Z = z_ + depth / 2.0f;
            tempVertices.Add(temp_vector);

            var tempIndices = new List<uint>
            {
                //Back
                //1, 2, 0,
                //2, 1, 3,
                //Top
                5, 0, 4,
                0, 5, 1,
                //Right
                5, 3, 1,
                3, 5, 7,
                //Left
                0, 6, 4,
                6, 0, 2,
                //Front
                4, 7, 5,
                7, 4, 6,
                //Bottom
                3, 6, 2,
                6, 3, 7
            };
            vertices = tempVertices;
            indices = tempIndices;
        }



        public void createEllipsoid(float x, float y, float z, float radX, float radY, float radZ, float sectorCount, float stackCount)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radX * (float)Math.Cos(stackAngle);
                tempY = radY * (float)Math.Sin(stackAngle);
                tempZ = radZ * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);

                    }

                    if (i != stackCount - 1)
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }
        }

        public void createTorus(float x, float y, float z, float radMajor, float radMinor, float sectorCount, float stackCount)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            stackCount *= 2;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = 2 * pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radMajor + radMinor * (float)Math.Cos(stackAngle);
                tempY = radMinor * (float)Math.Sin(stackAngle);
                tempZ = radMajor + radMinor * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    indices.Add(k1);
                    indices.Add(k2);
                    indices.Add(k1 + 1);

                    indices.Add(k1 + 1);
                    indices.Add(k2);
                    indices.Add(k2 + 1);
                }
            }
        }

        public void tabung(float _positionX, float _positionY, float _positionZ, float _radiusx, float _radiusy, float _radiusz)
        {
            Vector3 temp_vector; float _pi = (float)Math.PI;
            for (float v = -20.0f; v <= _pi / 100; v += 0.01f)
            {
                for (float u = -20.0f; u <= _pi; u += (_pi / 100))
                {
                    temp_vector.X = _positionX + _radiusx * (float)Math.Cos(u);
                    temp_vector.Y = _positionY + _radiusy * (float)Math.Sin(u);
                    temp_vector.Z = _positionZ + _radiusz * v;
                    vertices.Add(temp_vector);
                }
            }
        }

        public void createElipticParaboloid(float x, float y, float z, float radiusX, float radiusY, float radiusZ)

        {
            var tempVertex = new Vector3();
            for (float u = -MathF.PI; u < MathF.PI; u += MathF.PI / 1000.0f)
            {
                for (float v = 0.0f; v < 5.0f; v += 0.01f)
                {
                    tempVertex.X = radiusX * v * MathF.Cos(u) + x;
                    tempVertex.Y = radiusY * v * MathF.Sin(u) + y;
                    tempVertex.Z = radiusZ * v * v + z;
                    vertices.Add(tempVertex);
                }
            }
        }

        public void createSphere(float x, float y, float z, float radXYZ, float sectorCount, float stackCount)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radXYZ * (float)Math.Cos(stackAngle);
                tempY = radXYZ * (float)Math.Sin(stackAngle);
                tempZ = radXYZ * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);

                    }

                    if (i != stackCount - 1)
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }
        }

        //bezierr
        public void prepareVertices()
        {
            verticesCurve = new float[1080];
            index = 0;
        }
        public void setControlCoordinate(float x, float y, float z)
        {
            verticesCurve[index * 3] = x;
            verticesCurve[index * 3 + 1] = y;
            verticesCurve[index * 3 + 2] = z;
            index++;
        }
        public List<int> getRow(int rowIndex)
        {
            List<int> currow = new List<int>();
            //------
            currow.Add(1);
            if (rowIndex == 0)
            {
                return currow;
            }
            //-----
            List<int> prev = getRow(rowIndex - 1);
            for (int i = 1; i < prev.Count; i++)
            {
                int curr = prev[i - 1] + prev[i];
                currow.Add(curr);
            }
            currow.Add(1);
            return currow;

        }
        public List<Vector3> createCurveBazier()
        {
            List<Vector3> _verticesBazier = new List<Vector3>();
            List<int> pascal = getRow(index - 1);
            _pascal = pascal.ToArray();
            for (float t = 0; t <= 1; t += 0.01f)
            {
                Vector3 p = getP(index, t);
                _verticesBazier.Add(p);
            }
            return _verticesBazier;
        }
        public Vector3 getP(int n, float t)
        {
            Vector3 p = new Vector3(0, 0, 0);
            float k;
            for (int i = 0; i < n; i++)
            {
                k = (float)Math.Pow((1 - t), n - 1 - i) * (float)Math.Pow(t, i) * _pascal[i];
                p.X += k * verticesCurve[i * 3];
                p.Y += k * verticesCurve[i * 3 + 1];
                p.Z += k * verticesCurve[i * 3 + 2];

            }
            return p;
        }
        public void setVertices(List<Vector3> temp)
        {
            vertices = temp;
        }
        //end bezier

        #endregion

        #region transforms
        public void rotate(Vector3 pivot, Vector3 vector, float angle)
        {
            var radAngle = MathHelper.DegreesToRadians(angle);

            var arbRotationMatrix = new Matrix4
                (
                new Vector4((float)(Math.Cos(radAngle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(radAngle))), (float)(vector.X * vector.Y * (1.0f - Math.Cos(radAngle)) + vector.Z * Math.Sin(radAngle)), (float)(vector.X * vector.Z * (1.0f - Math.Cos(radAngle)) - vector.Y * Math.Sin(radAngle)), 0),
                new Vector4((float)(vector.X * vector.Y * (1.0f - Math.Cos(radAngle)) - vector.Z * Math.Sin(radAngle)), (float)(Math.Cos(radAngle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(radAngle))), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(radAngle)) + vector.X * Math.Sin(radAngle)), 0),
                new Vector4((float)(vector.X * vector.Z * (1.0f - Math.Cos(radAngle)) + vector.Y * Math.Sin(radAngle)), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(radAngle)) - vector.X * Math.Sin(radAngle)), (float)(Math.Cos(radAngle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(radAngle))), 0),
                Vector4.UnitW
                );

            model *= Matrix4.CreateTranslation(-pivot);
            model *= arbRotationMatrix;
            model *= Matrix4.CreateTranslation(pivot);

            for (int i = 0; i < 3; i++)
            {
                _euler[i] = Vector3.Normalize(getRotationResult(pivot, vector, radAngle, _euler[i], true));
            }

            objectCenter = getRotationResult(pivot, vector, radAngle, objectCenter);

            foreach (var i in child)
            {
                i.rotate(pivot, vector, angle);
            }
        }

        public Vector3 getRotationResult(Vector3 pivot, Vector3 vector, float angle, Vector3 point, bool isEuler = false)
        {
            Vector3 temp, newPosition;

            if (isEuler)
            {
                temp = point;
            }
            else
            {
                temp = point - pivot;
            }

            newPosition.X =
                temp.X * (float)(Math.Cos(angle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(angle))) +
                temp.Y * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) - vector.Z * Math.Sin(angle)) +
                temp.Z * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) + vector.Y * Math.Sin(angle));

            newPosition.Y =
                temp.X * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) + vector.Z * Math.Sin(angle)) +
                temp.Y * (float)(Math.Cos(angle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(angle))) +
                temp.Z * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) - vector.X * Math.Sin(angle));

            newPosition.Z =
                temp.X * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) - vector.Y * Math.Sin(angle)) +
                temp.Y * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) + vector.X * Math.Sin(angle)) +
                temp.Z * (float)(Math.Cos(angle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(angle)));

            if (isEuler)
            {
                temp = newPosition;
            }
            else
            {
                temp = newPosition + pivot;
            }
            return temp;
        }

        public void translate(float x, float y, float z)
        {
            model *= Matrix4.CreateTranslation(x, y, z);
            objectCenter.X += x;
            objectCenter.Y += y;
            objectCenter.Z += z;

            foreach (var i in child)
            {
                i.translate(x, y, z);
            }
        }

        public void scale(float scaleX, float scaleY, float scaleZ)
        {
            model *= Matrix4.CreateTranslation(-objectCenter);
            model *= Matrix4.CreateScale(scaleX, scaleY, scaleZ);
            model *= Matrix4.CreateTranslation(objectCenter);

            foreach (var i in child)
            {
                i.scale(scaleX, scaleY, scaleZ);
            }
        }
        #endregion
    }
}
