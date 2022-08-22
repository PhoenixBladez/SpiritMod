using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
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
			Projectile.hostile = false;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 4;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 7;
		}
		public override void AI()
		{
			if (Projectile.ai[1] < 0) {
				Projectile.ai[0] = 0 - Projectile.ai[0];
				Projectile.ai[1] = 0 - Projectile.ai[1];
			}
			Player player = Main.player[Main.myPlayer];
			if (Projectile.Hitbox.Intersects(player.Hitbox) && player.controlUp && Projectile.ai[1] < 0.7) {
				if (player.GetSpiritPlayer().ziplineCounter <= 45) {
					player.position = Projectile.position - new Vector2(0, player.height);
				}
				player.GetSpiritPlayer().zipline = true;
				player.GetSpiritPlayer().ziplineX = Projectile.ai[0];
				player.GetSpiritPlayer().ziplineY = Projectile.ai[1];
				//player.velocity.X = projectile.ai[0];
				//player.velocity.Y = projectile.ai[1];
				if (Main.rand.NextBool(4))
					player.position.Y--;
				if (Main.rand.NextBool(10)) {
					SoundEngine.PlaySound(SoundID.Item55, player.position);
				}
                player.AddBuff(ModContent.BuffType<Buffs.RailBuff>(), 20);
			}
		}
	}
}