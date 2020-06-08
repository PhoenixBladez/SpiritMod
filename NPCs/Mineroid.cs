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
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
    public class Mineroid : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Orbitite");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults() {
            npc.width = 40;
            npc.height = 40;
            npc.damage = 32;
            npc.defense = 15;
            npc.lifeMax = 70;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 110f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = .45f;
            npc.aiStyle = 44;
            aiType = NPCID.FlyingAntlion;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            return (spawnInfo.player.ZoneMeteor) && spawnInfo.spawnTileY < Main.rockLayer & NPC.downedBoss2 ? 0.15f : 0f;
        }

        public override void NPCLoot() {
            if(NPC.downedBoss2) {
                if(Main.rand.Next(20) == 0) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<OrbiterStaff>());
                }
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Meteorite, Main.rand.Next(1, 2));
            }
            if(Main.rand.Next(1) == 400) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GravityModulator>());
            }
            string[] lootTable = { "AstronautLegs", "AstronautHelm", "AstronautBody" };
            if(Main.rand.Next(40) == 0) {
                int loot = Main.rand.Next(lootTable.Length);
                {
                    npc.DropItem(mod.ItemType(lootTable[loot]));
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Mineroid_Glow"));
        }
        public override void HitEffect(int hitDirection, double damage) {
            for(int k = 0; k < 11; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 24, hitDirection, -1f, 0, default(Color), .61f);
            }
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mineroid/Mineroid1"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mineroid/Mineroid2"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mineroid/Mineroid3"));
                Gore.NewGore(npc.position, npc.velocity, 61);
                Gore.NewGore(npc.position, npc.velocity, 62);
                Gore.NewGore(npc.position, npc.velocity, 63);
                npc.position.X = npc.position.X + (float)(npc.width / 2);
                npc.position.Y = npc.position.Y + (float)(npc.height / 2);
                npc.width = 30;
                npc.height = 30;
                npc.position.X = npc.position.X - (float)(npc.width / 2);
                npc.position.Y = npc.position.Y - (float)(npc.height / 2);
                for(int num621 = 0; num621 < 20; num621++) {
                    int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 24, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[num622].velocity *= 3f;
                    if(Main.rand.Next(2) == 0) {
                        Main.dust[num622].scale = 0.5f;
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.15f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }
        int timer = 0;
        public override void AI() {
            timer++;
            if(timer >= 90) {
                bool expertMode = Main.expertMode;
                int damage = expertMode ? 10 : 16;
                Vector2 vector2_2 = Vector2.UnitY.RotatedByRandom(1.57079637050629f) * new Vector2(5f, 3f);
                int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, vector2_2.X, vector2_2.Y, mod.ProjectileType("MeteorShardHostile1"), damage, 0.0f, Main.myPlayer, 0.0f, (float)npc.whoAmI);
                Main.projectile[p].hostile = true;
                timer = 0;
            }

            npc.spriteDirection = npc.direction;
        }
    }
}
