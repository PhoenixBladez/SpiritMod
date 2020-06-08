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
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Critters.Algae
{
    public class GreenAlgae2 : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Bioluminescent Algae");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults() {
            npc.width = 6;
            npc.height = 6;
            npc.damage = 0;
            npc.defense = 1000;
            npc.lifeMax = 1;
            npc.aiStyle = -1;
            npc.npcSlots = 0;
            npc.noGravity = false;
            npc.alpha = 40;
            npc.behindTiles = true;
            npc.dontCountMe = true;
            npc.dontTakeDamage = true;
        }
        public float num42;
        int num = 0;
        bool collision = false;
        bool txt = false;
        int num1232;
        public override bool PreAI() {
            if(Main.dayTime) {
                num1232++;
                if(num1232 >= Main.rand.Next(100, 700)) {
                    npc.active = false;
                    npc.netUpdate = true;
                }
            }
            if(!txt) {
                for(int i = 0; i < 5; ++i) {
                    Vector2 dir = Main.player[npc.target].Center - npc.Center;
                    dir.Normalize();
                    dir *= 1;
                    string[] npcChoices = { "GreenAlgae1", "GreenAlgae3" };

                    int npcChoice = Main.rand.Next(npcChoices.Length);

                    int newNPC = NPC.NewNPC((int)npc.Center.X + (Main.rand.Next(-55, 55)), (int)npc.Center.Y + (Main.rand.Next(-20, 20)), mod.NPCType(npcChoices[npcChoice]), npc.whoAmI);
                    Main.npc[newNPC].velocity = dir;
                }
                txt = true;
                npc.netUpdate = true;
            }
            return true;
        }
        public override void AI() {
            num++;
            if(num >= Main.rand.Next(100, 400)) {
                num = 0;
            }
            if(!Main.dayTime) {
                Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.135f * 2, 0.255f * 2, .211f * 2);
            }
            npc.spriteDirection = -npc.direction;
            int npcXTile = (int)(npc.Center.X / 16);
            int npcYTile = (int)(npc.Center.Y / 16);
            for(int y = npcYTile; y > Math.Max(0, npcYTile - 100); y--) {
                if(Main.tile[npcXTile, y].liquid != 255) {
                    int liquid = (int)Main.tile[npcXTile, y].liquid;
                    float up = (liquid / 255f) * 16f;
                    npc.position.Y = (y + 1) * 16f - up;
                    break;
                }
            }
            if(!collision) {
                npc.velocity.X = .5f * Main.windSpeed;
            } else {
                npc.velocity.X = -.5f * Main.windSpeed;
            }
            if(npc.collideX || npc.collideY) {
                npc.velocity.X *= -1f;
                if(!collision) {
                    collision = true;
                } else {
                    collision = false;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) {
            drawColor = new Color(176 - (int)(num / 3 * 4), 255 - (int)(num / 3 * 4), 237 - (int)(num / 3 * 4), 255 - num);
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY - 8), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
    }
}
