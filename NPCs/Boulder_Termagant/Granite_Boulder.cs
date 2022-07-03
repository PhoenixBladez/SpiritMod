using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boulder_Termagant
{
	public class Granite_Boulder : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Granite Boulder");
		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.aiStyle = 1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Projectile.penetrate = 100;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			int height = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
			int y2 = height * Projectile.frame;
			Vector2 position = (Projectile.position - (0.5f * Projectile.velocity) + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition).Floor();
			var effects1 = SpriteEffects.None;
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boulder_Termagant/Granite_Boulder_Glow").Value, position, new Rectangle(0, y2, texture.Width, height), lightColor, Projectile.rotation, new Vector2(texture.Width / 2f, height / 2f), Projectile.scale, effects1, 0.0f);
			return true;
		}
		public override void AI()
		{
			Player player = Main.LocalPlayer;
			for (int index = 0; index < 6; ++index)
			{
				if (Main.rand.Next(10) != 0)
				{
					Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex, Projectile.velocity.X, Projectile.velocity.Y, 100, new Color(), 1f)];
					dust.velocity = dust.velocity / 4f + Projectile.velocity / 2f;
					dust.scale = (float)(0.400000011920929 + (double)Main.rand.NextFloat() * 0.400000005960464);
					dust.position = Projectile.Center;
					dust.position += new Vector2((float)(Projectile.width / 2), 0.0f).RotatedBy(6.28318548202515 * (double)Main.rand.NextFloat(), new Vector2()) * Main.rand.NextFloat();
					dust.noLight = false;
					dust.noGravity = true;
				}
			}
			if (Projectile.position.Y > player.position.Y)
				Projectile.tileCollide = true;
		}

		public override void Kill(int timeLeft) => SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
	}
}