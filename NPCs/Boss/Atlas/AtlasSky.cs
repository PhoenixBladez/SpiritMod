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

namespace SpiritMod.NPCs.Boss.Atlas
{
    public class AtlasSky : CustomSky
    {
        private bool isActive = false;
        private float intensity = 0f;
        private int AtlasIndex = -1;

        public override void Update(GameTime gameTime) {
            if(isActive && intensity < 1f) {
                intensity += 0.02f;
            } else if(!isActive && intensity > 0f) {
                intensity -= 0.02f;
            }
        }

        private float GetIntensity() {
            if(this.UpdateAtlasIndex()) {
                float x = 0f;
                if(this.AtlasIndex != -1)
                    x = Vector2.Distance(Main.player[Main.myPlayer].Center, Main.npc[this.AtlasIndex].Center);

                return 1f - Utils.SmoothStep(3000f, 6000f, x);
            }
            return 0f;
        }


        public override Color OnTileColor(Color inColor) {
            float amt = intensity * .02f;
            return inColor.MultiplyRGB(new Color(1f - amt, 1f - amt, 1f - amt));
        }


        private bool UpdateAtlasIndex() {
            int AtlasType = ModLoader.GetMod("SpiritMod").NPCType("Atlas");
            if(AtlasIndex >= 0 && Main.npc[AtlasIndex].active && Main.npc[AtlasIndex].type == AtlasType)
                return true;

            AtlasIndex = -1;
            for(int i = 0; i < Main.npc.Length; i++) {
                if(Main.npc[i].active && Main.npc[i].type == AtlasType) {
                    AtlasIndex = i;
                    break;
                }
            }
            //this.DoGIndex = DoGIndex;
            return AtlasIndex != -1;
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth) {
            if(maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f) {
                spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(0.13f, 0.13f, 0.13f) * intensity);
            }
            //front of bg
            if(maxDepth >= 0 && minDepth < 0) {
                spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(0.3f, 0.3f, 0.3f) * 0.5f);
            }
        }

        public override float GetCloudAlpha() {
            return 0f;
        }

        public override void Activate(Vector2 position, params object[] args) {
            isActive = true;
        }

        public override void Deactivate(params object[] args) {
            isActive = false;
        }

        public override void Reset() {
            isActive = false;
        }

        public override bool IsActive() {
            return isActive || intensity > 0f;
        }
    }
}