using Microsoft.Xna.Framework;
using SpiritMod.Buffs.DoT;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class CryoExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Bomb");
			Main.projFrames[projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			projectile.width = 80;
			projectile.height = 80;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.hide = true;
			projectile.penetrate = 3;
			projectile.timeLeft = 150;
			projectile.alpha = 0;
			projectile.tileCollide = false;
		}

		float counter = 3f;

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 27);
			for (int i = 0; i < 40; i++)
			{
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BlueCrystalShard, 0f, -2f, 0, default, 2f);
				Main.dust[num].noGravity = true;
				Dust dust = Main.dust[num];
				dust.position.X = dust.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
				Dust expr_92_cp_0 = Main.dust[num];
				expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
				if (Main.dust[num].position != projectile.Center)
				{
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}

			for (int i = 0; i < 3; i++)
				Gore.NewGore(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/CryoBomb/CryoShard1"), 1f);
			for (int i = 0; i < 3; i++)
				Gore.NewGore(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/CryoBomb/CryoShard2"), 1f);
			for (int i = 0; i < 3; i++)
				Gore.NewGore(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/CryoBomb/CryoShard3"), 1f);
		}

		public override void DrawBehind(int index, List<int> behindTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overWires) => behindTiles.Add(index);
		public override Color? GetAlpha(Color lightColor) => new Color(220, 220, 220, 100);

		public override bool PreAI()
		{
			if (counter > 0)
				counter -= 0.5f;

			projectile.frame = (int)counter;
			projectile.scale -= .0025f;
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(ModContent.BuffType<CryoCrush>(), 240);
		}
	}
}