using System;
using Unianio.Extensions;
using Unianio.Services;
using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        // http://wiki.unity3d.com/index.php/ProceduralPrimitives
        public static class meshes
        {
            static GameObject CreateGameObject(string prefix, DtBase data, out Mesh mesh)
            {
                var gameObject = new GameObject(data.name ?? prefix + "_" + ShortGuid.New().ToString("X"));
                var renderer = gameObject.AddComponent<MeshRenderer>();
                renderer.material = GlobalFactory.Default.Get<IResourceManager>().LoadMaterial(materials.Path + materials.DynamicMesh);
                //renderer.material.SetTexture("_DetailAlbedoMap", Resources.Load<Texture>(""));
                var meshFilter = gameObject.AddComponent<MeshFilter>();

                mesh = meshFilter.mesh;
                mesh.Clear();
                mesh.name = prefix;
                data.mesh = mesh;
                if (data.set != null) data.set(gameObject.transform);
                return gameObject;
            }
            #region Box
            public static GameObject CreateBox()
            {
                return CreateBox(new DtBox());
            }
            public static GameObject CreateBox(DtBox dt)
            {
                Mesh m;
                var gameObject = CreateGameObject("Box", dt, out m);

                var x = (float)dt.x;
                var y = (float)dt.y;
                var z = (float)dt.z;

                #region Vertices
                var p0 = new Vector3(-x * .5f, -y * .5f, z * .5f);
                var p1 = new Vector3(x * .5f, -y * .5f, z * .5f);
                var p2 = new Vector3(x * .5f, -y * .5f, -z * .5f);
                var p3 = new Vector3(-x * .5f, -y * .5f, -z * .5f);

                var p4 = new Vector3(-x * .5f, y * .5f, z * .5f);
                var p5 = new Vector3(x * .5f, y * .5f, z * .5f);
                var p6 = new Vector3(x * .5f, y * .5f, -z * .5f);
                var p7 = new Vector3(-x * .5f, y * .5f, -z * .5f);

                var vertices = new[]
                {
			        // Bottom
			        p0, p1, p2, p3,
			
			        // Left
			        p7, p4, p0, p3,
			
			        // Front
			        p4, p5, p1, p0,
			
			        // Back
			        p6, p7, p3, p2,
			
			        // Right
			        p5, p6, p2, p1,
			
			        // Top
			        p7, p6, p5, p4
                };
                #endregion

                #region normals
                var up = Vector3.up;
                var down = Vector3.down;
                var front = Vector3.forward;
                var back = Vector3.back;
                var left = Vector3.left;
                var right = Vector3.right;

                var normals = new[]
                {
			        // Bottom
			        down, down, down, down,
			
			        // Left
			        left, left, left, left,
			
			        // Front
			        front, front, front, front,
			
			        // Back
			        back, back, back, back,
			
			        // Right
			        right, right, right, right,
			
			        // Top
			        up, up, up, up
                };
                #endregion

                #region UVs
                var p00 = new Vector2(0f, 0f);
                var p10 = new Vector2(1f, 0f);
                var p01 = new Vector2(0f, 1f);
                var p11 = new Vector2(1f, 1f);

                var uvs = new[]
                {
			        // Bottom
			        p11, p01, p00, p10,
			
			        // Left
			        p11, p01, p00, p10,
			
			        // Front
			        p11, p01, p00, p10,
			
			        // Back
			        p11, p01, p00, p10,
			
			        // Right
			        p11, p01, p00, p10,
			
			        // Top
			        p11, p01, p00, p10,
                };
                #endregion

                #region Triangles
                var triangles = new[]
                {
			        // Bottom
			        3, 1, 0,
                    3, 2, 1,			
			
			        // Left
			        3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
                    3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
			
			        // Front
			        3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
                    3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
			
			        // Back
			        3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
                    3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
			
			        // Right
			        3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
                    3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
			
			        // Top
			        3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
                    3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,

                };
                #endregion

                m.vertices = vertices;
                m.normals = normals;
                m.uv = uvs;
                m.triangles = triangles;

                m.RecalculateBounds();
                ;
                return gameObject;
            }
            #endregion

            #region Triangle Plane
            public static GameObject CreateTwoSidedTrianglePlane()
            {
                return CreateTwoSidedTrianglePlane(new DtTrianglePlane());
            }
            public static GameObject CreateTwoSidedTrianglePlane(DtTrianglePlane dt)
            {
                Mesh m;
                var gameObject = CreateGameObject("TwoSidedTrianglePlane", dt, out m);

                var length = (float)dt.length;
                var width = (float)dt.width;

                #region Vertices		
                var vertices = new Vector3[4];
                vertices[0] = new Vector3(length * -0.5f, width * -0.5f, 0);
                vertices[1] = new Vector3(length * 0.5f, width * -0.5f, 0);
                vertices[2] = new Vector3(length * -0.5f, width * 0.5f, 0);
                #endregion

                #region Normales
                var normals = new Vector3[vertices.Length];
                normals[0] = Vector3.forward;
                normals[1] = Vector3.forward;
                normals[2] = Vector3.forward;
                #endregion

                #region UVs		
                Vector2[] uvs = new Vector2[vertices.Length];
                uvs[0] = new Vector2(0, 0);
                uvs[1] = new Vector2(1, 0);
                uvs[2] = new Vector2(0, 1);
                #endregion

                #region Triangles
                var triangles = new int[6];
                int t = 0;

                triangles[t++] = 0;
                triangles[t++] = 1;
                triangles[t++] = 2;

                triangles[t++] = 2;
                triangles[t++] = 1;
                triangles[t++] = 0;
                #endregion

                m.vertices = vertices;
                m.normals = normals;
                m.uv = uvs;
                m.triangles = triangles;

                m.RecalculateBounds();
                ;
                return gameObject;
            }
            #endregion

            #region Square Plane
            public static GameObject CreateTwoSidedSquarePlane()
            {
                return CreateTwoSidedSquarePlane(new DtSquarePlane());
            }
            public static GameObject CreateTwoSidedSquarePlane(DtSquarePlane dt)
            {
                Mesh m;
                var gameObject = CreateGameObject("TwoSidedSquarePlane", dt, out m);

                var length = (float)dt.length;
                var width = (float)dt.width;

                #region Vertices		
                var vertices = new Vector3[4];
                vertices[0] = new Vector3(length * -0.5f, width * -0.5f, 0);
                vertices[1] = new Vector3(length * 0.5f, width * -0.5f, 0);
                vertices[2] = new Vector3(length * -0.5f, width * 0.5f, 0);
                vertices[3] = new Vector3(length * 0.5f, width * 0.5f, 0);
                #endregion

                #region Normales
                var normals = new Vector3[vertices.Length];
                normals[0] = Vector3.forward;
                normals[1] = Vector3.forward;
                normals[2] = Vector3.forward;
                normals[3] = Vector3.forward;
                #endregion

                #region UVs		
                Vector2[] uvs = new Vector2[vertices.Length];
                uvs[0] = new Vector2(0, 0);
                uvs[1] = new Vector2(1, 0);
                uvs[2] = new Vector2(0, 1);
                uvs[3] = new Vector2(1, 1);
                #endregion

                #region Triangles
                var triangles = new int[12];
                int t = 0;

                triangles[t++] = 0;
                triangles[t++] = 1;
                triangles[t++] = 2;

                triangles[t++] = 3;
                triangles[t++] = 2;
                triangles[t++] = 1;

                triangles[t++] = 1;
                triangles[t++] = 2;
                triangles[t++] = 3;

                triangles[t++] = 2;
                triangles[t++] = 1;
                triangles[t++] = 0;
                #endregion

                m.vertices = vertices;
                m.normals = normals;
                m.uv = uvs;
                m.triangles = triangles;

                m.RecalculateBounds();
                ;
                return gameObject;
            }
            public static GameObject CreateSquarePlane()
            {
                return CreateSquarePlane(new DtSquarePlane());
            }
            public static GameObject CreateSquarePlane(DtSquarePlane dt)
            {
                Mesh m;
                var gameObject = CreateGameObject("SquarePlane", dt, out m);

                var length = (float)dt.length;
                var width = (float)dt.width;

                #region Vertices		
                var vertices = new Vector3[4];
                vertices[0] = new Vector3(length * -0.5f, width * -0.5f, 0);
                vertices[1] = new Vector3(length * 0.5f, width * -0.5f, 0);
                vertices[2] = new Vector3(length * -0.5f, width * 0.5f, 0);
                vertices[3] = new Vector3(length * 0.5f, width * 0.5f, 0);
                #endregion

                #region Normales
                var normals = new Vector3[vertices.Length];
                normals[0] = Vector3.forward;
                normals[1] = Vector3.forward;
                normals[2] = Vector3.forward;
                normals[3] = Vector3.forward;
                #endregion

                #region UVs		
                Vector2[] uvs = new Vector2[vertices.Length];
                uvs[0] = new Vector2(0, 0);
                uvs[1] = new Vector2(1, 0);
                uvs[2] = new Vector2(0, 1);
                uvs[3] = new Vector2(1, 1);
                #endregion

                #region Triangles
                var triangles = new int[6];
                int t = 0;

                triangles[t++] = 0;
                triangles[t++] = 1;
                triangles[t++] = 2;

                triangles[t++] = 3;
                triangles[t++] = 2;
                triangles[t++] = 1;
                #endregion

                m.vertices = vertices;
                m.normals = normals;
                m.uv = uvs;
                m.triangles = triangles;

                m.RecalculateBounds();
                ;
                return gameObject;
            }
            #endregion

            #region Cone
            public static GameObject CreateCone()
            {
                return CreateCone(new DtCone());
            }
            public static GameObject CreateCone(DtCone dt)
            {
                Mesh m;
                var gameObject = CreateGameObject("Cone", dt, out m);

                var height = (float)dt.height;
                var bottomRadius = (float)dt.bottomRadius;
                var topRadius = (float)dt.topRadius;
                var numSides = dt.numSides;
                var numHeightSeg = 1;

                var nbVerticesCap = numSides + 1;
                #region Vertices

                // bottom + top + sides
                Vector3[] vertices = new Vector3[nbVerticesCap + nbVerticesCap + numSides * numHeightSeg * 2 + 2];
                int vert = 0;
                float _2pi = Mathf.PI * 2f;

                // Bottom cap
                vertices[vert++] = new Vector3(0f, 0f, 0f);
                while (vert <= numSides)
                {
                    float rad = (float)vert / numSides * _2pi;
                    vertices[vert] = new Vector3(Mathf.Cos(rad) * bottomRadius, 0f, Mathf.Sin(rad) * bottomRadius);
                    vert++;
                }

                // Top cap
                vertices[vert++] = new Vector3(0f, height, 0f);
                while (vert <= numSides * 2 + 1)
                {
                    float rad = (float)(vert - numSides - 1) / numSides * _2pi;
                    vertices[vert] = new Vector3(Mathf.Cos(rad) * topRadius, height, Mathf.Sin(rad) * topRadius);
                    vert++;
                }


                // Sides
                int v = 0;
                while (vert <= vertices.Length - 4)
                {
                    float rad = (float)v / numSides * _2pi;
                    vertices[vert] = new Vector3(Mathf.Cos(rad) * topRadius, height, Mathf.Sin(rad) * topRadius);
                    vertices[vert + 1] = new Vector3(Mathf.Cos(rad) * bottomRadius, 0, Mathf.Sin(rad) * bottomRadius);
                    vert += 2;
                    v++;
                }
                vertices[vert] = vertices[numSides * 2 + 2];
                vertices[vert + 1] = vertices[numSides * 2 + 3];
                #endregion

                #region Normales

                // bottom + top + sides
                Vector3[] normales = new Vector3[vertices.Length];
                vert = 0;

                // Bottom cap
                while (vert <= numSides)
                {
                    normales[vert++] = Vector3.down;
                }

                // Top cap
                while (vert <= numSides * 2 + 1)
                {
                    normales[vert++] = Vector3.up;
                }

                // Sides
                v = 0;
                while (vert <= vertices.Length - 4)
                {
                    float rad = (float)v / numSides * _2pi;
                    float cos = Mathf.Cos(rad);
                    float sin = Mathf.Sin(rad);

                    normales[vert] = new Vector3(cos, 0f, sin);
                    normales[vert + 1] = normales[vert];

                    vert += 2;
                    v++;
                }
                normales[vert] = normales[numSides * 2 + 2];
                normales[vert + 1] = normales[numSides * 2 + 3];
                #endregion

                #region UVs
                Vector2[] uvs = new Vector2[vertices.Length];

                // Bottom cap
                int u = 0;
                uvs[u++] = new Vector2(0.5f, 0.5f);
                while (u <= numSides)
                {
                    float rad = (float)u / numSides * _2pi;
                    uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
                    u++;
                }

                // Top cap
                uvs[u++] = new Vector2(0.5f, 0.5f);
                while (u <= numSides * 2 + 1)
                {
                    float rad = (float)u / numSides * _2pi;
                    uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
                    u++;
                }

                // Sides
                int u_sides = 0;
                while (u <= uvs.Length - 4)
                {
                    float t = (float)u_sides / numSides;
                    uvs[u] = new Vector3(t, 1f);
                    uvs[u + 1] = new Vector3(t, 0f);
                    u += 2;
                    u_sides++;
                }
                uvs[u] = new Vector2(1f, 1f);
                uvs[u + 1] = new Vector2(1f, 0f);
                #endregion

                #region Triangles
                int nbTriangles = numSides + numSides + numSides * 2;
                int[] triangles = new int[nbTriangles * 3 + 3];

                // Bottom cap
                int tri = 0;
                int i = 0;
                while (tri < numSides - 1)
                {
                    triangles[i] = 0;
                    triangles[i + 1] = tri + 1;
                    triangles[i + 2] = tri + 2;
                    tri++;
                    i += 3;
                }
                triangles[i] = 0;
                triangles[i + 1] = tri + 1;
                triangles[i + 2] = 1;
                tri++;
                i += 3;

                // Top cap
                //tri++;
                while (tri < numSides * 2)
                {
                    triangles[i] = tri + 2;
                    triangles[i + 1] = tri + 1;
                    triangles[i + 2] = nbVerticesCap;
                    tri++;
                    i += 3;
                }

                triangles[i] = nbVerticesCap + 1;
                triangles[i + 1] = tri + 1;
                triangles[i + 2] = nbVerticesCap;
                tri++;
                i += 3;
                tri++;

                // Sides
                while (tri <= nbTriangles)
                {
                    triangles[i] = tri + 2;
                    triangles[i + 1] = tri + 1;
                    triangles[i + 2] = tri + 0;
                    tri++;
                    i += 3;

                    triangles[i] = tri + 1;
                    triangles[i + 1] = tri + 2;
                    triangles[i + 2] = tri + 0;
                    tri++;
                    i += 3;
                }
                #endregion

                m.vertices = vertices;
                m.normals = normales;
                m.uv = uvs;
                m.triangles = triangles;

                m.RecalculateBounds();
                ;
                return gameObject;
            }
            public static GameObject CreatePointyCone()
            {
                return CreateCone(new DtCone());
            }
            public static GameObject CreatePointyCone(DtCone dt)
            {
                Mesh m;
                var gameObject = CreateGameObject("PointyCone", dt, out m);

                var height = (float)dt.height;
                var bottomRadius = (float)dt.bottomRadius;
                var topRadius = (float)dt.topRadius;
                var nbSides = dt.numSides;
                var noseLen = (float)dt.relNoseLen;
                var nbHeightSeg = 1;

                int nbVerticesCap = nbSides + 1;
                #region Vertices

                // bottom + top + sides
                Vector3[] vertices = new Vector3[nbVerticesCap + nbVerticesCap + nbSides * nbHeightSeg * 2 + 2];
                int vert = 0;
                float _2pi = Mathf.PI * 2f;

                // Bottom cap
                vertices[vert++] = new Vector3(0f, 0f, 0f);
                while (vert <= nbSides)
                {
                    float rad = (float)vert / nbSides * _2pi;
                    var isXtop = Mathf.Sin(rad) > 0.95f;
                    var nose = isXtop ? noseLen * bottomRadius : 0;
                    var noseShift = isXtop ? 0 : 1;
                    vertices[vert] = new Vector3(Mathf.Cos(rad) * bottomRadius * noseShift, 0f, Mathf.Sin(rad) * bottomRadius + nose);
                    vert++;
                }

                // Top cap
                vertices[vert++] = new Vector3(0f, height, 0f);
                while (vert <= nbSides * 2 + 1)
                {
                    float rad = (float)(vert - nbSides - 1) / nbSides * _2pi;

                    vertices[vert] = new Vector3(Mathf.Cos(rad) * topRadius, height, Mathf.Sin(rad) * topRadius);
                    vert++;
                }


                // Sides
                int v = 0;
                while (vert <= vertices.Length - 4)
                {
                    float rad = (float)v / nbSides * _2pi;

                    var isXtop = Mathf.Sin(rad) > 0.95f;
                    var nose = isXtop ? noseLen * bottomRadius : 0;
                    var noseShift = isXtop ? 0 : 1;
                    vertices[vert] = new Vector3(Mathf.Cos(rad) * topRadius, height, Mathf.Sin(rad) * topRadius);
                    vertices[vert + 1] = new Vector3(Mathf.Cos(rad) * bottomRadius * noseShift, 0, Mathf.Sin(rad) * bottomRadius + nose);
                    vert += 2;
                    v++;
                }
                vertices[vert] = vertices[nbSides * 2 + 2];
                vertices[vert + 1] = vertices[nbSides * 2 + 3];
                #endregion

                #region Normales

                // bottom + top + sides
                Vector3[] normales = new Vector3[vertices.Length];
                vert = 0;

                // Bottom cap
                while (vert <= nbSides)
                {
                    normales[vert++] = Vector3.down;
                }

                // Top cap
                while (vert <= nbSides * 2 + 1)
                {
                    normales[vert++] = Vector3.up;
                }

                // Sides
                v = 0;
                while (vert <= vertices.Length - 4)
                {
                    float rad = (float)v / nbSides * _2pi;
                    float cos = Mathf.Cos(rad);
                    float sin = Mathf.Sin(rad);

                    normales[vert] = new Vector3(cos, 0f, sin);
                    normales[vert + 1] = normales[vert];

                    vert += 2;
                    v++;
                }
                normales[vert] = normales[nbSides * 2 + 2];
                normales[vert + 1] = normales[nbSides * 2 + 3];
                #endregion

                #region UVs
                Vector2[] uvs = new Vector2[vertices.Length];

                // Bottom cap
                int u = 0;
                uvs[u++] = new Vector2(0.5f, 0.5f);
                while (u <= nbSides)
                {
                    float rad = (float)u / nbSides * _2pi;
                    uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
                    u++;
                }

                // Top cap
                uvs[u++] = new Vector2(0.5f, 0.5f);
                while (u <= nbSides * 2 + 1)
                {
                    float rad = (float)u / nbSides * _2pi;
                    uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
                    u++;
                }

                // Sides
                int u_sides = 0;
                while (u <= uvs.Length - 4)
                {
                    float t = (float)u_sides / nbSides;
                    uvs[u] = new Vector3(t, 1f);
                    uvs[u + 1] = new Vector3(t, 0f);
                    u += 2;
                    u_sides++;
                }
                uvs[u] = new Vector2(1f, 1f);
                uvs[u + 1] = new Vector2(1f, 0f);
                #endregion

                #region Triangles
                int nbTriangles = nbSides + nbSides + nbSides * 2;
                int[] triangles = new int[nbTriangles * 3 + 3];

                // Bottom cap
                int tri = 0;
                int i = 0;
                while (tri < nbSides - 1)
                {
                    triangles[i] = 0;
                    triangles[i + 1] = tri + 1;
                    triangles[i + 2] = tri + 2;
                    tri++;
                    i += 3;
                }
                triangles[i] = 0;
                triangles[i + 1] = tri + 1;
                triangles[i + 2] = 1;
                tri++;
                i += 3;

                // Top cap
                //tri++;
                while (tri < nbSides * 2)
                {
                    triangles[i] = tri + 2;
                    triangles[i + 1] = tri + 1;
                    triangles[i + 2] = nbVerticesCap;
                    tri++;
                    i += 3;
                }

                triangles[i] = nbVerticesCap + 1;
                triangles[i + 1] = tri + 1;
                triangles[i + 2] = nbVerticesCap;
                tri++;
                i += 3;
                tri++;

                // Sides
                while (tri <= nbTriangles)
                {
                    triangles[i] = tri + 2;
                    triangles[i + 1] = tri + 1;
                    triangles[i + 2] = tri + 0;
                    tri++;
                    i += 3;

                    triangles[i] = tri + 1;
                    triangles[i + 1] = tri + 2;
                    triangles[i + 2] = tri + 0;
                    tri++;
                    i += 3;
                }
                #endregion

                m.vertices = vertices;
                m.normals = normales;
                m.uv = uvs;
                m.triangles = triangles;

                m.RecalculateBounds();
                ;
                return gameObject;
            }
            #endregion

            #region Sphere
            public static GameObject CreateSphere()
            {
                return CreateSphere(new DtSphere());
            }

            public static GameObject CreateSphere(DtSphere dt)
            {
                Mesh m;
                var gameObject = CreateGameObject("Sphere", dt, out m);

                var radius = (float)dt.radius;
                // Longitude |||
                int nbLong = dt.longitude;
                // Latitude ---
                int nbLat = dt.latitude;

                #region Vertices
                Vector3[] vertices = new Vector3[(nbLong + 1) * nbLat + 2];
                float _pi = Mathf.PI;
                float _2pi = _pi * 2f;

                vertices[0] = Vector3.up * radius;
                for (int lat = 0; lat < nbLat; lat++)
                {
                    float a1 = _pi * (float)(lat + 1) / (nbLat + 1);
                    float sin1 = Mathf.Sin(a1);
                    float cos1 = Mathf.Cos(a1);

                    for (int lon = 0; lon <= nbLong; lon++)
                    {
                        float a2 = _2pi * (float)(lon == nbLong ? 0 : lon) / nbLong;
                        float sin2 = Mathf.Sin(a2);
                        float cos2 = Mathf.Cos(a2);

                        vertices[lon + lat * (nbLong + 1) + 1] = new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
                    }
                }
                vertices[vertices.Length - 1] = Vector3.up * -radius;
                #endregion

                #region Normales		
                Vector3[] normales = new Vector3[vertices.Length];
                for (int n = 0; n < vertices.Length; n++)
                    normales[n] = vertices[n].normalized;
                #endregion

                #region UVs
                Vector2[] uvs = new Vector2[vertices.Length];
                uvs[0] = Vector2.up;
                uvs[uvs.Length - 1] = Vector2.zero;
                for (int lat = 0; lat < nbLat; lat++)
                    for (int lon = 0; lon <= nbLong; lon++)
                        uvs[lon + lat * (nbLong + 1) + 1] = new Vector2((float)lon / nbLong, 1f - (float)(lat + 1) / (nbLat + 1));
                #endregion

                #region Triangles
                int nbFaces = vertices.Length;
                int nbTriangles = nbFaces * 2;
                int nbIndexes = nbTriangles * 3;
                int[] triangles = new int[nbIndexes];

                //Top Cap
                int i = 0;
                for (int lon = 0; lon < nbLong; lon++)
                {
                    triangles[i++] = lon + 2;
                    triangles[i++] = lon + 1;
                    triangles[i++] = 0;
                }

                //Middle
                for (int lat = 0; lat < nbLat - 1; lat++)
                {
                    for (int lon = 0; lon < nbLong; lon++)
                    {
                        int current = lon + lat * (nbLong + 1) + 1;
                        int next = current + nbLong + 1;

                        triangles[i++] = current;
                        triangles[i++] = current + 1;
                        triangles[i++] = next + 1;

                        triangles[i++] = current;
                        triangles[i++] = next + 1;
                        triangles[i++] = next;
                    }
                }

                //Bottom Cap
                for (int lon = 0; lon < nbLong; lon++)
                {
                    triangles[i++] = vertices.Length - 1;
                    triangles[i++] = vertices.Length - (lon + 2) - 1;
                    triangles[i++] = vertices.Length - (lon + 1) - 1;
                }
                #endregion

                m.vertices = vertices;
                m.normals = normales;
                m.uv = uvs;
                m.triangles = triangles;

                m.RecalculateBounds();
                ;

                return gameObject;
            }

            #endregion

            #region Capsule
            public static GameObject CreateCapsule()
            {
                return CreateCapsule(new DtCapsule());
            }

            public static GameObject CreateCapsule(DtCapsule dt)
            {
                Mesh m;
                var gameObject = CreateGameObject("Capsule", dt, out m);

                var height = (float)dt.height;
                var radius = (float)dt.radius;
                // Longitude |||
                int nbLong = dt.longitude;
                // Latitude ---
                int nbLat = dt.latitude;


                #region Vertices
                Vector3[] vertices = new Vector3[(nbLong + 1) * nbLat + 2];
                float _pi = Mathf.PI;
                float _2pi = _pi * 2f;

                var upperShift = Vector3.up * (height / 2f);

                vertices[0] = Vector3.up * radius + upperShift + dt.localPos;
                var halfLat = nbLat / 2;
                for (int lat = 0; lat < halfLat; lat++)
                {
                    float a1 = _pi * (float)(lat + 1) / (nbLat + 1);
                    float sin1 = Mathf.Sin(a1);
                    float cos1 = Mathf.Cos(a1);

                    for (int lon = 0; lon <= nbLong; lon++)
                    {
                        float a2 = _2pi * (float)(lon == nbLong ? 0 : lon) / nbLong;
                        float sin2 = Mathf.Sin(a2);
                        float cos2 = Mathf.Cos(a2);
                        vertices[lon + lat * (nbLong + 1) + 1] = new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius + upperShift + dt.localPos;
                    }
                }

                var lowerShift = Vector3.up * (-height / 2f);

                for (int lat = halfLat; lat < nbLat; lat++)
                {
                    float a1 = _pi * (float)(lat + 1) / (nbLat + 1);
                    float sin1 = Mathf.Sin(a1);
                    float cos1 = Mathf.Cos(a1);

                    for (int lon = 0; lon <= nbLong; lon++)
                    {
                        float a2 = _2pi * (float)(lon == nbLong ? 0 : lon) / nbLong;
                        float sin2 = Mathf.Sin(a2);
                        float cos2 = Mathf.Cos(a2);
                        vertices[lon + lat * (nbLong + 1) + 1] = new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius + lowerShift + dt.localPos;
                    }
                }
                vertices[vertices.Length - 1] = Vector3.up * -radius + lowerShift + dt.localPos;
                #endregion

                #region Normales		
                Vector3[] normales = new Vector3[vertices.Length];
                for (int n = 0; n < vertices.Length / 2; n++)
                {
                    normales[n] = ((vertices[n] - upperShift).normalized - dt.localPos).normalized;
                }
                for (int n = vertices.Length / 2; n < vertices.Length; n++)
                {
                    normales[n] = ((vertices[n] - lowerShift).normalized - dt.localPos).normalized;
                }
                #endregion

                #region UVs
                Vector2[] uvs = new Vector2[vertices.Length];
                uvs[0] = Vector2.up;
                uvs[uvs.Length - 1] = Vector2.zero;
                for (int lat = 0; lat < nbLat; lat++)
                    for (int lon = 0; lon <= nbLong; lon++)
                        uvs[lon + lat * (nbLong + 1) + 1] = new Vector2((float)lon / nbLong, 1f - (float)(lat + 1) / (nbLat + 1));
                #endregion

                #region Triangles
                int nbFaces = vertices.Length;
                int nbTriangles = nbFaces * 2;
                int nbIndexes = nbTriangles * 3;
                int[] triangles = new int[nbIndexes];

                //Top Cap
                int i = 0;
                for (int lon = 0; lon < nbLong; lon++)
                {
                    triangles[i++] = lon + 2;
                    triangles[i++] = lon + 1;
                    triangles[i++] = 0;
                }

                //Middle
                for (int lat = 0; lat < nbLat - 1; lat++)
                {
                    for (int lon = 0; lon < nbLong; lon++)
                    {
                        int current = lon + lat * (nbLong + 1) + 1;
                        int next = current + nbLong + 1;

                        triangles[i++] = current;
                        triangles[i++] = current + 1;
                        triangles[i++] = next + 1;

                        triangles[i++] = current;
                        triangles[i++] = next + 1;
                        triangles[i++] = next;
                    }
                }

                //Bottom Cap
                for (int lon = 0; lon < nbLong; lon++)
                {
                    triangles[i++] = vertices.Length - 1;
                    triangles[i++] = vertices.Length - (lon + 2) - 1;
                    triangles[i++] = vertices.Length - (lon + 1) - 1;
                }
                #endregion



                m.vertices = vertices;
                m.normals = normales;
                m.uv = uvs;
                m.triangles = triangles;

                m.RecalculateBounds();
                ;
                return gameObject;
            }
            #endregion

        }

    }
    public abstract class DtBase
    {
        public string name;
        public Mesh mesh;
        public Action<Transform> set;
    }
    public class DtTrianglePlane : DtBase
    {
        public double length = 1f;
        public double width = 1f;
    }
    public class DtSquarePlane : DtBase
    {
        public double length = 1f;
        public double width = 1f;
    }
    public class DtCone : DtBase
    {
        public double height = 1f;
        public double bottomRadius = .5f;
        public double topRadius = .01f;
        public int numSides = 18;
        public double relNoseLen = 0.75;
    }
    public class DtCapsule : DtBase
    {
        public double height = 3f; // distance from capsule upper sphere center to capsule lower sphere center
        public double radius = 1f;
        public int longitude = 24;
        public int latitude = 16;
        public Vector3 localPos = Vector3.zero;

        public static DtCapsule Create(in Vector3 a, in Vector3 b, double radius)
        {
            var mid = fun.lerp(a, b, 0.5);
            var fw = b.DirTo(a);
            fun.vector.ComputeRandomXYAxesForPlane(fw, out var rt, out var up);
            return new DtCapsule
            {
                height = fun.distance.Between(in a, in b),
                radius = radius,
                set = t =>
                {
                    t.position = mid;
                    t.rotation = fun.lookAt(up, fw);
                }
            };
        }
    }
    public class DtBox : DtBase
    {
        public double x = 1;
        public double y = 1;
        public double z = 1;

        public double side
        {
            get => (x + y + z) / 3.0;
            set => x = y = z = value;
        }
    }
    public class DtSphere : DtBase
    {
        public double radius = 1;
        public int longitude = 24;
        public int latitude = 16;
    }
}