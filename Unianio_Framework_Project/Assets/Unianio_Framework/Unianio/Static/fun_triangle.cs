using System;
using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class triangle
        {
            /* different tasks for any type of triangle 
                   ^
                  /C\  
               a /   \ b
                /B___A\
                   c 
            */

            /* "we know sides:a,b,c what is angle C"
                   ^
                  /?\  
               a /   \ b
                /_____\
                   c 
                C = arccos((a*a + b*b - c*c) / 2*a*b)
            */
            public static float GetDegreesBySides(double a, double b, double c)
            {
                return (float)Math.Acos((a * a + b * b - c * c) / (2 * a * b)) * RTD; // http://mathworld.wolfram.com/LawofCosines.html
            }
            /* "we know sides:a,b AND angle:C what is side c"
                   ^
                  /C\  
               a /   \ b
                /_____\
                   ? 
                c	=	sqrt(a*a + b*b-2*a*b*cos(C))
            */
            public static float GetBaseByTwoSidesAndAngleBetween(double a, double b, double degreesC)
            {
                return (float)Math.Sqrt(a * a + b * b - 2 * a * b * Math.Cos(degreesC * DTR)); // http://mathworld.wolfram.com/LawofCosines.html
            }
            /* "we know sides:a,b AND angle:A what is angle C"
                   ^
                  /?\  
               a /   \ b
                /____A\
                   c 
                C	=	180 - A - arcsin((b * sin(A)) / a)
            */
            public static float GetAngleDegreesByTwoSidesAndSideAngle(double a, double b, double degreesA)
            {
                return 180f - (float)degreesA - RTD * (float)Math.Asin((b * Math.Sin(degreesA * DTR)) / a); // https://www.mathsisfun.com/algebra/trig-sine-law.html
            }
            public static Vector3 GetCentroid(Vector3 a, Vector3 b, Vector3 c)
            {
                return new Vector3((a.x + b.x + c.x) / 3f, (a.y + b.y + c.y) / 3f, (a.z + b.z + c.z) / 3f);
            }
            public static Vector3 GetCentroid(in Vector3 a, in Vector3 b, in Vector3 c)
            {
                return new Vector3((a.x + b.x + c.x) / 3f, (a.y + b.y + c.y) / 3f, (a.z + b.z + c.z) / 3f);
            }
            public static void GetCentroid(in Vector3 a, in Vector3 b, in Vector3 c, out Vector3 output)
            {
                output = new Vector3((a.x + b.x + c.x) / 3f, (a.y + b.y + c.y) / 3f, (a.z + b.z + c.z) / 3f);
            }
            public static Vector2 GetCentroid2D(Vector2 a, Vector2 b, Vector2 c)
            {
                return new Vector2((a.x + b.x + c.x) / 3f, (a.y + b.y + c.y) / 3f);
            }
            public static Vector2 GetCentroid2D(in Vector2 a, in Vector2 b, in Vector2 c)
            {
                return new Vector2((a.x + b.x + c.x) / 3f, (a.y + b.y + c.y) / 3f);
            }
            public static void GetCentroid2D(in Vector2 a, in Vector2 b, in Vector2 c, out Vector2 centroid)
            {
                centroid = new Vector2((a.x + b.x + c.x) / 3f, (a.y + b.y + c.y) / 3f);
            }
            /*
               ^
              /|\  
           a / |h\ b
            /__|__\
               c
            height starts between sides a and b and falls info side c
            */
            public static float GetHeight(double a, double b, double c)
            {
                if (abs(c) < 0.000001) return (float)((a + b) / 2);
                var s = (a + b + c) / 2f;
                var n = s * (s - a) * (s - b) * (s - c);
                if (n < 0) return 0;
                return (float)((2 * Math.Sqrt(n)) / c);
            }
            /*
               ^
              /|\  
           a / |h\ b
            /__|__\
             ac+bc = c

            ac+bc = c
            ac^2 + h^2 = a^2
            bc^2 + h^2 = b^2
            */
            public static void GetBaseSubSides(double a, double b, double c, out float ac, out float bc)
            {
                var h = GetHeight(a, b, c);
                ac = sqrt(a * a - h * h);
                bc = sqrt(b * b - h * h);
            }
            /*
               ^
              /|\  
           a / |h\ b
            /__|__\
             ac+bc = c

            ac+bc = c
            ac^2 + h^2 = a^2
            bc^2 + h^2 = b^2
            */
            public static float GetBaseSubSideAc(double a, double b, double c)
            {
                var h = GetHeight(a, b, c);
                return sqrt(a * a - h * h);
            }

            /*
               ^
              /D\  
             / |h\
            /__|__\
            baseLen

            D = 180 - 2*arctan(2h/baseLen)*radians_to_degrees
            */
            public static float GetInIsoscelesDegreesByBaseAndHeight(double baseLen, double height)
            {
                return 180f - (float)(2 * Math.Atan((2 * height) / baseLen) * RTD);
            }
            public static bool IsPointInside(Vector2 point, Vector2 t1, Vector2 t2, Vector2 t3)
            {
                var b1 = Sign3(in point, in t1, in t2) < 0.0f;
                var b2 = Sign3(in point, in t2, in t3) < 0.0f;
                var b3 = Sign3(in point, in t3, in t1) < 0.0f;

                return (b1 == b2) && (b2 == b3);
            }
            public static bool IsPointInside(in Vector2 point, in Vector2 t1, in Vector2 t2, in Vector2 t3)
            {
                var b1 = Sign3(in point, in t1, in t2) < 0.0f;
                var b2 = Sign3(in point, in t2, in t3) < 0.0f;
                var b3 = Sign3(in point, in t3, in t1) < 0.0f;

                return (b1 == b2) && (b2 == b3);
            }
            private static float Sign3(in Vector2 p1, in Vector2 p2, in Vector2 p3)
            {
                return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
            }
            public static bool Overlap(Vector3 t1p1, Vector3 t1p2, Vector3 t1p3, Vector3 t2p1, Vector3 t2p2, Vector3 t2p3)
            {
                return Overlap(in t1p1, in t1p2, in t1p3, in t2p1, in t2p2, in t2p3);
            }
            public static bool Overlap(in Vector3 t1p1, in Vector3 t1p2, in Vector3 t1p3, in Vector3 t2p1, in Vector3 t2p2, in Vector3 t2p3)
            {
                Vector2 isect1 = Vector2.zero, isect2 = Vector2.zero;

                // compute plane equation of triangle(v0,v1,v2) 
                var e1 = t1p2 - t1p1;
                var e2 = t1p3 - t1p1;
                var n1 = cross.Product(in e1, in e2);
                var d1 = -dot(in n1, in t1p1);
                // plane equation 1: N1.X+d1=0 */

                // put u0,u1,u2 into plane equation 1 to compute signed distances to the plane
                var du0 = dot(in n1, in t2p1) + d1;
                var du1 = dot(in n1, in t2p2) + d1;
                var du2 = dot(in n1, in t2p3) + d1;

                // coplanarity robustness check 
                if (abs(du0) < Mathf.Epsilon) { du0 = 0.0f; }
                if (abs(du1) < Mathf.Epsilon) { du1 = 0.0f; }
                if (abs(du2) < Mathf.Epsilon) { du2 = 0.0f; }

                var du0du1 = du0 * du1;
                var du0du2 = du0 * du2;

                // same sign on all of them + not equal 0 ? 
                if (du0du1 > 0.0f && du0du2 > 0.0f)
                {
                    // no intersection occurs
                    return false;
                }

                // compute plane of triangle (u0,u1,u2)
                e1 = t2p2 - t2p1;
                e2 = t2p3 - t2p1;
                var n2 = cross.Product(in e1, in e2);
                var d2 = -dot(in n2, in t2p1);

                // plane equation 2: N2.X+d2=0 
                // put v0,v1,v2 into plane equation 2
                var dv0 = dot(in n2, in t1p1) + d2;
                var dv1 = dot(in n2, in t1p2) + d2;
                var dv2 = dot(in n2, in t1p3) + d2;

                if (abs(dv0) < Mathf.Epsilon) { dv0 = 0.0f; }
                if (abs(dv1) < Mathf.Epsilon) { dv1 = 0.0f; }
                if (abs(dv2) < Mathf.Epsilon) { dv2 = 0.0f; }


                var dv0dv1 = dv0 * dv1;
                var dv0dv2 = dv0 * dv2;

                // same sign on all of them + not equal 0 ? 
                if (dv0dv1 > 0.0f && dv0dv2 > 0.0f)
                {
                    // no intersection occurs
                    return false;
                }

                // compute direction of intersection line 
                var dd = Vector3.Cross(n1, n2);

                // compute and index to the largest component of D 
                var max = abs(dd.x);
                short index = 0;
                var bb = abs(dd.y);
                var cc = abs(dd.z);
                if (bb > max) { max = bb; index = 1; }
                if (cc > max) { max = cc; index = 2; }

                // this is the simplified projection onto L
                var vp0 = t1p1[index];
                var vp1 = t1p2[index];
                var vp2 = t1p3[index];

                var up0 = t2p1[index];
                var up1 = t2p2[index];
                var up2 = t2p3[index];

                // compute interval for triangle 1 
                float a = 0, b = 0, c = 0, x0 = 0, x1 = 0;
                if (ComputeIntervals(vp0, vp1, vp2, dv0, dv1, dv2, dv0dv1, dv0dv2, ref a, ref b, ref c, ref x0, ref x1))
                {
                    return TriTriCoplanar(in n1, in t1p1, in t1p2, in t1p3, in t2p1, in t2p2, in t2p3);
                }

                // compute interval for triangle 2 
                float d = 0, e = 0, f = 0, y0 = 0, y1 = 0;
                if (ComputeIntervals(up0, up1, up2, du0, du1, du2, du0du1, du0du2, ref d, ref e, ref f, ref y0, ref y1))
                {
                    return TriTriCoplanar(in n1, in t1p1, in t1p2, in t1p3, in t2p1, in t2p2, in t2p3);
                }

                var xx = x0 * x1;
                var yy = y0 * y1;
                var xxyy = xx * yy;

                var tmp = a * xxyy;
                isect1.x = tmp + b * x1 * yy;
                isect1.y = tmp + c * x0 * yy;

                tmp = d * xxyy;
                isect2.x = tmp + e * xx * y1;
                isect2.y = tmp + f * xx * y0;

                Sort(ref isect1);
                Sort(ref isect2);

                return !(isect1.y < isect2.x || isect2.y < isect1.x);
            }
            private static bool ComputeIntervals(float VV0, float VV1, float VV2,
                               float D0, float D1, float D2, float D0D1, float D0D2,
                               ref float A, ref float B, ref float C, ref float X0, ref float X1)
            {
                if (D0D1 > 0.0f)
                {
                    // here we know that D0D2<=0.0 
                    // that is D0, D1 are on the same side, D2 on the other or on the plane 
                    A = VV2; B = (VV0 - VV2) * D2; C = (VV1 - VV2) * D2; X0 = D2 - D0; X1 = D2 - D1;
                }
                else if (D0D2 > 0.0f)
                {
                    // here we know that d0d1<=0.0 
                    A = VV1; B = (VV0 - VV1) * D1; C = (VV2 - VV1) * D1; X0 = D1 - D0; X1 = D1 - D2;
                }
                else if (D1 * D2 > 0.0f || D0 > 0.000001f || D0 < -0.000001f)
                {
                    // here we know that d0d1<=0.0 or that D0!=0.0 
                    A = VV0; B = (VV1 - VV0) * D0; C = (VV2 - VV0) * D0; X0 = D0 - D1; X1 = D0 - D2;
                }
                else if (D1 > 0.000001f || D1 < -0.000001f)
                {
                    A = VV1; B = (VV0 - VV1) * D1; C = (VV2 - VV1) * D1; X0 = D1 - D0; X1 = D1 - D2;
                }
                else if (D2 > 0.000001f || D2 < -0.000001f)
                {
                    A = VV2; B = (VV0 - VV2) * D2; C = (VV1 - VV2) * D2; X0 = D2 - D0; X1 = D2 - D1;
                }
                else
                {
                    return true;
                }

                return false;
            }
            private static bool TriTriCoplanar(in Vector3 N, in Vector3 v0, in Vector3 v1, in Vector3 v2, in Vector3 u0, in Vector3 u1, in Vector3 u2)
            {
                var A = new float[3];
                short i0, i1;

                // first project onto an axis-aligned plane, that maximizes the area
                // of the triangles, compute indices: i0,i1. 
                A[0] = abs(N[0]);
                A[1] = abs(N[1]);
                A[2] = abs(N[2]);
                if (A[0] > A[1])
                {
                    if (A[0] > A[2])
                    {
                        // A[0] is greatest
                        i0 = 1;
                        i1 = 2;
                    }
                    else
                    {
                        // A[2] is greatest
                        i0 = 0;
                        i1 = 1;
                    }
                }
                else
                {
                    if (A[2] > A[1])
                    {
                        // A[2] is greatest 
                        i0 = 0;
                        i1 = 1;
                    }
                    else
                    {
                        // A[1] is greatest 
                        i0 = 0;
                        i1 = 2;
                    }
                }

                // test all edges of triangle 1 against the edges of triangle 2 
                if (EdgeAgainstTriEdges(in v0, in v1, in u0, in u1, in u2, i0, i1)) { return true; }
                if (EdgeAgainstTriEdges(in v1, in v2, in u0, in u1, in u2, i0, i1)) { return true; }
                if (EdgeAgainstTriEdges(in v2, in v0, in u0, in u1, in u2, i0, i1)) { return true; }

                // finally, test if tri1 is totally contained in tri2 or vice versa 
                if (PointInTri(in v0, in u0, in u1, in u2, i0, i1)) { return true; }
                if (PointInTri(in u0, in v0, in v1, in v2, i0, i1)) { return true; }

                return false;
            }
            private static bool EdgeAgainstTriEdges(in Vector3 v0, in Vector3 v1, in Vector3 u0, in Vector3 u1, in Vector3 u2, short i0, short i1)
            {
                // test edge u0,u1 against v0,v1
                if (EdgeEdgeTest(in v0, in v1, in u0, in u1, i0, i1)) { return true; }

                // test edge u1,u2 against v0,v1 
                if (EdgeEdgeTest(in v0, in v1, in u1, in u2, i0, i1)) { return true; }

                // test edge u2,u1 against v0,v1 
                if (EdgeEdgeTest(in v0, in v1, in u2, in u0, i0, i1)) { return true; }

                return false;
            }
            private static bool EdgeEdgeTest(in Vector3 v0, in Vector3 v1, in Vector3 u0, in Vector3 u1, int i0, int i1)
            {
                var Ax = v1[i0] - v0[i0];
                var Ay = v1[i1] - v0[i1];

                var Bx = u0[i0] - u1[i0];
                var By = u0[i1] - u1[i1];
                var Cx = v0[i0] - u0[i0];
                var Cy = v0[i1] - u0[i1];
                var f = Ay * Bx - Ax * By;
                var d = By * Cx - Bx * Cy;
                if ((f > 0 && d >= 0 && d <= f) || (f < 0 && d <= 0 && d >= f))
                {
                    var e = Ax * Cy - Ay * Cx;
                    if (f > 0)
                    {
                        if (e >= 0 && e <= f) { return true; }
                    }
                    else
                    {
                        if (e <= 0 && e >= f) { return true; }
                    }
                }

                return false;
            }
            private static bool PointInTri(in Vector3 v0, in Vector3 u0, in Vector3 u1, in Vector3 u2, short i0, short i1)
            {
                // is T1 completly inside T2?
                // check if v0 is inside tri(u0,u1,u2)
                var a = u1[i1] - u0[i1];
                var b = -(u1[i0] - u0[i0]);
                var c = -a * u0[i0] - b * u0[i1];
                var d0 = a * v0[i0] + b * v0[i1] + c;

                a = u2[i1] - u1[i1];
                b = -(u2[i0] - u1[i0]);
                c = -a * u1[i0] - b * u1[i1];
                var d1 = a * v0[i0] + b * v0[i1] + c;

                a = u0[i1] - u2[i1];
                b = -(u0[i0] - u2[i0]);
                c = -a * u2[i0] - b * u2[i1];
                var d2 = a * v0[i0] + b * v0[i1] + c;

                if (d0 * d1 > 0.0f)
                {
                    if (d0 * d2 > 0.0f) { return true; }
                }

                return false;
            }
            private static void Sort(ref Vector2 v)
            {
                if (v.x > v.y)
                {
                    var temp = v.x;
                    v.x = v.y;
                    v.y = temp;
                }
            }
        }

    }
}