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
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BloodMoon
{
    public class FleshHound : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Flesh Hound");
            Main.npcFrameCount[npc.type] = 10;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults() {
            npc.width = 60;
            npc.height = 36;
            npc.damage = 28;
            npc.defense = 7;
            npc.lifeMax = 85;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath5;
            npc.value = 180f;
            npc.knockBackResist = .2f;
            npc.aiStyle = 3;
            aiType = NPCID.WalkingAntlion;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            return spawnInfo.spawnTileY < Main.rockLayer && (Main.bloodMoon) && NPC.downedBoss1 ? 0.12f : 0f;
        }

        public override void HitEffect(int hitDirection, double damage) {
            for(int k = 0; k < 40; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection * 2.5f, -1f, 0, default(Color), Main.rand.NextFloat(.45f, 1.15f));
            }
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hound1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hound2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hound2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hound2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hound2"), 1f);
                for(int k = 0; k < 40; k++) {
                    Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection * 2.5f, -1f, 0, default(Color), Main.rand.NextFloat(.45f, 1.15f));
                }
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor) {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            if(trailbehind) {
                Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
                for(int k = 0; k < npc.oldPos.Length; k++) {
                    Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                    Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                    spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
                }
            }
        }
        public override void NPCLoot() {
            if(Main.rand.Next(2) == 1) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<BloodFire>());
            }
        }

        public override void FindFrame(int frameHeight) {
            npc.frameCounter += num34616;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }
        int timer;
        bool trailbehind = false;
        float num34616;
        public override void AI() {
            npc.spriteDirection = npc.direction;
            timer++;
            if(timer == 400 && Main.netMode != 1) {
                Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 7);
                npc.netUpdate = true;
            }
            if(timer == 400 && Main.netMode != 1) {
                num34616 = .95f;
                Vector2 direction = Main.player[npc.target].Center - npc.Center;
                direction.Normalize();
                direction.X = direction.X * Main.rand.Next(8, 10);
                direction.Y = direction.Y * Main.rand.Next(2, 4);
                npc.velocity.X = direction.X;
                npc.velocity.Y = direction.Y;
                npc.velocity.Y *= 0.98f;
                npc.velocity.X *= 0.995f;
                npc.netUpdate = true;
                trailbehind = true;
                npc.knockBackResist = 0f;
            } else {
                num34616 = .55f;
            }
            if(timer >= 601) {
                timer = 0;
                npc.netUpdate = true;
                trailbehind = false;
                npc.knockBackResist = .2f;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit) {
            target.AddBuff(ModContent.BuffType<BCorrupt>(), 180);
        }
    }
}
