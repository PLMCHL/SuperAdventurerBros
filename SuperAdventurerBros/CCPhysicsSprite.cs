using System;
using Box2D.Common;
using Box2D.Dynamics;
using Cocos2D;

namespace SuperAdventurerBros
{
    internal class CCPhysicsSprite : CCSprite
    {
        readonly float ptmRatio;

        public CCPhysicsSprite(float ptmRatio) : base()
        {
            this.ptmRatio = ptmRatio;
        }

        public b2Body PhysicsBody { get; set; }

        public void UpdateBodyTransform()
        {
            if (PhysicsBody != null)
            {
                b2Vec2 pos = PhysicsBody.Position;

                float x = pos.x * ptmRatio;
                float y = pos.y * ptmRatio;

                if (IgnoreAnchorPointForPosition)
                {
                    x += AnchorPointInPoints.X * ptmRatio;
                    y += AnchorPointInPoints.Y * ptmRatio;
                }

                // Make matrix
                float radians = PhysicsBody.Angle;
                if (radians != 0)
                {
                    Rotation = CCMathHelper.ToDegrees(-radians);
                }

                PositionX = x;
                PositionY = y;
            }
        }
    }
}