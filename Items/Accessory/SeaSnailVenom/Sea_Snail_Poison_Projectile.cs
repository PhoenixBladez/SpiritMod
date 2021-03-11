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
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.timeLeft = 180;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 8;
			height = 8;
			return true;
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(70,60*4);
		}
		public override bool? CanCutTiles() 
		{
			return false;
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			for (int index1 = 0; index1 < 2; ++index1)
			{
				int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + player.height - 34), player.width, 6, 171, 0.0f, 0.0f, 220, new Color(), 0.4f);
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.2f;
			}		
		}
	}
}
