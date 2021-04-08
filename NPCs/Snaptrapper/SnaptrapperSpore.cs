using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Snaptrapper
{
	public class SnaptrapperSpore : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snaptrapper Spore");
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.height = 30;
			projectile.width = 26;
			projectile.friendly = false;
			projectile.aiStyle = 2;
			projectile.penetrate = 4;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
            Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 7);
            Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
            Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 2);
            Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 95);
            return true;
		}

		public override void AI()
		{
			projectile.rotation += 0.3f;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(6) == 1)
				target.AddBuff(BuffID.Poisoned, 180);
		}
        public override void Kill(int timeLeft)
		{
			for (int num621 = 0; num621 < 8; num621++) {
				Dust.NewDust(projectile.position, projectile.width, projectile.height,
					2, 0f, 0f, 100, default(Color), .7f);
                Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height, ModContent.DustType<Dusts.PoisonGas>(), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, new Color(), 5f)];
                dust.noGravity = true;
                dust.velocity.X = dust.velocity.X * 0.3f;
                dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;
            }
            int amountOfProjectiles = Main.rand.Next(2, 5);
            for (int i = 0; i < amountOfProjectiles; ++i)
            {
                int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-3, 3), Main.rand.Next(-4, -1), ModContent.ProjectileType<SnaptrapperGas>(), 12, 1, Main.myPlayer, 0, 0);
                Main.projectile[p].hostile = true;
                Main.projectile[p].friendly = false;
            }
        }

	}
}