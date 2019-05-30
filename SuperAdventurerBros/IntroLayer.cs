using System;
using Cocos2D;
using Microsoft.Xna.Framework;

namespace SuperAdventurerBros
{
    public class IntroLayer : CCLayerColor
    {
        CCSprite sprite;

        public IntroLayer()
        {
            // setup our color for the background
            Color = new CCColor3B(Microsoft.Xna.Framework.Color.Gray);
            Opacity = 255;

            initAdventurer();
            initLabel();
        }
        private void initAdventurer()
        {
            // Adventurer Idle
            sprite = new CCSprite();
            sprite.Position = CCDirector.SharedDirector.WinSize.Center;
            sprite.Scale = 4f;
            AddChild(sprite);
            
            var spriteSheet = new CCSpriteSheet("sprites/idle/adventurer-idle-0.plist");

            // Find all frames whose name contains "walk". walk1, walk2, walk3, etc.
            var idleFrames = spriteSheet.Frames.FindAll(x => x.Texture.Name.Name.Contains("idle"));

            // Create an animation that cycles through the walk frames.
            var idleAnimation = new CCAnimation(idleFrames, 0.1f);

            // Create an action that repeats the animation forever. 
            var idleRepeat = new CCRepeatForever(new CCAnimate(idleAnimation));

            // Add the repeating animation action to the sprite.
            sprite.RunAction(idleRepeat);
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

