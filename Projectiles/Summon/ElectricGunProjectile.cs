using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
namespace SpiritMod.Projectiles.Summon
{
    class ElectricGunProjectile : ModProjectile
    {
        int timer = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sparkbeam Bolt");
        }

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 110;
            projectile.height = 6;
            projectile.minion = true;
            projectile.width = 6;
            projectile.alpha = 255;
            projectile.penetrate = 1;
            projectile.extraUpdates = 4;
            projectile.usesLocalNPCImmunity = false;
            projectile.localNPCHitCooldown = 1;
            aiType = ProjectileID.Bullet;
        }
        int counter;
        public override void AI()
        {
            projectile.ai[0]+= .5f;
            counter++;
            if (counter >= 120)
            {
                counter = -120;
            }
            else
            {
                for (int i = 0; i < 13; i++)
                {
                    float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                    float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;

                    int num = Dust.NewDust(projectile.Center + new Vector2(0, (float)Math.Acos(counter / 1.4f) * 8.2f).RotatedBy(projectile.rotation), 6, 6, 226, 0f, 0f, 0, default(Color), .65f);
                    Main.dust[num].velocity *= .1f;
                    Main.dust[num].noGravity = true;
                    Main.dust[num].fadeIn = (float)(100 + projectile.owner);
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 10; i++)
            {
                int num = Dust.NewDust(target.position, target.width, target.height, 226, 0f, -2f, 0, default(Color), .52f);
                Main.dust[num].noGravity = true;
                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].scale *= .25f;
                if (Main.dust[num].position != target.Center)
                    Main.dust[num].velocity = target.DirectionTo(Main.dust[num].position) * 6f;
            }
            Player player = Main.player[projectile.owner];
            target.AddBuff(mod.BuffType("SummonTag4"), 240, true);
            int num1 = -1;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].CanBeChasedBy(player, false) && Main.npc[i] == target)
                {
                    num1 = i;
                }
            }
            {
                player.MinionAttackTargetNPC = num1;
            }
        }
	}
}
