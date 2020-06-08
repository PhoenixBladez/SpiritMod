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
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SpiritMod.Skies
{
    public class SpiritBiomeSky : CustomSky
    {
        private struct LightPillar
        {
            public Vector2 Position;

            public float Depth;
        }

        private LightPillar[] _pillars;

        private UnifiedRandom _random = new UnifiedRandom();

        private Texture2D _beamTexture;

        private Texture2D[] _rockTextures;

        private bool skyActive;

        private float opacity;

        public override void OnLoad() {
            Mod mod = SpiritMod.instance;
            this._beamTexture = TextureManager.Load("Images/Misc/NebulaSky/Beam");
            this._rockTextures = new Texture2D[3];
            for(int i = 0; i < this._rockTextures.Length; i++) {
                this._rockTextures[i] = mod.GetTexture("Textures/Soul" + i.ToString());
            }
        }

        public override void Update(GameTime gameTime) {
            if(skyActive && opacity < 1f) {
                opacity += 0.01f;
            } else if(!skyActive && opacity > 0f) {
                opacity -= 0.005f;
            }
        }
        public override Color OnTileColor(Color inColor) {
            float amt = opacity * .6f;
            return inColor.MultiplyRGB(new Color(1f - amt, 1f - amt, 1f - amt));
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth) {

            int num11 = -1;
            int num10 = 0;
            for(int j = 0; j < this._pillars.Length; j++) {
                float depth = this._pillars[j].Depth;
                if(num11 == -1 && depth < maxDepth) {
                    num11 = j;
                }
                if(depth <= minDepth) {
                    break;
                }
                num10 = j;
            }
            if(num11 != -1) {
                Vector2 value4 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
                Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
                float scale = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
                for(int i = num11; i < num10; i++) {
                    Vector2 value3 = new Vector2(1f / this._pillars[i].Depth, 0.9f / this._pillars[i].Depth);
                    Vector2 vector2 = this._pillars[i].Position;
                    vector2 = (vector2 - value4) * value3 + value4 - Main.screenPosition;
                    if(rectangle.Contains((int)vector2.X, (int)vector2.Y)) {
                        float num9 = value3.X * 450f;
                        spriteBatch.Draw(this._beamTexture, vector2, null, new Color(0, 200, 255) * 0.2f * scale * this.opacity, 0f, Vector2.Zero, new Vector2(num9 / 70f, num9 / 45f), SpriteEffects.None, 0f);
                        int num8 = 0;
                    }
                }
            }
        }

        public override float GetCloudAlpha() {
            return (1f - this.opacity) * 0.3f + 0.7f;
        }

        public override void Activate(Vector2 position, params object[] args) {
            this.opacity = 0.002f;
            this.skyActive = true;
            this._pillars = new LightPillar[40];
            for(int i = 0; i < this._pillars.Length; i++) {
                this._pillars[i].Position.X = (float)i / (float)this._pillars.Length * ((float)Main.maxTilesX * 16f + 20000f) + this._random.NextFloat() * 40f - 20f - 20000f;
                this._pillars[i].Position.Y = this._random.NextFloat() * 200f - 2000f;
                this._pillars[i].Depth = this._random.NextFloat() * 8f + 7f;
            }
            Array.Sort(this._pillars, this.SortMethod);
        }

        private int SortMethod(LightPillar pillar1, LightPillar pillar2) {
            return pillar2.Depth.CompareTo(pillar1.Depth);
        }

        public override void Deactivate(params object[] args) {
            this.skyActive = false;
        }

        public override void Reset() {
            this.skyActive = false;
        }

        public override bool IsActive() {
            if(!this.skyActive) {
                return this.opacity > 0.001f;
            }
            return true;
        }
    }
}
