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
		public override void SetStaticDefaults() => DisplayName.SetDefault("Arcbolt");
		private static readonly int maxtimeleft = 200;
		private readonly static int numY = -16;

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = maxtimeleft;
            Projectile.height = 8;
            Projectile.minion = true;
            Projectile.width = 8;
            Projectile.alpha = 255;
            Projectile.hide = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = false;
            Projectile.localNPCHitCooldown = 1;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            int num = 5;
            for (int k = 0; k < 6; k++)
            {
                int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.Electric, 0.0f, 0.0f, 0, new Color(), .8f);
                Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
                Main.dust[index2].scale = .5f;
                Main.dust[index2].velocity *= 0f;
                Main.dust[index2].noGravity = true;
                Main.dust[index2].noLight = false;
                Main.dust[index2].fadeIn = (float)(100 + Projectile.owner);

            }

            if (Projectile.timeLeft % 2 == 0)
                Projectile.velocity.Y -= (Projectile.timeLeft == maxtimeleft) ? numY/2 : numY;
            else
            {
                Projectile.velocity.Y += numY;
				if(Projectile.timeLeft <= (maxtimeleft * 0.9f)) {
					Projectile.velocity.Y += 0.25f;
				}
            }
        }

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int i = 0; i < 10; i++)
			{
				int num = Dust.NewDust(target.position, target.width, target.height, DustID.Electric, 0f, -2f, 0, default, 1.952f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .25f;
				if (Main.dust[num].position != target.Center)
					Main.dust[num].velocity = target.DirectionTo(Main.dust[num].position) * 3f;
			}
			Player player = Main.player[Projectile.owner];
			target.AddBuff(Mod.Find<ModBuff>("ElectricSummonTag").Type, 8 * 60 * 60, true);
			int num1 = -1;
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].CanBeChasedBy(player, false) && Main.npc[i] == target)
				{
					num1 = i;
				}
			}
			player.MinionAttackTargetNPC = num1;
		}
	}
}
