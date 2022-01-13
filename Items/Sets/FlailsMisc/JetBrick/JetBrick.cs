using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Particles;
using SpiritMod.Prim;
using SpiritMod.Utilities;
using SpiritMod.Mechanics.Trails.CustomTrails;

namespace SpiritMod.Items.Sets.FlailsMisc.JetBrick
{
	public class JetBrick : BaseFlailItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jet Brick");
			Tooltip.SetDefault("Pulls you along with it when launched far enough");
		}

		public override void SafeSetDefaults()
		{
			item.Size = new Vector2(34, 30);
			item.damage = 50;
			item.rare = ItemRarityID.Pink;
			item.value = Item.buyPrice(0, 3, 0, 0);
			item.useTime = 30;
			item.useAnimation = 30;
			item.shoot = ModContent.ProjectileType<JetBrickProj>();
			item.shootSpeed = 16;
			item.knockBack = 4;
		}
	}

	public class JetBrickProj : BaseFlailProj, ITrailProjectile, IDrawAdditive
	{
		public JetBrickProj() : base(new Vector2(0.7f, 1.6f), new Vector2(0.5f, 2f), 2, 70, 13) { }

		public override void SetStaticDefaults() => DisplayName.SetDefault("Jet Brick");

		bool Blur = false;
		public override void SpinExtras(Player player)
		{
			projectile.localAI[0]++;
			AddColor();
			//CreateSmoke(3, true, player);
		}
		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateCustomTrail(new FlameTrail(projectile, Color.Orange, Color.Red, Color.DarkRed, 20, 10));
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, Color.Orange * 0.33f, Color.Transparent), new RoundCap(), new DefaultTrailPosition(), 60 * projectile.scale, 300, null, TrailLayer.AboveProjectile);
		}
		public override void NotSpinningExtras(Player player)
		{
			Vector2 position = projectile.Center + Vector2.Normalize(projectile.velocity) * 10;

			for (int i = 0; i < 4; i++)
			{
				Dust newDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, 0f, 0f, 0, default, 1f)];
				if (Main.rand.NextBool(2))
					newDust.position = position;
				if (Main.rand.NextBool(2))
				{
					newDust.velocity = projectile.velocity.RotatedBy(Math.PI / 2, default) * 0.33F + projectile.velocity / 4;
					newDust.position += projectile.velocity.RotatedBy(Math.PI / 2, default);
				}
				else
                {
					newDust.velocity = projectile.velocity.RotatedBy(-Math.PI / 2, default) * 0.33F + projectile.velocity / 4;
					newDust.position += projectile.velocity.RotatedBy(-Math.PI / 2, default);
				}
				newDust.fadeIn = 0.5f;
				newDust.noGravity = true;

			}
			projectile.localAI[1]++;
			AddColor();
			//CreateSmoke(5, false, player);
		}

		public override void LaunchExtras(Player player)
		{
			if (projectile.Distance(player.Center) > 190 && projectile.localAI[0] > 100)
				player.velocity = projectile.velocity * 0.7f;

			base.LaunchExtras(player);
		}

		private void AddColor()
		{
			Color color = Color.Lerp(Color.Red, Color.Black, Math.Max(projectile.localAI[0] / 120f, 1));
			Lighting.AddLight(projectile.Center, color.ToVector3() * .7f);
		}

		/*private void CreateSmoke(int rate, bool spinning, Player player)
		{
			if (projectile.localAI[1] % rate == 0)
			{
				Vector2 direction = spinning ? projectile.DirectionTo(player.Center).RotatedBy(player.direction * -1.57f) * 3 : Main.rand.NextVector2Circular(3, 3);
				var particle = new SmokeParticle(
					projectile.Center,
					direction,
					Color.Lerp(Color.Black, Color.Gray, Main.rand.NextFloat()),
					Main.rand.NextFloat(0.3f, 0.5f),
					60);
				ParticleHandler.SpawnParticle(particle);
			}
		}*/

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => projectile.velocity = projectile.DirectionFrom(target.Center) * projectile.velocity.Length() * 0.8f;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float sineAdd = (float)Math.Sin(projectile.localAI[1] * 0.1f) + 3;
			Color color = Color.Lerp(Color.Orange, Color.Red, Math.Min(projectile.localAI[0] / 120f, 1));
			color.A = 0;
			Main.spriteBatch.Draw(SpiritMod.Instance.GetTexture("Effects/Masks/Extra_49"), projectile.Center - Main.screenPosition, null, color * 0.7f, 0f, SpiritMod.Instance.GetTexture("Effects/Masks/Extra_49").Size() / 2, 0.1f * (sineAdd + .5f), SpriteEffects.None, 0f);
			return true;
		}
		public void AdditiveCall(SpriteBatch sb)
		{
			float blurLength = 200 * projectile.scale * projectile.Opacity;
			float blurWidth = 25 * projectile.scale * projectile.Opacity;

			Effect blurEffect = mod.GetEffect("Effects/BlurLine");
			SquarePrimitive blurLine = new SquarePrimitive()
			{
				Position = projectile.Center - Main.screenPosition,
				Height = blurWidth,
				Length = blurLength,
				Rotation = 0,
				Color = new Color(255, 222, 181) * projectile.Opacity
			};
			PrimitiveRenderer.DrawPrimitiveShape(blurLine, blurEffect);
		}
	}
}
