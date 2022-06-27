using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.SeaSnailVenom
{
	public class Sea_Snail_Poison_Projectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sea Snail's Poison");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.timeLeft = 180;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) => false;
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 8;
			height = 8;
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => target.AddBuff(70, 60 * 4);
		public override bool? CanCutTiles() => false;

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			for (int index1 = 0; index1 < 2; ++index1)
			{
				int index2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + player.height - 34), player.width, 6, DustID.Venom, 0.0f, 0.0f, 220, new Color(), 0.4f);
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.2f;
			}		
		}
	}
}
