using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Furniture.Reach;
using SpiritMod.NPCs.Critters;
using SpiritMod.Mounts;
using SpiritMod.NPCs.Boss.SpiritCore;
using SpiritMod.Boss.SpiritCore;
using SpiritMod.Buffs.Candy;
using SpiritMod.Buffs.Potion;
using SpiritMod.Projectiles.Pet;
using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Arrow.Artifact;
using SpiritMod.Projectiles.Bullet.Crimbine;
using SpiritMod.Projectiles.Bullet;
using SpiritMod.Projectiles.Magic.Artifact;
using SpiritMod.Projectiles.Summon.Artifact;
using SpiritMod.Projectiles.Summon.LaserGate;
using SpiritMod.Projectiles.Flail;
using SpiritMod.Projectiles.Arrow;
using SpiritMod.Projectiles.Magic;
using SpiritMod.Projectiles.Sword.Artifact;
using SpiritMod.Projectiles.Summon.Dragon;
using SpiritMod.Projectiles.Sword;
using SpiritMod.Projectiles.Thrown.Artifact;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Projectiles.Returning;
using SpiritMod.Projectiles.Held;
using SpiritMod.Projectiles.Thrown;
using SpiritMod.Items.Equipment;
using SpiritMod.Projectiles.DonatorItems;
using SpiritMod.Buffs.Mount;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Projectiles.Yoyo;
using SpiritMod.Items.Weapon.Spear;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.NPCs.Boss;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pets;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Projectiles.Boss;
using SpiritMod.Items.BossBags;
using SpiritMod.Items.Consumable.Fish;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using SpiritMod.NPCs.Spirit;
using SpiritMod.Items.Consumable;
using SpiritMod.Tiles.Block;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Placeable.IceSculpture;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Buffs;
using SpiritMod.Items;
using SpiritMod.Items.Weapon;
using SpiritMod.Items.Weapon.Returning;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Accessory;

using SpiritMod.Items.Accessory.Leather;
using SpiritMod.Items.Ammo;
using SpiritMod.Items.Armor;
using SpiritMod.Dusts;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Artifact;
using SpiritMod.NPCs;
using SpiritMod.NPCs.Asteroid;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Tiles;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Ambient.IceSculpture;
using SpiritMod.Tiles.Ambient.ReachGrass;
using SpiritMod.Tiles.Ambient.ReachMicros;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.Utilities;


namespace SpiritMod.Skies.Overlays
{
    public class AshstormOverlay : Overlay
    {
        private Ash[] _ashes;

        public AshstormOverlay(EffectPriority priority = 0, RenderLayers layer = RenderLayers.TilesAndNPCs) : base(priority, layer) {
            _ashes = new Ash[2000];
        }

        public override void Activate(Vector2 position, params object[] args) {
            this.Mode = OverlayMode.FadeIn;
        }

        public override void Deactivate(params object[] args) {
            this.Mode = OverlayMode.FadeOut;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Mod mod = ModLoader.GetMod("SpiritMod");
            for(int i = 0; i < _ashes.Length; i++) {
                if(_ashes[i].active) {
                    Ash ash = _ashes[i];
                    float alpha = ash.timeLeft < 1f ? ash.timeLeft : 1f;
                    float scale = 1.3f;
                    float textureWidth = 10f; //change this to be the width of the sprite you eventually make
                    float textureHeight = 10f; //change this to be the height of the sprite you eventually make
                    Texture2D spriteTexture = mod.GetTexture("Textures/Ash");
                    Rectangle spriteSource = new Rectangle(761, 1, 6, 6);
                    Color c = Lighting.GetColor((int)(ash.center.X), (int)(ash.center.Y));

                    spriteBatch.Draw(spriteTexture, ash.center - Main.screenPosition, spriteSource, new Color(20, 20, 20), ash.rotation, new Vector2(textureWidth * 0.5f, textureHeight * 0.5f), scale, SpriteEffects.None, 0f);

                }
            }
        }

        public override bool IsVisible() {
            Player player = Main.LocalPlayer;
            if(!player.ZoneUnderworldHeight) {
                return false;
            }
            return true;
        }

