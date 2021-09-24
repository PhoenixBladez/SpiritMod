using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;

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

	public class JetBrickProj : BaseFlailProj
	{
		public JetBrickProj() : base(new Vector2(0.7f, 1.6f), new Vector2(0.5f, 2f), 2, 70, 13) { }

		public override void SetStaticDefaults() => DisplayName.SetDefault("Jet Brick");

		public override void SpinExtras(Player player)
		{
			projectile.localAI[1]++;
			if (projectile.localAI[0]++ == 0)
				SpiritMod.primitives.CreateTrail(new JetBrickPrimTrail(projectile));

			AddColor();
			CreateSmoke(3, true, player);
		}

		public override void NotSpinningExtras(Player player)
		{
			projectile.localAI[1]++;
			AddColor();
			CreateSmoke(5, false, player);
		}

		public override void LaunchExtras(Player player)
		{
			if (projectile.Distance(player.Center) > 190 && projectile.localAI[0] > 100)
				player.velocity = projectile.velocity * 0.7f;

			base.LaunchExtras(player);
		}

		private void AddColor()
		{
			Color color = Color.Lerp(Color.Orange, Color.Red, Math.Min(projectile.localAI[0] / 120f, 1));
			Lighting.AddLight(projectile.Center, color.ToVector3());
		}

		private void CreateSmoke(int rate, bool spinning, Player player)
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
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => projectile.velocity = projectile.DirectionFrom(target.Center) * projectile.velocity.Length() * 0.8f;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float sineAdd = (float)Math.Sin(projectile.localAI[1] * 0.1f) + 3;
			Color color = Color.Lerp(Color.Orange, Color.Red, Math.Min(projectile.localAI[0] / 120f, 1));
			color.A = 0;
			Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), projectile.Center - Main.screenPosition, null, color * 0.7f, 0f, SpiritMod.instance.GetTexture("Effects/Masks/Extra_49").Size() / 2, 0.1f * (sineAdd + 1), SpriteEffects.None, 0f);
			return true;
		}
	}
}
