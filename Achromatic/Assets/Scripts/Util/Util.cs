using System.Linq;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace SOO
{
    public static class Util
    {
        public static Vector3[] ToVector3(this Vector2[] vectors)
            => System.Array.ConvertAll<Vector2, Vector3>(vectors, v => v);

        public static Vector2 Centroid(this ICollection<Vector2> vectors)
            => vectors.Aggregate((agg, next) => agg + next) / vectors.Count();

        public static void Set(this Vector2 vector, Vector2 newVector) => vector = newVector;

        public static Vector2 Abs(this Vector2 vector)
            => new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));

        public static float LengthSq(this Vector2 vector)
            => vector.x * vector.x + vector.y * vector.y;

        public static Vector2 neg(this Vector2 vector)
            => -vector;

        public static Vector2 Cross(Vector2 vector, float a, Vector2 output)
        {
            output.x = vector.y * a;
            output.y = vector.x * -a;
            return output;
        }

        public static Vector2 Cross(float a, Vector2 vector, Vector2 output)
        {
            output.x = vector.y * -a;
            output.y = vector.x * a;
            return output;
        }

        public static float Cross(Vector2 a, Vector2 b)
            => a.x * b.y - a.y * b.x;

        public static float Cross(Vector2 a, Vector2 b, Vector2 c)
            => (c.y - a.y) * (b.x - a.x) - (c.x - a.x) * (b.y - a.y);

        public static float Dot(Vector2 a, Vector2 b)
            => a.x * b.x + a.y * b.y;

        public static float Dot(Vector2 a, Vector2 b, Vector2 c)
            => (c.x - a.x) * (b.x - a.x) + (c.y - a.y) * (b.y - a.y);


        public static float Distance(Vector2 a, Vector2 b)
            => (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);

        public static float IsBetween(Vector2 a, Vector2 b, Vector2 c)
        {
            float result = Distance(a, b) + Distance(b, c) - Distance(a, c);
            return result;
            //return -Mathf.Epsilon < result && result < Mathf.Epsilon;
        }

        public static float distanceSq(Vector2 a, Vector2 b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;

            return dx * dx + dy * dy;
        }

        //정적 클래스는 사용자 정의 연산자 (연산자 오버로딩)을 포함할 수 없다.

        //string은 일반적으론 불변성을 띄고있어서 대상 문자열을 참조만 하고있다.
        // + 연산을 쓰게된다면 매번 문자열 이어붙이기 연산을 위해서 새로운 string객체를 만들게 되는것
        public static string StringBuilder(params string[] str)
        {
            StringBuilder strBuilder = new StringBuilder(str[0]);

            if (str.Length <= 1)
                return strBuilder.ToString();

            for (int i = 1; i < str.Length; i++)
                strBuilder.Append(str[i]);

            return strBuilder.ToString();
        }

        public static string StringBuilder(string str, int i)
        {
            StringBuilder strBuilder = new StringBuilder(str);
            return strBuilder.Append(i).ToString();
        }

        public static T[] ListToArray<T>(this List<T> list)
        {
            T[] array = new T[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                array[i] = list[i];
            }

            return array;
        }

        public static List<T> ArrayToList<T>(this T[] array)
        {
            List<T> list = new List<T>(array.Length);

            for (int i = 0; i < array.Length; i++)
            {
                list.Add(array[i]);
            }

            return list;
        }

        public static Vector2 AngleToVector(float angle)
        {
            angle *= Mathf.Deg2Rad;

            Vector2 vec =
                new Vector2(
                    -Mathf.Sin(angle),
                    Mathf.Cos(angle)
                    );

            return vec;
        }

        /// <summary>
        /// 베지어 곡선
        /// </summary>
        /// <param name="pointCount"></param>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static Vector2[] CurvePointsOfVectors(int pointCount, params Vector2[] vec)
        {
            Vector2[] points = new Vector2[pointCount + 1];
            float unit = 1.0f / pointCount;

            int n = vec.Length - 1;
            float[] t = new float[n];
            float[] u = new float[n];
            t[0] = 0f;
            for (int i = 0; i < pointCount + 1; i++, t[0] += unit)
            {
                u[0] = (1 - t[0]);
                for (int j = 1; j < n; j++)
                {
                    t[j] = t[0] * t[j - 1];
                    u[j] = u[0] * u[j - 1];
                }

                points[i] =
                    vec[0] * u[n - 1] + vec[n] * t[n - 1];
                for (int j = 1; j < n; j++)
                {
                    points[i] += vec[j] * (t[j - 1] * u[n - 1 - j] * n);
                }
            }

            return points;
        }

        /// 교점알고리즘 
        /// <summary>
        /// 두 선이 교차하면 해당 점을 리턴
        /// </summary>
        /// <param name="aP1">선 a의 점1</param>
        /// <param name="aP2">선 a의 점2</param>
        /// <param name="bP1">선 b의 점1</param>
        /// <param name="bP2">선 b의 점2</param>
        /// <returns>교차되는 점</returns>
        public static Vector2? GetIntersectPosition(Vector2 aP1, Vector2 aP2, Vector2 bP1, Vector2 bP2)
        {
            var under = (bP2.y - bP1.y) * (aP2.x - aP1.x) - (bP2.x - aP1.x) * (aP2.y - aP1.y);
            if(Mathf.Abs(under) < Mathf.Epsilon)
            {
                return null;
            }

            var t1 = (bP2.x - bP1.x) * (aP1.y - bP1.y) - (bP2.y - bP1.y) * (aP1.x - bP1.x);
            var s1 = (aP2.x - aP1.x) * (aP1.y - bP1.y) - (aP2.y - aP1.y) * (aP1.x - bP1.x);
            if (Mathf.Abs(t1) < Mathf.Epsilon && Mathf.Abs(s1) < Mathf.Epsilon)
            {
                return null;
            }

            var t2 = t1 / under;
            var s2 = s1 / under;
            if (t2 < 0f || t2 > 1.0f || s2 < 0f || s2 > 1f)
            {
                return null;
            }

            var intersectionX = aP1.x + t2 * (aP2.x - aP1.x);
            var intersectionY = aP1.y + t2 * (aP2.y - aP1.y);
            return new Vector2(intersectionX, intersectionY);
        }

        public static Sprite TextureToSprite(this Texture2D texture)
           => Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        public static Sprite TextureToSprite(this Texture2D texture, Rect rect)
           => Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

    }
}