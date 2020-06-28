using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon
{
	public class GlowTongue : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glow Toad");
		}

		public override void SetDefaults()
		{
			projectile.width = 5;
			projectile.height = 5;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
		}
		//projectile.ai[0] - initial direction: 0 = left, 1 = right
		Vector2 originPos = Vector2.Zero;
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			if(originPos == Vector2.Zero) {
				originPos = projectile.position;
			}
			projectile.velocity.Y = 0;
			if(projectile.ai[0] == 0) {
				projectile.velocity.X += 0.25f;
			} else {
				projectile.velocity.X -= 0.25f;
			}
			if(Math.Abs(projectile.velocity.X) > 11) {
				projectile.active = false;
			}

		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if(Main.rand.Next(5) == 0)
				target.AddBuff(ModContent.BuffType<StarFlame>(), 200);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			ProjectileExtras.DrawChain(projectile.whoAmI, originPos,
				"SpiritMod/NPCs/BlueMoon/GlowTongue_Chain");
			ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
			return false;
		}

	}
}
