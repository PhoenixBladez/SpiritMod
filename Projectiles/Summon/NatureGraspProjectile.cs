using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

namespace SpiritMod.Projectiles.Summon
{
    class NatureGraspProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nature's Grasp");
        }

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 80;
            projectile.height = 6;
            projectile.minion = true;
            projectile.width = 6;
            projectile.alpha = 255;
            projectile.penetrate = 1;
            projectile.extraUpdates = 2;
            projectile.usesLocalNPCImmunity = false;
            projectile.localNPCHitCooldown = 1;
            aiType = ProjectileID.Bullet;
        }

        int counter;
        public override void AI()
        {
            Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.32f/3, .65f/3, 0.32f/3);
            projectile.ai[0]+= .5f;
            counter++;
            if (counter >= 120)
                counter = -120;
            else
            {
                for (int i = 0; i < 13; i++)
                {
                    int num = Dust.NewDust(projectile.Center + new Vector2(0, (float)Math.Cos(counter / 4.2f) * 16.2f).RotatedBy(projectile.rotation), 6, 6, DustID.SapphireBolt, 0f, 0f, 0, default, .65f);
                    Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(18, Main.LocalPlayer);
                    Main.dust[num].velocity *= .1f;
                    Main.dust[num].scale = MathHelper.Clamp(1.25f, .2f, 10/projectile.ai[0]);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].fadeIn = (100 + projectile.owner);
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            target.AddBuff(mod.BuffType("SummonTag3"), 240, true);
            int num = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
                if (Main.npc[i].CanBeChasedBy(player, false) && Main.npc[i] == target)
                    num = i;
            player.MinionAttackTargetNPC = num;
        }
	}
}
