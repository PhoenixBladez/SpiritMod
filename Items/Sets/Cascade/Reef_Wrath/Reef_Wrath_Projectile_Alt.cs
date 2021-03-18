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
			projectile.timeLeft = 60;
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		
		public override void AI()
		{
			projectile.velocity.Y = 100f;
			projectile.velocity.X = 0f;
		}
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y - 8, 0f, 0f, mod.ProjectileType("Reef_Wrath_Projectile_1"), player.HeldItem.damage, 8f, 0);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y - 8-18, 0f, 0f, mod.ProjectileType("Reef_Wrath_Projectile_2"), player.HeldItem.damage, 8f, 0);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y - 8-36, 0f, 0f, mod.ProjectileType("Reef_Wrath_Projectile_3"), player.HeldItem.damage, 8f, 0);	
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(42, 3));
		}
	}
}
