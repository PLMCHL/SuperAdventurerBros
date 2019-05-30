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
            // Adventurer Idle
            sprite = new CCSprite("sprites/idle/adventurer-idle-00");
            sprite.Position = CCDirector.SharedDirector.WinSize.Center;
            sprite.Scale = 4f;
            AddChild(sprite);
            
            // create and initialize a Label
            var label = new CCLabelTTF("Super Adventurer Bros", "MarkerFelt", 22);

            // position the label on the center of the screen
            label.PositionX = CCDirector.SharedDirector.WinSize.Center.X;
            label.PositionY = CCDirector.SharedDirector.WinSize.Height - 100;
            label.Color = new CCColor3B(Microsoft.Xna.Framework.Color.DarkRed);

            // add the label as a child to this Layer
            AddChild(label);

            // setup our color for the background
            Color = new CCColor3B(Microsoft.Xna.Framework.Color.Gray);
            Opacity = 255;

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

