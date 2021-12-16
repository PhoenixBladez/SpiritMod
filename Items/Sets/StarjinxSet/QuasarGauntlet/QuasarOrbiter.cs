using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Utilities;
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
			projectile.width = 2;
			projectile.height = 2;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.magic = true;
            projectile.ignoreWater = true;
			projectile.hide = true;
			projectile.extraUpdates = 1;
		}

		private bool _initialized;

		private ref float radiusModifer => ref projectile.ai[1];

        private Vector2 target = Vector2.Zero;
		private Vector2 newCenter = Vector2.Zero;
        public override void AI()
        {
            Projectile parent = Main.projectile[(int)projectile.ai[0]];
            if (parent.active && parent.owner == projectile.owner && parent.type == ModContent.ProjectileType<QuasarOrb>())
                target = parent.Center;

            else
                projectile.Kill();

            if (!_initialized)
            {
				projectile.velocity = projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f));
				radiusModifer = Main.rand.NextFloat(0.8f, 1.3f);
				_initialized = true;
				projectile.netUpdate = true;
            }

			projectile.alpha = parent.alpha;
			projectile.position = target + newCenter;
			newCenter += projectile.velocity;

			float minRadius = 40 * parent.scale * radiusModifer;
			float maxHomeEffectRadius = 120 * parent.scale * radiusModifer;
			if(newCenter.Length() > minRadius)
			{
				float lerpSpeed = MathHelper.Lerp(0.08f, 0.16f, MathHelper.Clamp((newCenter.Length() - minRadius) / (maxHomeEffectRadius - minRadius), 0, 1)) * radiusModifer;
				projectile.velocity = projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(projectile.velocity, projectile.DirectionTo(target) * projectile.velocity.Length(), lerpSpeed)) * 0.95f +
					Vector2.Lerp(projectile.velocity, projectile.DirectionTo(target) * projectile.velocity.Length(), lerpSpeed) * 0.05f + 
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
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, Color.Magenta), new RoundCap(), new DefaultTrailPosition(), 40f, 100f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));

			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(255, 5, 30)), new RoundCap(), new DefaultTrailPosition(), 30f, 100f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(255, 247, 0)), new RoundCap(), new DefaultTrailPosition(), 20f, 100f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(255, 251, 199)), new RoundCap(), new DefaultTrailPosition(), 6f, 100f);
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(255, 251, 199)), new RoundCap(), new DefaultTrailPosition(), 6f, 100f);
		}
	}
}