using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Utilities;

namespace SpiritMod.Mechanics.BoonSystem.HecateBoon
{
	public class HecateSpawnParticle : Particle
	{
		private readonly uint _maxTime;
		private readonly Projectile _parent;
		public HecateSpawnParticle(Projectile parent, Color color, float scale, uint displayTime)
		{
			_parent = parent;
			Color = color;
			Scale = scale;
			_maxTime = displayTime;
		}

		public override bool UseAdditiveBlend => true;
		public override bool UseCustomDraw => true;

		public override void Update()
		{
			if (!_parent.active || _parent == null || _parent.type != ModContent.ProjectileType<HecateBoonProj>() || TimeActive > _maxTime) 
				Kill();

			Position = _parent.Center;
		}

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			float progress = TimeActive / (float)_maxTime;
			progress = EaseFunction.EaseQuadOut.Ease(progress);

			Texture2D texture = ParticleHandler.GetTexture(Type);

			float reverseProgress = 1 - progress;
			Color glowColor = Color.Lerp(Color.White, Color, MathHelper.Lerp(progress, 1, 0.25f));

			spriteBatch.Draw(texture, Position - Main.screenPosition, null, glowColor * reverseProgress, 0, texture.Size() / 2, reverseProgress * Scale, SpriteEffects.None, 0);


			float blurLength = 150 * reverseProgress;
			float blurWidth = 25 * reverseProgress;

			Effect blurEffect = ModContent.Request<Effect>("Effects/BlurLine", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Prim.SquarePrimitive blurLine = new Prim.SquarePrimitive()
			{
				Position = Position - Main.screenPosition,
				Height = blurWidth,
				Length = blurLength,
				Color = glowColor * reverseProgress
			};

			Prim.PrimitiveRenderer.DrawPrimitiveShape(blurLine, blurEffect);
		}
	}
}
