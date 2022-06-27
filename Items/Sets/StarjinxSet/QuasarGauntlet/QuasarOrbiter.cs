using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Trails;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.QuasarGauntlet
{
	public class QuasarOrbiter : ModProjectile, ITrailProjectile
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Quasar Orb");

		public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
			Projectile.hide = true;
			Projectile.extraUpdates = 1;
		}

		private bool _initialized;

		private ref float radiusModifer => ref Projectile.ai[1];

        private Vector2 target = Vector2.Zero;
		private Vector2 newCenter = Vector2.Zero;
        public override void AI()
        {
            Projectile parent = Main.projectile[(int)Projectile.ai[0]];
            if (parent.active && parent.owner == Projectile.owner && parent.type == ModContent.ProjectileType<QuasarOrb>())
                target = parent.Center;

            else
                Projectile.Kill();

            if (!_initialized)
            {
				Projectile.velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f));
				radiusModifer = Main.rand.NextFloat(0.8f, 1.3f);
				_initialized = true;
				Projectile.netUpdate = true;
            }

			Projectile.alpha = parent.alpha;
			Projectile.position = target + newCenter;
			newCenter += Projectile.velocity;

			float minRadius = 40 * parent.scale * radiusModifer;
			float maxHomeEffectRadius = 120 * parent.scale * radiusModifer;
			if(newCenter.Length() > minRadius)
			{
				float lerpSpeed = MathHelper.Lerp(0.08f, 0.16f, MathHelper.Clamp((newCenter.Length() - minRadius) / (maxHomeEffectRadius - minRadius), 0, 1)) * radiusModifer;
				Projectile.velocity = Projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target) * Projectile.velocity.Length(), lerpSpeed)) * 0.95f +
					Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target) * Projectile.velocity.Length(), lerpSpeed) * 0.05f + 
					parent.velocity * 0.01f;
			}
        }

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(_initialized);
			writer.WriteVector2(target);
			writer.WriteVector2(newCenter);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			_initialized = reader.ReadBoolean();
			target = reader.ReadVector2();
			newCenter = reader.ReadVector2();
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, Color.Magenta), new RoundCap(), new DefaultTrailPosition(), 40f, 100f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_4").Value, 0.01f, 1f, 1f));

			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, new Color(255, 5, 30)), new RoundCap(), new DefaultTrailPosition(), 30f, 100f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_4").Value, 0.01f, 1f, 1f));
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, new Color(255, 247, 0)), new RoundCap(), new DefaultTrailPosition(), 20f, 100f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_4").Value, 0.01f, 1f, 1f));
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, new Color(255, 251, 199)), new RoundCap(), new DefaultTrailPosition(), 6f, 100f);
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, new Color(255, 251, 199)), new RoundCap(), new DefaultTrailPosition(), 6f, 100f);
		}
	}
}