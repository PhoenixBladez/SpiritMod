using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class Comet1 : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Comet");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 72;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 6000;
			Projectile.height = 72;
            Projectile.tileCollide = true;
		}
        public void AdditiveCall(SpriteBatch spriteBatch)
        {
			for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Color color = new Color(255, 224, 69) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                float scale = Projectile.scale * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length); 
                Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/Projectiles/Hostile/Comet", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

                spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, color, Projectile.rotation, tex.Size() / 2, scale, default, default);
                spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, color, Projectile.rotation, tex.Size() / 2, scale * .85f, default, default);
               
            }
        }
        public override void AI()
        {
			if (Projectile.soundDelay <= 0) {
				Projectile.soundDelay = 10;
				Projectile.soundDelay *= 9;
				if (Main.rand.NextBool(3))
					SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 9);
			}
			Lighting.AddLight(Projectile.position, 0.255f, .199f, .069f);
            Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
            if (Projectile.timeLeft > 5960)
            {
                Projectile.velocity.Y -= .1f;
            }
            else
            {
                Projectile.velocity.Y += 0.1F;                
            }
        }
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCHit3, Projectile.Center);
            if (Main.rand.NextBool(4))
            {
                Item.NewItem(Projectile.GetSource_Death(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ItemID.FallenStar, 1, false, 0, false, false);
            }            
            for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0f, -2f, 0, default, 1.13f);
				Main.dust[num].noGravity = true;
				Dust dust = Main.dust[num];
				dust.position.X = dust.position.X + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
				Dust expr_92_cp_0 = Main.dust[num];
				expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
				if (Main.dust[num].position != Projectile.Center) {
		    		Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
			    }
            }
            for (int num625 = 0; num625 < 2; num625++) {
				float scaleFactor10 = 0.33f;
				if (num625 == 1) {
					scaleFactor10 = 0.66f;
				}
				if (num625 == 2) {
					scaleFactor10 = 1f;
				}

				for (int i = 0; i < 2; ++i)
				{
					int num626 = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
					Main.gore[num626].velocity *= scaleFactor10;
					Gore expr_13AB6_cp_0 = Main.gore[num626];
					expr_13AB6_cp_0.velocity.X = expr_13AB6_cp_0.velocity.X + 1f;
					Gore expr_13AD6_cp_0 = Main.gore[num626];
					expr_13AD6_cp_0.velocity.Y = expr_13AD6_cp_0.velocity.Y + 1f;
				}
            }
        }
	}
}