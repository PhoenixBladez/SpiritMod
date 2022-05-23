using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
 	public class UrchinBall : ModProjectile
	{
		private bool hasTarget = false;
		private Vector2 relativePoint = Vector2.Zero;

		private bool stuckInTile = false;
		private Point stuckTilePos = new Point(0, 0);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Urchin");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.magic = true;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.aiStyle = 0;
		}

		public override bool CanDamage() => !hasTarget;
		public override bool? CanCutTiles() => !hasTarget;

		public override void AI()
		{
			if (!hasTarget)
			{
				if(stuckInTile) //Check if tile it's stuck in is still active
				{
					projectile.velocity = Vector2.Zero;
					if(!Main.tile[stuckTilePos.X, stuckTilePos.Y].active()) //If not, update and let the projectile fall again
					{
						stuckInTile = false;
						stuckTilePos = new Point(0, 0);
						projectile.netUpdate = true;
					}	
				}

				else
				{
					projectile.velocity.Y += 0.2f;
					projectile.rotation += 0.06f * Math.Sign(projectile.velocity.X);
				}
			}

			else
			{
				NPC npc = Main.npc[(int)projectile.ai[1]];

				if (!npc.CanBeChasedBy(this))
				{
					projectile.netUpdate = true;
					projectile.tileCollide = true;
					projectile.timeLeft *= 2;

					hasTarget = false;
					return;
				}

				projectile.Center = npc.Center + relativePoint;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.timeLeft > 240)
				projectile.timeLeft = 240;

			projectile.velocity = Vector2.Zero;
			stuckInTile = true;
			stuckTilePos = (projectile.Center + oldVelocity).ToTileCoordinates();
			projectile.netUpdate = true;

			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.ai[1] = target.whoAmI;
			projectile.tileCollide = false;
			projectile.netUpdate = true;
			projectile.timeLeft = 240;
			projectile.velocity = new Vector2(0, -0.4f);

			hasTarget = true;
			relativePoint = projectile.Center - target.Center;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 8; ++i)
			{
				Vector2 vel = new Vector2(Main.rand.NextFloat(6f, 8f), 0).RotatedBy(i * MathHelper.TwoPi / 8f).RotatedByRandom(0.33f);
				Projectile.NewProjectile(projectile.Center + (hasTarget ? Vector2.Normalize(relativePoint) * 6 : vel), vel, ModContent.ProjectileType<UrchinSpike>(), projectile.damage, 2f, projectile.owner);
			}

			for (int i = 0; i < 2; ++i)
				Gore.NewGore(projectile.Center, Vector2.Zero, mod.GetGoreSlot("Gores/Projectiles/UrchinLobber/Urchin" + i));

			Main.PlaySound(SoundID.Item14.WithPitchVariance(0.2f).WithVolume(0.4f), projectile.Center);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDrawTrail(spriteBatch, 0.33f);
			projectile.QuickDraw(spriteBatch);

			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			const float Cutoff = 120;

			Texture2D tex = ModContent.GetTexture(Texture + "Glow");

			float flashTimer = (Math.Max(Cutoff - projectile.timeLeft, 0) / Cutoff);
			int numFlashes = 6;
			float alpha = 1 - (float)Math.Pow(Math.Sin(Utilities.EaseFunction.EaseQuadIn.Ease(flashTimer) * numFlashes * MathHelper.Pi), 4);

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * (1 - alpha), projectile.rotation, tex.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width /= 3;
			height /= 3;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(hasTarget);
			writer.Write(stuckInTile);
			writer.WriteVector2(relativePoint);
			writer.WriteVector2(stuckTilePos.ToVector2());
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			hasTarget = reader.ReadBoolean();
			stuckInTile = reader.ReadBoolean();
			relativePoint = reader.ReadVector2();
			stuckTilePos = reader.ReadVector2().ToPoint();
		}
	}

	public class UrchinSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Urchin");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		private bool hasTarget = false;
		private Vector2 relativePoint = Vector2.Zero;

		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 6;
			projectile.magic = true;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.aiStyle = 0;
			projectile.extraUpdates = 1;
			projectile.timeLeft = 40;
			projectile.scale = Main.rand.NextFloat(0.9f, 1.1f);
		}

		public override bool CanDamage() => !hasTarget;
		public override bool? CanCutTiles() => !hasTarget;

		public override void AI()
		{
			projectile.alpha = 255 - (int)((projectile.timeLeft / 50f) * 255);
			projectile.velocity *= 0.96f;
			if (!hasTarget)
				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

			else
			{
				NPC npc = Main.npc[(int)projectile.ai[1]];

				if (!npc.active)
					projectile.Kill();
				else
					projectile.Center = npc.Center + relativePoint;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.ai[1] = target.whoAmI;
			projectile.tileCollide = false;
			projectile.netUpdate = true;
			projectile.timeLeft = 60;
			projectile.velocity = Vector2.Zero;
			projectile.penetrate++;

			hasTarget = true;
			relativePoint = projectile.Center - target.Center;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDrawTrail(spriteBatch, 0.5f);
			projectile.QuickDraw(spriteBatch);
			return false;
		}
	}
}
