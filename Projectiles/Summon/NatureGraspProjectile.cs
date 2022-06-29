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
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 80;
            Projectile.height = 6;
            Projectile.minion = true;
            Projectile.width = 6;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = false;
            Projectile.localNPCHitCooldown = 1;
            AIType = ProjectileID.Bullet;
        }

        int counter;
        public override void AI()
        {
            Lighting.AddLight((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), 0.32f/3, .65f/3, 0.32f/3);
            Projectile.ai[0]+= .5f;
            counter++;
            if (counter >= 120)
                counter = -120;
            else
            {
                for (int i = 0; i < 13; i++)
                {
                    int num = Dust.NewDust(Projectile.Center + new Vector2(0, (float)Math.Cos(counter / 4.2f) * 16.2f).RotatedBy(Projectile.rotation), 6, 6, DustID.GemSapphire, 0f, 0f, 0, default, .65f);
                    Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(18, Main.LocalPlayer);
                    Main.dust[num].velocity *= .1f;
                    Main.dust[num].scale = MathHelper.Clamp(1.25f, .2f, 10/Projectile.ai[0]);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].fadeIn = (100 + Projectile.owner);
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(Mod.Find<ModBuff>("SummonTag3").Type, 240, true);
            int num = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
                if (Main.npc[i].CanBeChasedBy(player, false) && Main.npc[i] == target)
                    num = i;
            player.MinionAttackTargetNPC = num;
        }
	}
}
