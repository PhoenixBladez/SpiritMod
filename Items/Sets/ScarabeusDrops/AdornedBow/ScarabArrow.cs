using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Mechanics.Trails;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ScarabeusDrops.AdornedBow
{
	public class ScarabArrow : ModProjectile, ITrailProjectile
	{
		static readonly int traillength = 10;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarab Arrow");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = traillength;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			Projectile.aiStyle = -1;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 720;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			if (Projectile.ai[0] != 1)
				return;

			tM.CreateTrail(Projectile, new StandardColorTrail(new Color(163, 255, 246)), new RoundCap(), new DefaultTrailPosition(), 10, 200, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/CrystalTrail").Value, Vector2.One));
			tM.CreateTrail(Projectile, new StandardColorTrail(new Color(163, 255, 246)), new RoundCap(), new WaveTrailPos(10), 10, 200, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/CrystalTrail").Value, Vector2.One));
			tM.CreateTrail(Projectile, new StandardColorTrail(new Color(163, 255, 246)), new RoundCap(), new WaveTrailPos(-10), 10, 200, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/CrystalTrail").Value, Vector2.One));
			tM.CreateTrail(Projectile, new GradientTrail(new Color(163, 255, 246) * 0.5f, Color.Transparent), new RoundCap(), new ArrowGlowPosition(), 25, 300, new DefaultShader());
		}

		bool Enchanted => Projectile.ai[0] == 1;
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.Pi/2;

			if (Enchanted)
			{
				Lighting.AddLight(Projectile.position, Color.White.ToVector3() / 2);
				Projectile.extraUpdates = 1;
			}
			else if (++Projectile.ai[1] > 20)
				Projectile.velocity.Y += 0.25f;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PlatinumCoin);
			}
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(Enchanted)
				target.AddBuff(ModContent.BuffType<TopazMarked>(), 240);
		}

		public override bool PreDraw(ref Color lightColor) //refactor to be drawadditive when ported to main??
		{
			Texture2D projtexture = TextureAssets.Projectile[Projectile.type].Value;

			void drawtexture(Vector2 position, Color color, float Opacity, Texture2D tex) => spriteBatch.Draw(tex, position - Main.screenPosition, null, color * Opacity, Projectile.rotation, tex.Size() / 2, Opacity * Projectile.scale, SpriteEffects.None, 0);

			for (int i = 0; i < traillength; i += 1)
			{
				Vector2 drawpos = Projectile.oldPos[i] + Projectile.Size / 2;
				float Opacity = (traillength - i) / (float)traillength;
				Opacity *= 0.8f;

				drawtexture(drawpos, lightColor, Opacity, projtexture);
			}
			drawtexture(Projectile.Center, lightColor, 1, projtexture);
			return false;
		}
	}
}
