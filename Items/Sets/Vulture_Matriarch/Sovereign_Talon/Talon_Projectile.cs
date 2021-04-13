using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SpiritMod.Items.Sets.Vulture_Matriarch.Sovereign_Talon
{
    public class Talon_Projectile : ModProjectile
    {
		protected int dustTimer;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Annihilation Talon");
        } 
        public override void SetDefaults()
        {
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = 1;
			projectile.penetrate = 2;
			projectile.timeLeft = 360;
			projectile.scale = 1f;
			projectile.tileCollide = true;
			projectile.friendly = true;
			projectile.melee = true;
        }
		public override void AI()
        {		
			Player player = Main.player[projectile.owner];
			player.channel = false;
			createDusts();
			makeDusts();
        }
		private void makeDusts()
		{		
			int index2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 87, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f, 100, new Color(), 1f);
			Main.dust[index2].noGravity = true;
			Main.dust[index2].velocity.X *= 1f;
			Main.dust[index2].velocity.Y *= 1f;
		}
		private void createDusts()
		{
			if (Main.player[projectile.owner].channel)
			{
				dustTimer--;
			}
			else
			{
				dustTimer = 0;
			}
			if (dustTimer <= 0 && Main.player[projectile.owner].channel)
			{
				dustTimer = 24;
				for (int index1 = 0; index1 < 5; ++index1)
				{
				  int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 87, 0.0f, 0.0f, 200, new Color(), 2f);
				  Main.dust[index2].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.14159274101257) * (float) Main.rand.NextDouble() * (float) projectile.width / 2f;
				  Main.dust[index2].noGravity = true;
				  Main.dust[index2].velocity *= 3f;
				  int index3 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 87, 0.0f, 0.0f, 100, new Color(), 1f);
				  Main.dust[index3].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.14159274101257) * (float) Main.rand.NextDouble() * (float) projectile.width / 2f;
				  Main.dust[index3].velocity *= 2f;
				  Main.dust[index3].noGravity = true;
				  Main.dust[index3].fadeIn = 1f;
				  Main.dust[index3].color = Color.Crimson * 0.5f;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects effects1 = SpriteEffects.None;
				Texture2D texture = Main.projectileTexture[projectile.type];
				int height = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
				int y2 = height * projectile.frame;
				Vector2 position = (projectile.position - (0.5f * projectile.velocity) + new Vector2((float) projectile.width, (float) projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition).Floor();
				float num1 = 1f;
				if (projectile.direction == 1)
				{
					Main.spriteBatch.Draw(texture, position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), Color.White, projectile.rotation, new Vector2((float) texture.Width / 2f, (float) height / 2f), projectile.scale, effects1, 0.0f);
					//Main.spriteBatch.Draw(mod.GetTexture("Items/Sets/Dim_Mayhem_Set/Annihilation_Claws/Projectiles/Annihilation_Claws_Projectile_Glow"), position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), Color.White, projectile.rotation, new Vector2((float) texture.Width / 2f, (float) height / 2f), projectile.scale, effects1, 0.0f);
				}
				else
				{
					Main.spriteBatch.Draw(texture, position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), Color.White, projectile.rotation, new Vector2((float) texture.Width / 2f, (float) height / 2f), projectile.scale, SpriteEffects.FlipHorizontally, 0.0f);
					//Main.spriteBatch.Draw(mod.GetTexture("Items/Sets/Dim_Mayhem_Set/Annihilation_Claws/Projectiles/Annihilation_Claws_Projectile_Glow"), position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), Color.White, projectile.rotation, new Vector2((float) texture.Width / 2f, (float) height / 2f), projectile.scale, SpriteEffects.FlipHorizontally, 0.0f);
				}
			return false;
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 73, 1f, 0f);
			for (int index1 = 0; index1 < 10; ++index1)
			{
			  int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 87, 0.0f, 0.0f, 200, new Color(), 2f);
			  Main.dust[index2].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.14159274101257) * (float) Main.rand.NextDouble() * (float) projectile.width / 2f;
			  Main.dust[index2].noGravity = true;
			  Main.dust[index2].velocity *= 3f;
			  int index3 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 87, 0.0f, 0.0f, 100, new Color(), 1f);
			  Main.dust[index3].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.14159274101257) * (float) Main.rand.NextDouble() * (float) projectile.width / 2f;
			  Main.dust[index3].velocity *= 2f;
			  Main.dust[index3].noGravity = true;
			  Main.dust[index3].fadeIn = 1f;
			  Main.dust[index3].color = Color.Crimson * 0.5f;
			}
		}
    }
}