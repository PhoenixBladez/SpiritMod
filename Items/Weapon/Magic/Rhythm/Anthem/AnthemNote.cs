using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
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
			projectile.width = 14;
			projectile.height = 10;
			projectile.magic = true;
			projectile.friendly = true;
			projectile.timeLeft = 600;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation();
		}

		public override void Kill(int timeLeft)
		{
			minigame.BeatEvent -= OnBeat;
		}

		public void AdditiveCall(SpriteBatch sB) 
		{
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			Texture2D tex = Main.projectileTexture[projectile.type];
			sB.Draw(tex, drawPos, null, Color.White * (0.8f + 0.2f * minigame.BeatScale), projectile.rotation, new Vector2(tex.Width, tex.Height) / 2, projectile.scale + 0.2f * minigame.BeatScale, projectile.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
		}

		public void SetUpBeat(RhythmMinigame minigame)
		{
			this.minigame = minigame;
			minigame.BeatEvent += OnBeat;
		}

		private void OnBeat(bool successful, int combo)
		{
			
		}
	}
}
