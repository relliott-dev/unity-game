using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LaireonGames
{
    public class Curve3D
    {
        public Curve curveX = new Curve();
        public Curve curveY = new Curve();
        public Curve curveZ = new Curve();

        Vector3 offset = Vector3.zero;

        public Curve3D()
        {
        }

        public void Reset()
        {
            curveX.Reset();
            curveY.Reset();
            curveZ.Reset();
        }

        public void AddPoint(Vector3 point, float time)
        {
            curveX.Keys.Add(new CurveKey(time, point.x));
            curveY.Keys.Add(new CurveKey(time, point.y));
            curveZ.Keys.Add(new CurveKey(time, point.z));
        }

        /// <summary>
        /// Basically takes a given curve and moves all of its points
        /// </summary>
        /// <param name="offset"></param>
        public void OffsetCurves(Vector3 offset)
        {
            this.offset = offset;
        }

        public void SetTangents()
        {
            CurveKey prev;
            CurveKey current;
            CurveKey next;
            int prevIndex;
            int nextIndex;
            for(int i = 0; i < curveX.Keys.Count; i++)
            {
                prevIndex = i - 1;
                if(prevIndex < 0) prevIndex = i;

                nextIndex = i + 1;
                if(nextIndex == curveX.Keys.Count) nextIndex = i;

                prev = curveX.Keys[prevIndex];
                next = curveX.Keys[nextIndex];
                current = curveX.Keys[i];
                SetCurveKeyTangent(ref prev, ref current, ref next);
                curveX.Keys[i] = current;
                prev = curveY.Keys[prevIndex];
                next = curveY.Keys[nextIndex];
                current = curveY.Keys[i];
                SetCurveKeyTangent(ref prev, ref current, ref next);
                curveY.Keys[i] = current;

                prev = curveZ.Keys[prevIndex];
                next = curveZ.Keys[nextIndex];
                current = curveZ.Keys[i];
                SetCurveKeyTangent(ref prev, ref current, ref next);
                curveZ.Keys[i] = current;
            }
        }

        void SetCurveKeyTangent(ref CurveKey prev, ref CurveKey cur,
            ref CurveKey next)
        {
            float dt = next.Position - prev.Position;
            float dv = next.Value - prev.Value;
            if(Math.Abs(dv) < float.Epsilon)
            {
                cur.TangentIn = 0;
                cur.TangentOut = 0;
            }
            else
            {
                // The in and out tangents should be equal to the 
                // slope between the adjacent keys.
                cur.TangentIn = dv * (cur.Position - prev.Position) / dt;
                cur.TangentOut = dv * (next.Position - cur.Position) / dt;
            }
        }

        public Vector3 GetPointOnCurve(float time)
        {
            return offset + new Vector3(curveX.Evaluate(time), curveY.Evaluate(time), curveZ.Evaluate(time));
        }
    }
}