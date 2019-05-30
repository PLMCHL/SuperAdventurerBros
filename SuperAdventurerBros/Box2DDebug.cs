using System;
using System.Collections.Generic;

using System.Text;

using Box2D.Collision;
using Box2D.Common;
using Cocos2D;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperAdventurerBros
{
    internal static class b2VecHelper
    {
        public static CCPoint ToCCPoint(this b2Vec2 vec)
        {
            return new CCPoint(vec.x, vec.y);
        }
        
        internal static CCColor4B ToCCColor4B(this b2Color color)
        {
            return new CCColor4B(color.r, color.g, color.b, 255);
        }

        internal static CCColor4F ToCCColor4F(this b2Color color)
        {
            return new CCColor4F(color.r, color.g, color.b, 255);
        }
    }

    public class Box2DDebug : b2Draw
    {
        CCDrawNode DrawNode { get; set; }

        #region Constructors

        public Box2DDebug(CCDrawNode drawNode, int ptmRatio) : base(ptmRatio)
        {
            DrawNode = drawNode;
            DrawNode.BlendFunc = CCBlendFunc.NonPremultiplied;
        }

        #endregion Constructors
        
        public override void DrawPolygon(b2Vec2[] vertices, int vertexCount, b2Color color)
        {
            var transparent = new CCColor4F(0, 0, 0, 0);
            DrawNode.DrawPolygon(getPoints(vertices), vertexCount, transparent, 1, color.ToCCColor4F());
        }

        public override void DrawSolidPolygon(b2Vec2[] vertices, int vertexCount, b2Color color)
        {
            DrawNode.DrawPolygon(getPoints(vertices), vertexCount, color.ToCCColor4F(), 1, color.ToCCColor4F());
        }

        private CCPoint[] getPoints(b2Vec2[] vertices)
        {
            var ccVertices = new CCPoint[vertices.Length];

            for (int i = 0; i < vertices.Length - 1; i++)
            {
                ccVertices[i] = vertices[i].ToCCPoint() * PTMRatio;
            }

            return ccVertices;
        }
        
        public override void DrawCircle(b2Vec2 center, float radius, b2Color color)
        {
            var centr = center.ToCCPoint() * PTMRatio;
            var rad = radius * PTMRatio;
            DrawNode.DrawCircle(centr, rad, color.ToCCColor4B());
        }

        public override void DrawSolidCircle(b2Vec2 center, float radius, b2Vec2 axis, b2Color color)
        {
            var colorFill = color.ToCCColor4B(); // * 0.5f;
            var centr = center.ToCCPoint() * PTMRatio;
            var rad = radius * PTMRatio;
            DrawNode.DrawCircle(centr, rad, colorFill);
            DrawNode.DrawSegment(centr, centr + (axis.ToCCPoint() * PTMRatio) * radius, 3, color.ToCCColor4F());
        }

        public override void DrawSegment(b2Vec2 p1, b2Vec2 p2, b2Color color)
        {
            DrawNode.DrawSegment(p1.ToCCPoint() * PTMRatio, p2.ToCCPoint()* PTMRatio, 1, color.ToCCColor4F());

            //DrawNode.AddLineVertex(p1.ToVectorC4B(color.ToCCColor4B(), PTMRatio));
            //DrawNode.AddLineVertex(p2.ToVectorC4B(color.ToCCColor4B(), PTMRatio));
        }

        public override void DrawTransform(b2Transform xf)
        {
            const float axisScale = 0.4f;
            b2Vec2 p1 = xf.p;

            b2Vec2 p2 = p1 + axisScale * xf.q.GetXAxis();
            DrawSegment(p1, p2, new b2Color(1, 0, 0));

            p2 = p1 + axisScale * xf.q.GetYAxis();
            DrawSegment(p1, p2, new b2Color(0, 1, 0));
        }

        public void DrawPoint(b2Vec2 p, float size, b2Color color)
        {
            b2Vec2[] verts = new b2Vec2[4];
            float hs = size / 2.0f;
            verts[0] = p + new b2Vec2(-hs, -hs);
            verts[1] = p + new b2Vec2(hs, -hs);
            verts[2] = p + new b2Vec2(hs, hs);
            verts[3] = p + new b2Vec2(-hs, hs);

            DrawSolidPolygon(verts, 4, color);
        }

        public void DrawAABB(b2AABB aabb, b2Color p1)
        {
            b2Vec2[] verts = new b2Vec2[4];
            verts[0] = new b2Vec2(aabb.LowerBound.x, aabb.LowerBound.y);
            verts[1] = new b2Vec2(aabb.UpperBound.x, aabb.LowerBound.y);
            verts[2] = new b2Vec2(aabb.UpperBound.x, aabb.UpperBound.y);
            verts[3] = new b2Vec2(aabb.LowerBound.x, aabb.UpperBound.y);

            DrawPolygon(verts, 4, p1);
        }

        public void Begin()
        {
            if (DrawNode != null)
                DrawNode.Clear();
        }

        public void End()
        { }



    }
}