        public override void Update(GameTime gameTime) {
            Player player = Main.LocalPlayer;
            if(!Main.gameMenu && player.ZoneUnderworldHeight) {
                int ashToSpawn = new UnifiedRandom().Next(10);
                for(int i = 0; i < ashToSpawn; i++) {
                    int index = 0;
                    while(index < _ashes.Length && _ashes[index].active) {
                        index++;
                    }

                    if(index >= _ashes.Length) break;

                    float spawnX = Main.windSpeed < 0f ? Main.screenPosition.X + Main.screenWidth + 100f : Main.screenPosition.X - 100f;
                    float spawnY = new UnifiedRandom().NextFloat(Main.screenPosition.Y - 500f, Main.screenPosition.Y + Main.screenHeight);

                    _ashes[index].active = true;
                    _ashes[index].center = new Vector2(spawnX, spawnY);
                    _ashes[index].velocity = new Vector2(Main.windSpeed * 70f, new UnifiedRandom().NextFloat(0.2f, 2f));
                    _ashes[index].rotation = new UnifiedRandom().NextFloat(0f, MathHelper.TwoPi);
                    _ashes[index].cinder = new UnifiedRandom().Next(150) == 0;
                    _ashes[index].timeLeft = 6f;
                }
                for(int i = 0; i < _ashes.Length; i++) {
                    if(_ashes[i].active) {
                        _ashes[i].UpdatePosition(gameTime);
                        Point tilePos = _ashes[i].center.ToTileCoordinates();
                        if(_ashes[i].timeLeft <= 0f || (Main.tile[tilePos.X, tilePos.Y] != null && Main.tile[tilePos.X, tilePos.Y].active() && Main.tileSolid[Main.tile[tilePos.X, tilePos.Y].type])) {
                            _ashes[i].active = false;
                        } else if(Main.tile[tilePos.X, tilePos.Y].liquid > 230) {
                            _ashes[i].velocity *= 0.9f;
                            _ashes[i].center.Y -= 0.3f;
                            _ashes[i].timeLeft -= 0.03f;
                            _ashes[i].cinder = false;
                        } else {
                            _ashes[i].velocity.Y += new UnifiedRandom().NextFloat(0.005f, 0.02f);
                            _ashes[i].rotation += Main.windSpeed * 0.142f;
                        }
                    }
                }
            }
        }

        private struct Ash
        {
            public bool active;
            public float rotation;
            public bool cinder;
            public Vector2 center;
            public Vector2 velocity;
            public float timeLeft;

            public void UpdatePosition(GameTime time) {
                center += velocity;
                timeLeft -= (float)time.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ID;
using Events;


namespace Events.Overlays
{
    public class AshstormOverlay : Overlay
    {
        private Ash[] _ashes;

        public AshstormOverlay(EffectPriority priority = 0, RenderLayers layer = RenderLayers.TilesAndNPCs) : base(priority, layer)
        {
            _ashes = new Ash[100];
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            this.Mode = OverlayMode.FadeIn;
        }

        public override void Deactivate(params object[] args)
        {
            this.Mode = OverlayMode.FadeOut;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
			Mod mod = ModLoader.GetMod("Events");
            for (int i = 0; i < _ashes.Length; i++)
            {
                if (_ashes[i].active)
                {
                    Ash ash = _ashes[i];
                    float alpha = ash.timeLeft < 1f ? ash.timeLeft : 1f;
                   	float scale = 1f;
					float textureWidth = 10f; //change this to be the width of the sprite you eventually make
					float textureHeight = 10f; //change this to be the height of the sprite you eventually make
					Texture2D spriteTexture = mod.GetTexture("Images/Misc/LuminousRain");
					Rectangle spriteSource = new Rectangle(761, 1, 6, 6);
					Color c = Lighting.GetColor((int)(ash.center.X / 16), (int)(ash.center.Y / 16));
            
					spriteBatch.Draw(spriteTexture, ash.center - Main.screenPosition, null, Color.White, ash.rotation, new Vector2(textureWidth * 0.5f, textureHeight * 0.5f), scale, SpriteEffects.None, 0f);
		
                }
            }
        }

        public override bool IsVisible()
        {
            return true;
        }

        public override void Update(GameTime gameTime)
        {
			if (!Main.gameMenu)
			{
            int ashToSpawn = new UnifiedRandom().Next(4);
            for (int i = 0; i < ashToSpawn; i++)
            {
                int index = 0;
                while (index < _ashes.Length && _ashes[index].active)
                {
                    index++;
                }

                if (index >= _ashes.Length) break;

                float spawnX = new UnifiedRandom().NextFloat(Main.screenPosition.X - 100, Main.screenPosition.X + Main.screenWidth + 100);
                float spawnY = Main.screenPosition.Y - 20;

                _ashes[index].active = true;
                _ashes[index].center = new Vector2(spawnX, spawnY);
                _ashes[index].velocity = new Vector2(Main.windSpeed * 6f, new UnifiedRandom().NextFloat(10f, 12f));
                _ashes[index].rotation = 0f;
                _ashes[index].cinder = new UnifiedRandom().Next(150) == 0;
                _ashes[index].timeLeft = 6f;
            }
            for (int i = 0; i < _ashes.Length; i++)
            {
                if (_ashes[i].active)
                {
                    _ashes[i].UpdatePosition(gameTime);
                    Point tilePos = _ashes[i].center.ToTileCoordinates();
                    if (_ashes[i].timeLeft <= 0f || (Main.tile[tilePos.X, tilePos.Y] != null && Main.tile[tilePos.X, tilePos.Y].active() && Main.tileSolid[Main.tile[tilePos.X, tilePos.Y].type]))
                    {
                        _ashes[i].active = false;
					}
                    else
                    {
                        _ashes[i].velocity.Y += new UnifiedRandom().NextFloat(5f, 12f);
                    }
                }
            }
			}
        }

        private struct Ash
        {
            public bool active;
            public float rotation;
            public bool cinder;
            public Vector2 center;
            public Vector2 velocity;
            public float timeLeft;

            public void UpdatePosition(GameTime time)
            {
                center += velocity;
                timeLeft -= (float)time.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}*/
