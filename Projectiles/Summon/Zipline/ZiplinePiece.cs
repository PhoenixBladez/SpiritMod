using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Zipline
{
	public class ZiplinePiece : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zipline");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 8;
			projectile.melee = true;
			projectile.height = 8;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.timeLeft = 4;
			projectile.tileCollide = false;
			projectile.extraUpdates = 7;
		}
		public override void AI()
		{
			if(projectile.ai[1] < 0) {
				projectile.ai[0] = 0 - projectile.ai[0];
				projectile.ai[1] = 0 - projectile.ai[1];
			}
			Player player = Main.player[Main.myPlayer];
			if(projectile.Hitbox.Intersects(player.Hitbox) && player.controlUp) {
				if(player.GetSpiritPlayer().ziplineCounter <= 45) {
					player.position = projectile.position - new Vector2(0, player.height);
				}
				player.GetSpiritPlayer().zipline = true;
				player.GetSpiritPlayer().ziplineX = projectile.ai[0];
				player.GetSpiritPlayer().ziplineY = projectile.ai[1];
				//player.velocity.X = projectile.ai[0];
				//player.velocity.Y = projectile.ai[1];
				if(Main.rand.Next(4) == 1)
					player.position.Y--;
				if(Main.rand.Next(10) == 0) {
					Main.PlaySound(2, player.position, 55);
				}
			}
		}
	}
}