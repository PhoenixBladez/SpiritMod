using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class SkullSentrySentry : ModProjectile
	{
		int[] eyeWhoAmIs = null;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Maneater");

		public override void SetDefaults()
		{
			projectile.width = 46;
			projectile.height = 64;
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.aiStyle = -1;
		}

		public override void AI()
		{
			if (eyeWhoAmIs is null) //Init
			{
				eyeWhoAmIs = new int[3];
				Vector2[] offsets = new Vector2[] { new Vector2(-10, -8), new Vector2(10, -8), new Vector2(0, 14) };

				for (int i = 0; i < 3; ++i)
				{
					Vector2 pos = projectile.Center + offsets[i];
					int p = Projectile.NewProjectile(pos, Vector2.Zero, ModContent.ProjectileType<SkullSentryEye>(), Main.player[projectile.owner].HeldItem.damage, 0f, projectile.owner, -1);

					(Main.projectile[p].modProjectile as SkullSentryEye).anchor = pos;

					eyeWhoAmIs[i] = p;
				}
			}
		}
	}
}
