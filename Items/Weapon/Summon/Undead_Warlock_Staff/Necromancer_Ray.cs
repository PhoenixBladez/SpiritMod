using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon.Undead_Warlock_Staff
{
	public class Necromancer_Ray : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Necromancer's Ray");
		}
		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = 0;
			projectile.timeLeft = 40;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.alpha = 230;
			//projectile.hide = true;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, (int) 0, (int) 0, 230)*0.4f;
		}
			
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (player.HeldItem.type != mod.ItemType("Undead_Warlock_Staff"))
				projectile.Kill();
			Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.255f*2, 0f, 0f);
			float num1 = 20f;
			++projectile.localAI[0];
			projectile.alpha = (int) MathHelper.Lerp(0.0f, (float) byte.MaxValue, projectile.localAI[0] / num1);
			int index = (int) projectile.ai[0];
			int num3 = -1;
			num3 = 0;
			if (num3 == 0)
			{
			  if ((double) projectile.localAI[0] >= (double) num1 || index < 0 || (index > 1000 || !player.active))
			  {
				projectile.Kill();
				return;
			  }
			  if (player.direction == 1)
				projectile.Center = new Vector2(player.Center.X + 18, player.Center.Y - 40) - projectile.velocity;
			  else
				projectile.Center = new Vector2(player.Center.X - 24, player.Center.Y - 40) - projectile.velocity; 
			}
			projectile.rotation = projectile.velocity.ToRotation() + 1.570796f;
			if (!player.mount.Active)
				return;
			projectile.Kill();
		}
	}
}