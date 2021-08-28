using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boulder_Termagant
{
	public class Granite_Boulder : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Granite Boulder");
		public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 40;
			projectile.aiStyle = 1;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			projectile.penetrate = 100;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			int height = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
			int y2 = height * projectile.frame;
			Vector2 position = (projectile.position - (0.5f * projectile.velocity) + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition).Floor();
			var effects1 = SpriteEffects.None;
			Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boulder_Termagant/Granite_Boulder_Glow"), position, new Rectangle(0, y2, texture.Width, height), lightColor, projectile.rotation, new Vector2(texture.Width / 2f, height / 2f), projectile.scale, effects1, 0.0f);
			return true;
		}
		public override void AI()
		{
			Player player = Main.LocalPlayer;
			for (int index = 0; index < 6; ++index)
			{
				if (Main.rand.Next(10) != 0)
				{
					Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Vortex, projectile.velocity.X, projectile.velocity.Y, 100, new Color(), 1f)];
					dust.velocity = dust.velocity / 4f + projectile.velocity / 2f;
					dust.scale = (float)(0.400000011920929 + (double)Main.rand.NextFloat() * 0.400000005960464);
					dust.position = projectile.Center;
					dust.position += new Vector2((float)(projectile.width / 2), 0.0f).RotatedBy(6.28318548202515 * (double)Main.rand.NextFloat(), new Vector2()) * Main.rand.NextFloat();
					dust.noLight = false;
					dust.noGravity = true;
				}
			}
			if (projectile.position.Y > player.position.Y)
				projectile.tileCollide = true;
		}

		public override void Kill(int timeLeft) => Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y, 1, 1f, 0f);
	}
}