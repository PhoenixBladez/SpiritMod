using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.LaunchersMisc.Liberty
{
	public class LibertyChargeFire : Particle
	{
		private Color _startColor;
		private Color _endColor;
		public int MaxTime;

		private Vector2 _startingOffset;
		private Projectile _parent;

		private const float FADETIME = 0.3f;

		public override bool UseAdditiveBlend => true;

		public override bool UseCustomDraw => true;

		public LibertyChargeFire(Projectile projectile, Vector2 offset, Color startColor, Color endColor, float scale, int maxTime)
		{
			_parent = projectile;
			_startingOffset = offset;
			_startColor = startColor;
			_endColor = endColor;
			Scale = scale;
			MaxTime = maxTime;
		}

		public override void Update()
		{
			//checks to make sure the particle is supposed to still exist
			if (_parent == null)
			{
				Kill();
				return;
			}
			if (!_parent.active || _parent.type != ModContent.ProjectileType<LibertyProjHeld>())
			{
				Kill();
				return;
			}

			float fadeintime = MaxTime * FADETIME;
			Color = Color.Lerp(_startColor, _endColor, TimeActive / (float)MaxTime);
			if (TimeActive < fadeintime)
				Color *= (TimeActive / fadeintime);
			else if(TimeActive > (MaxTime - fadeintime))
				Color *= ((MaxTime - TimeActive) / fadeintime);

			Lighting.AddLight(Position, Color.ToVector3() * Scale * 0.5f);

			Position = _parent.Center + (_parent.velocity * 40) + Vector2.Lerp(_startingOffset.RotatedBy(_parent.velocity.ToRotation()), Vector2.Zero, TimeActive / (float)MaxTime);

			if (TimeActive > MaxTime)
				Kill();
		}

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D tex = ParticleHandler.GetTexture(Type);
			Texture2D bloom = SpiritMod.instance.GetTexture("Effects/Masks/CircleGradient");
			spriteBatch.Draw(bloom, Position - Main.screenPosition, null, Color * 0.6f, 0, bloom.Size() / 2, Scale / 5f, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, Position - Main.screenPosition, null, Color, 0, tex.Size() / 2, Scale, SpriteEffects.None, 0);
		}
	}
}
