using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class DrakomireFlame : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Drakomire Flame");
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.timeLeft = 60;
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.hide = true;
		}

		public override bool PreAI()
		{
			if (Projectile.velocity.Y < 8f)
				Projectile.velocity.Y += 0.1f;


			Vector2 next = Projectile.position + Projectile.velocity;
			Tile inside = Main.tile[((int)Projectile.position.X + (Projectile.width >> 1)) >> 4, ((int)Projectile.position.Y + (Projectile.height >> 1)) >> 4];
			if (inside.HasTile && Main.tileSolid[inside.TileType]) {
				Projectile.position.Y -= 16f;
				return false;
			}

			if (Collision.WetCollision(next, Projectile.width, Projectile.height)) {
				if (Main.player[Projectile.owner].waterWalk) {
					Projectile.velocity.Y = 0f;
				}
				else {
					Projectile.timeLeft = 0;
				}
			}

			int num = Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height, DustID.Torch, (float)Main.rand.Next(-3, 4), (float)Main.rand.Next(-3, 4), 100, default, 1f);
			Dust dust = Main.dust[num];
			dust.position.X = dust.position.X - 2f;
			dust.position.Y = dust.position.Y + 2f;
			dust.scale += (float)Main.rand.Next(50) * 0.01f;
			dust.noGravity = true;
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) => false;

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => target.AddBuff(24, 60);
	}
}
