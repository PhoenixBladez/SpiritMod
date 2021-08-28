using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.TideDrops.Whirltide
{
	public class Whirltide_Water_Explosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Water Explosion");
		}
		public override void SetDefaults()
		{
			projectile.width = 60;
			projectile.height = 60;
			projectile.aiStyle = 1;
			aiType = ProjectileID.WoodenArrowFriendly;
			projectile.hide = true;
			projectile.penetrate = -1;
			projectile.magic = true;
			projectile.friendly = true;
			projectile.scale = 1f;
			projectile.extraUpdates = 1;
			projectile.timeLeft = 60;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void AI()
		{
			for (int index = 0; index < 12; ++index)
			{
				if (Main.rand.Next(5) != 0)
				{
					int Type = Utils.SelectRandom<int>(Main.rand, new int[2]
					{
				  4,
				  157
					});
					Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, Type, projectile.velocity.X, projectile.velocity.Y, 100, new Color(), 1f)];
					dust.velocity = dust.velocity / 4f + projectile.velocity / 2f;
					dust.scale = (float)(0.800000011920929 + (double)Main.rand.NextFloat() * 0.400000005960464);
					dust.position = projectile.Center;
					dust.position += new Vector2((float)(projectile.width / 3), 0.0f).RotatedBy(6.28318548202515 * (double)Main.rand.NextFloat(), new Vector2()) * Main.rand.NextFloat();
					dust.noLight = true;
					dust.noGravity = true;
					if (dust.type == 4)
					{
						dust.color = new Color(80, 170, 40, 120);
					}
				}
			}
			float num2 = 60f;

			++projectile.ai[1];

			float num3 = projectile.ai[1] / num2;
			Vector2 spinningpoint = new Vector2(0.0f, -30f);
			spinningpoint = spinningpoint.RotatedBy((double)num3 * 1.5 * 6.28318548202515, new Vector2()) * new Vector2(1f, 0.4f);
			for (int index1 = 0; index1 < 4; ++index1)
			{
				Vector2 vector2 = Vector2.Zero;
				float num4 = 1f;
				if (index1 == 0)
				{
					vector2 = Vector2.UnitY * -15f;
					num4 = 0.6f;
				}
				if (index1 == 1)
				{
					vector2 = Vector2.UnitY * -5f;
					num4 = 0f;
				}
				if (index1 == 2)
				{
					vector2 = Vector2.UnitY * 5f;
					num4 = 0f;
				}
				if (index1 == 3)
				{
					vector2 = Vector2.UnitY * 20f;
					num4 = 0.6f;
				}
				int index2 = Dust.NewDust(projectile.Center, 0, 0, DustID.ChlorophyteWeapon, 0.0f, 0.0f, 100, new Color(), 1f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].position = projectile.Center + spinningpoint * num4 + vector2;
				Main.dust[index2].velocity = Vector2.Zero;
				spinningpoint *= -1f;
				int index3 = Dust.NewDust(projectile.Center, 0, 0, DustID.ChlorophyteWeapon, 0.0f, 0.0f, 100, new Color(), 1f);
				Main.dust[index3].noGravity = true;
				Main.dust[index3].position = projectile.Center + spinningpoint * num4 + vector2;
				Main.dust[index3].velocity = Vector2.Zero;
			}
		}
	}
}