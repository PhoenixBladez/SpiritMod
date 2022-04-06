using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Equipment
{
	internal class MagnetHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magnet Hook");
			Tooltip.SetDefault("Homes in on nearby blocks");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.AmethystHook);
			item.shoot = ModContent.ProjectileType<MagnetHookProjectile>();
		}
	}

	internal class MagnetHookProjectile : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("${ProjectileName.GemHookAmethyst}");

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
			projectile.timeLeft = 1200;
		}

		public override bool? CanUseGrapple(Player player) => player.ownedProjectileCounts[projectile.type] == 0;

		int extendlength = 450;
		public override float GrappleRange() => extendlength;

		public override void NumGrappleHooks(Player player, ref int numHooks) => numHooks = 1;

		bool retracting = false;
		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			speed = 13f;
			retracting = true;
		}

		public override void GrapplePullSpeed(Player player, ref float speed) => speed = 12;

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor) => ProjectileExtras.DrawChain(projectile.whoAmI, Main.player[projectile.owner].MountedCenter, "SpiritMod/Items/Equipment/MagnetHookChain");

		Point targetPos = new Point();
		bool homing = false;
		float lowestDist = float.MaxValue;

		public override void AI()
		{
			for (int k = 0; k < 2; k++)
			{
				int index2 = Dust.NewDust(new Vector2(projectile.Center.X + 15, projectile.Center.Y), 1, 1, k == 0 ? DustID.DungeonSpirit : DustID.Firework_Red, 0.0f, 0.0f, 0, default, 1f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / 5 * k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}

			if (projectile.timeLeft < 1180)
			{
				Point tPos = projectile.position.ToTileCoordinates();

				if (!homing)
				{
					lowestDist = float.MaxValue;
					for (int i = tPos.X - 5; i < tPos.X + 5; i++)
					{
						for (int j = tPos.Y - 5; j < tPos.Y + 5; j++)
						{
							Tile tile = Framing.GetTileSafely(i, j);
							if (tile.nactive() && Main.tileSolid[tile.type])
							{
								float dist = projectile.Distance(new Vector2(i * 16, j * 16));
								if (dist < lowestDist + 32)
								{
									lowestDist = dist;
									targetPos = new Point(i, j);
								}
							}
						}
					}
				}

				if (lowestDist < 113.137085 && !retracting)
				{
					projectile.velocity = projectile.DirectionTo(new Vector2(targetPos.X, targetPos.Y) * 16) * projectile.velocity.Length();
					homing = true;
					extendlength++;
				}
			}
		}
	}
}