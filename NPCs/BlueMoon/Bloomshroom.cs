using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.NPCs.Dungeon;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon
{
    public class Bloomshroom : ModNPC
    {
        bool attack = false;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Bloomshroom");
            Main.npcFrameCount[npc.type] = 12;
        }

        public override void SetDefaults() {
            npc.width = 50;
            npc.height = 54;
            npc.damage = 29;
            npc.defense = 16;
            npc.lifeMax = 140;
            npc.HitSound = SoundID.NPCHit2;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.value = 1000f;
            npc.knockBackResist = .35f;
        }


        public override void HitEffect(int hitDirection, double damage) {
            int d = 37;
            int d1 = 75;
            for(int k = 0; k < 30; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
            }
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PDoctor1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PDoctor2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PDoctor3"), 1f);
            }
        }


        int frame = 0;
        int timer = 0;
        //  int shootTimer = 0;
        public override void AI() {
            npc.spriteDirection = npc.direction;
            Player target = Main.player[npc.target];
            int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
            if(distance < 200) {
                attack = true;
            }
            if(distance > 250) {
                attack = false;
            }
            if(attack) {
                npc.velocity.X = .008f * npc.direction;
                //shootTimer++;
                if(frame == 9 && timer == 0) {
                     Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    float ai = Main.rand.Next(100);
                    direction.Normalize();
                    int MechBat = Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -6, ModContent.ProjectileType<Pollen>(), 31, 0);
                    int MechBat1 = Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 3, -6, ModContent.ProjectileType<Pollen>(), 31, 0);
                    if(Main.rand.Next(3) == 0) {
                        int MechBat2 = Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -3, -6, ModContent.ProjectileType<Pollen>(), 31, 0);
                    }
                    timer++;
                }
                timer++;
                if(timer >= 12) {
                    frame++;
                    timer = 0;
                }
                if(frame > 11) {
                    frame = 7;
                }
                 if(frame < 7) {
                    frame = 7;
                }
                if(target.position.X > npc.position.X) {
                    npc.direction = 1;
                } else {
                    npc.direction = -1;
                }
            } else {
                //shootTimer = 0;
                npc.aiStyle = 26;
                aiType = NPCID.Skeleton;
                timer++;
                if(timer >= 4) {
                    frame++;
                    timer = 0;
                }
                if(frame > 6) {
                    frame = 0;
                }
            }
            /*if (shootTimer > 120)
            {
                shootTimer = 120;
            }
            if (shootTimer < 0)
            {
                shootTimer = 0;
            }*/
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void FindFrame(int frameHeight) {
            npc.frame.Y = frameHeight * frame;
        }
    }
}
