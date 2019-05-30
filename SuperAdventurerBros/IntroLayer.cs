using System;
using Cocos2D;
using Box2D.Dynamics;
using Box2D.Common;
using Box2D.Collision.Shapes;
using System.Collections.Generic;

namespace SuperAdventurerBros
{
    public class IntroLayer : CCLayerColor
    {
        const int PTM_RATIO = 32;
        private String IDLE_HAND_SPRITE_SHEET = "sprites/idle/adventurer-idle-0.plist";
        private String IDLE_SWORD_SPRITE_SHEET = "sprites/idle/adventurer-idle-1.plist";

        private Random random = new Random();
        private List<CCPhysicsSprite> sprites = new List<CCPhysicsSprite>();

        private CCPhysicsSprite heroSprite;
        private b2World world;

        Box2DDebug debugDraw;

        public IntroLayer()
        {
            // setup our color for the background
            Color = new CCColor3B(Microsoft.Xna.Framework.Color.Gray);
            Opacity = 255;

            initPhysics();
            initAdventurer();
            initLabel();

            // StartDebugging();

            Schedule(Run);
        }
        private void Run(float time)
        {
            world.Step(time, 8, 8);

            foreach (var sprite in sprites)
            {
                sprite.UpdateBodyTransform();
            }
            
            RenderDebug();
        }
        void StartDebugging()
        {
            var debugNode = new CCDrawNode();
            AddChild(debugNode, 1000);
            debugDraw = new Box2DDebug(debugNode, PTM_RATIO);

            debugDraw.Flags = b2DrawFlags.e_shapeBit | b2DrawFlags.e_aabbBit | b2DrawFlags.e_centerOfMassBit | b2DrawFlags.e_jointBit;
            world.SetDebugDraw(debugDraw);
        }

        void RenderDebug()
        {
            if (debugDraw != null)
            {
                debugDraw.Begin();
                world.DrawDebugData();
                debugDraw.End();
            }
        }

        void initPhysics()
        {
            var gravity = new b2Vec2(0.0f, -10.0f);
            world = new b2World(gravity);

            world.SetAllowSleeping(true);
            world.SetContinuousPhysics(true);

            var def = new b2BodyDef();
            def.allowSleep = true;
            def.position = b2Vec2.Zero;
            def.type = b2BodyType.b2_staticBody;
            b2Body groundBody = world.CreateBody(def);
            groundBody.SetActive(true);
            
            b2EdgeShape groundBox = new b2EdgeShape();
            groundBox.Set(b2Vec2.Zero, new b2Vec2(CCDirector.SharedDirector.WinSize.Width / PTM_RATIO, 10 / PTM_RATIO)); // TODO: remove slant

            b2FixtureDef fd = new b2FixtureDef();
            fd.friction = 0.01f;
            fd.restitution = 0.1f;
            fd.shape = groundBox;

            groundBody.CreateFixture(fd);
        }

        private void initAdventurer()
        {
            var startingPosition = CCDirector.SharedDirector.WinSize.Center;

            // Creating sprite
            heroSprite = new CCPhysicsSprite(PTM_RATIO)
            {
                Position = startingPosition,
                Scale = 3f
            };
            
            // Add the repeating animation action to the sprite.
            heroSprite.RunAction(getIdleRepeat());
            
            // Add physics
            b2Vec2 positionVec = new b2Vec2(startingPosition.X, startingPosition.Y);
            var def = new b2BodyDef();
            def.position = new b2Vec2(positionVec.x / PTM_RATIO, positionVec.y / PTM_RATIO);
            def.linearVelocity = new b2Vec2(3f, 10.0f);
            def.type = b2BodyType.b2_dynamicBody;
            b2Body heroBody = world.CreateBody(def);
            heroSprite.PhysicsBody = heroBody;

            b2Shape heroBox;
            var circle = true;
            if (circle)
            {
                var tempBox = new b2CircleShape();
                tempBox.Position = new b2Vec2(0, -20 / PTM_RATIO);
                tempBox.Radius = 1;
                heroBox = tempBox; 
            }
            else
            {
                // TODO: fix this box
                var tempBox = new b2PolygonShape();

                var b = 32;

                var a = new b2Vec2[4];
                a[0] = new b2Vec2(0 / PTM_RATIO, 0 / PTM_RATIO);
                a[1] = new b2Vec2(0 / PTM_RATIO, b / PTM_RATIO);
                a[2] = new b2Vec2(b / PTM_RATIO, b / PTM_RATIO);
                a[3] = new b2Vec2(b / PTM_RATIO, 0 / PTM_RATIO);

                tempBox.Set(a, 4);
                heroBox = tempBox;
            }

            b2FixtureDef fd = new b2FixtureDef();
            fd.friction = 0.3f;
            fd.restitution = 0.1f;
            fd.shape = heroBox;

            heroBody.CreateFixture(fd);
           

            // Add sprite to internal list and render list
            sprites.Add(heroSprite);
            AddChild(heroSprite);
        }

        private CCRepeatForever getIdleRepeat()
        {
            // Select random sheet
            var sheet = IDLE_HAND_SPRITE_SHEET;
            if (random.Next(2) == 0)
            {
                sheet = IDLE_SWORD_SPRITE_SHEET;
            }

            // load spritesheet
            var spriteSheet = new CCSpriteSheet(sheet);

            // Find all frames whose name contains "idle"
            var idleFrames = spriteSheet.Frames.FindAll(x => x.Texture.Name.Name.Contains("idle"));

            // Create an animation that cycles through the idle frames.
            var idleAnimation = new CCAnimation(idleFrames, 0.18f);

            // Create an action that repeats the animation forever. 
            return new CCRepeatForever(new CCAnimate(idleAnimation));
        }

        private void initLabel()
        {
            // create and initialize a Label
            var label = new CCLabelTTF("Super Adventurer Bros", "MarkerFelt", 22);

            // position the label on the center of the screen
            label.PositionX = CCDirector.SharedDirector.WinSize.Center.X;
            label.PositionY = CCDirector.SharedDirector.WinSize.Height - 100;
            label.Color = new CCColor3B(Microsoft.Xna.Framework.Color.DarkRed);

            // add the label as a child to this Layer
            AddChild(label);
        }

        public static CCScene Scene
        {
            get
            {
                // 'scene' is an autorelease object.
                var scene = new CCScene();

                // 'layer' is an autorelease object.
                var layer = new IntroLayer();

                // add layer as a child to scene
                scene.AddChild(layer);

                // return the scene
                return scene;

            }

        }

    }
}

