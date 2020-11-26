using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Projectiles.Magic;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;

namespace SpiritMod.Projectiles.Bullet
{
	public class MoonshotProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonshot");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.timeLeft = 999999;
		}

		bool firing = false;
		Vector2 direction = Vector2.Zero;
        int counter = 0;
        Vector2 holdOffset = new Vector2(0, -15);
        public override bool PreAI()
		{
            Player player = Main.player[projectile.owner];
            if (player.channel)
            {
				if (counter % 10 == 0 )
                {
                    direction = Main.MouseWorld - (player.Center - new Vector2(4, 4));
                    direction.Normalize();
                    direction *= 5f;
                }
				if (counter == 45 || counter == 140)
                {
                    Main.PlaySound(25, (int)projectile.position.X, (int)projectile.position.Y);
                }
                player.itemTime = 5;
                player.itemAnimation = 5;
                projectile.position = player.Center + holdOffset;
                player.velocity.X *= 0.97f;
                counter++;
                if (counter > 45)
                {
                    DoDustEffect(player.Center + holdOffset, 8f);
                }
                if (counter > 140)
                {
                    DoDustEffect(player.Center + holdOffset, 26f);
                }
            }
            else
            {
                if (counter < 45)
                {
                    Main.PlaySound(2, projectile.Center, 41);
                    direction *= 2.5f;
                    int proj = Projectile.NewProjectile(player.Center - new Vector2(4, 4), direction, 14, projectile.damage, projectile.knockBack/2, 0, projectile.owner);
                    Main.projectile[proj].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromMaliwanShockCommon = true;
                }
				else if (counter >= 45 && counter < 140)
                {
                    Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 109));
                    direction *= 2.25f;
                    Projectile.NewProjectile(player.Center - new Vector2(4, 4), direction, ModContent.ProjectileType<MoonshotBulletLarge>(), (int)(projectile.damage * 1.65f), 5, projectile.owner);
                }
				else if (counter > 140)
                {
                    direction *= .8f;
                    Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 109));
                    int proj = Projectile.NewProjectile(player.Center - new Vector2(4, 4), direction, mod.ProjectileType("SineBall"), projectile.damage, projectile.knockBack * 2.5f, projectile.owner, 180);
                    int p1 = Projectile.NewProjectile(player.Center - new Vector2(4, 4), direction, mod.ProjectileType("SineBall"), projectile.damage, projectile.knockBack * 2.5f, projectile.owner, 0, proj + 1);
                    Main.projectile[proj].hostile = false;
                    Main.projectile[p1].hostile = false;
                    Main.projectile[proj].friendly = true;
                    Main.projectile[p1].friendly = true;
                    Main.projectile[proj].scale *= .65f;
                    Main.projectile[p1].scale *= .65f;
                    Main.projectile[proj].timeLeft = 120;
                    Main.projectile[p1].timeLeft = 120;
                }
                projectile.active = false;
            }
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            return true;
		}
        private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null)
        {
            float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
            Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

            int dust = Dust.NewDust(position - vec * distance, 0, 0, 226);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale *= .3f;
            Main.dust[dust].velocity = vel;
            Main.dust[dust].customData = follow;
        }
    }
}
