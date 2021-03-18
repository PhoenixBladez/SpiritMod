using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Cascade.Reef_Wrath
{
	public class Reef_Wrath_Projectile_Alt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coral Reef");
		}

		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = 1;
			aiType = ProjectileID.WoodenArrowFriendly;
			projectile.hide = true;
			projectile.scale = 1f;
			projectile.timeLeft = 2;
		}

		public override bool PreAI()
		{
			projectile.position -= projectile.velocity;
			return base.PreAI();
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			for (int i = 1; i <= 3; i++) {
				Vector2 position = projectile.position;
				position += new Vector2(0, - (18 * (i - 1))).RotatedBy(projectile.velocity.ToRotation());
				Projectile.NewProjectile(position, projectile.velocity, mod.ProjectileType("Reef_Wrath_Projectile_" + i), player.HeldItem.damage, 8f, 0);
			}
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(42, 3));
		}
	}
}
