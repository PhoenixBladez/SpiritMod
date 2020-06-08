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
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
    public class HellEater : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Gluttonous Devourer");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Pixie];
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults() {
            npc.width = 56;
            npc.height = 46;
            npc.damage = 30;
            npc.defense = 16;
            npc.lifeMax = 112;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 300f;
            npc.knockBackResist = .25f;
            npc.aiStyle = 85;
            npc.noGravity = true;
            aiType = NPCID.StardustCellBig;
            animationType = NPCID.Pixie;
            npc.lavaImmune = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            return spawnInfo.player.ZoneUnderworldHeight && NPC.downedBoss3 ? 0.18f : 0f;
        }

        public override void HitEffect(int hitDirection, double damage) {
            for(int k = 0; k < 20; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 6, 2.5f * hitDirection, -2.5f, 117, default(Color), .6f);
            }
            if(npc.life <= 0) {
                for(int i = 0; i < 20; i++) {
                    int num = Dust.NewDust(npc.position, npc.width, npc.height, 6, 0f, -2f, 117, default(Color), .6f);
                    Main.dust[num].noGravity = true;
                    Dust expr_62_cp_0 = Main.dust[num];
                    expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
                    Dust expr_92_cp_0 = Main.dust[num];
                    expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
                    if(Main.dust[num].position != npc.Center) {
                        Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
                    }
                }
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EaterGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EaterGore2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EaterGore2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EaterGore2"), 1f);
            }
        }
        int dashtimer;
        public override void AI() {
            Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.1f, 0.04f, 0.02f);

            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
            Main.dust[dust].noGravity = true;

            Vector2 direction = Main.player[npc.target].Center - npc.Center;
            direction.Normalize();
            npc.velocity *= 0.98f;
            dashtimer++;
            if(dashtimer >= 180) {
                dashtimer = 0;
                direction.X = direction.X * Main.rand.Next(8, 11);
                direction.Y = direction.Y * Main.rand.Next(8, 11);
                npc.velocity.X = direction.X;
                npc.velocity.Y = direction.Y;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height * 0.5f));
            for(int k = 0; k < npc.oldPos.Length; k++) {
                var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
            return true;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit) {
            if(Main.rand.Next(3) == 0) {
                target.AddBuff(BuffID.OnFire, 180);
            }
            target.AddBuff(BuffID.Bleeding, 180);
            if(npc.life <= npc.lifeMax - 10) {
                npc.life += 10;
                npc.HealEffect(10, true);
            } else if(npc.life < npc.lifeMax) {
                npc.HealEffect(npc.lifeMax - npc.life, true);
                npc.life += npc.lifeMax - npc.life;
            }
        }

        public override void NPCLoot() {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CarvedRock>(), Main.rand.Next(2) + 2);

            if(Main.rand.Next(20) == 0) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessory.HellEater>());
            }
        }
    }
}
