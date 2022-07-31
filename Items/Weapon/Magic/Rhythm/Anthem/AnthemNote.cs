using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.Rhythm.Anthem
{
	public class AnthemNote : ModProjectile, IDrawAdditive
	{
		RhythmMinigame minigame;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anthem Note");
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 10;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.timeLeft = 600;
		}

		public override bool PreDraw(ref Color lightColor) => false;

		public override void AI() => Projectile.rotation = Projectile.velocity.ToRotation();

		public void AdditiveCall(SpriteBatch sB, Vector2 screenPos) 
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			sB.Draw(tex, drawPos, null, Color.White * (0.8f + 0.2f * minigame.BeatScale), Projectile.rotation, new Vector2(tex.Width, tex.Height) / 2, Projectile.scale + 0.2f * minigame.BeatScale, Projectile.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
		}

		public void SetUpBeat(RhythmMinigame minigame)
		{
			this.minigame = minigame;
		}
	}
}
