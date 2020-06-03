using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class SandWall2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Wall2");
		}
		int counter = 0;
		float distance = 5f;
		int rotationalSpeed = 2;
		float initialSpeedMult = 1;
		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 15;
			projectile.height = 15;
			//projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 5;
			projectile.alpha = 255;
			projectile.timeLeft = 450;
			projectile.extraUpdates = 2;
			projectile.tileCollide = true; //Tells the game whether or not it can collide with a tile
		}
        public override void AI()
		{
			
			 if (Main.rand.Next(3) == 1)
			 {
				Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.GoldCoin);
				dust.velocity = Vector2.Zero;
				dust.noGravity = true;
			 }
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			distance += 0.025f;
			initialSpeedMult+= 0.01f;
			counter += rotationalSpeed;
			Vector2 initialSpeed = new Vector2(projectile.ai[0], projectile.ai[1]) * initialSpeedMult;
			Vector2 offset = initialSpeed.RotatedBy(Math.PI / 2);
			offset.Normalize();
			offset *= (float)(Math.Cos(counter * (Math.PI / 180)) * (distance / 3));
			projectile.velocity = initialSpeed + offset;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.boss && target.velocity != Vector2.Zero && target.knockBackResist != 0)
            {
                target.velocity.Y = -4f;
            }
        }
    }
}
