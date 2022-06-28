using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.aiStyle = 0;
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => !hasTarget;
		public override bool? CanCutTiles() => !hasTarget;

		public override void AI()
		{
			if (!hasTarget)
			{
				if(stuckInTile) //Check if tile it's stuck in is still active
				{
					Projectile.velocity = Vector2.Zero;
					if(!Main.tile[stuckTilePos.X, stuckTilePos.Y].HasTile) //If not, update and let the projectile fall again
					{
						stuckInTile = false;
						stuckTilePos = new Point(0, 0);
						Projectile.netUpdate = true;
					}	
				}

				else
				{
					Projectile.velocity.Y += 0.2f;
					Projectile.rotation += 0.06f * Math.Sign(Projectile.velocity.X);
				}
			}

			else
			{
				NPC npc = Main.npc[(int)Projectile.ai[1]];

				if (!npc.CanBeChasedBy(this) && npc.type != NPCID.TargetDummy)
				{
					Projectile.netUpdate = true;
					Projectile.tileCollide = true;
					Projectile.timeLeft *= 2;

					hasTarget = false;
					return;
				}

				Projectile.Center = npc.Center + relativePoint;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.timeLeft > 240)
				Projectile.timeLeft = 240;

			Projectile.velocity = Vector2.Zero;
			stuckInTile = true;
			stuckTilePos = (Projectile.Center + oldVelocity).ToTileCoordinates();
			Projectile.netUpdate = true;

			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.ai[1] = target.whoAmI;
			Projectile.tileCollide = false;
			Projectile.netUpdate = true;
			Projectile.timeLeft = 240;
			Projectile.velocity = new Vector2(0, -0.4f);

			hasTarget = true;
			relativePoint = Projectile.Center - target.Center;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 8; ++i)
			{
				Vector2 vel = new Vector2(Main.rand.NextFloat(6f, 8f), 0).RotatedBy(i * MathHelper.TwoPi / 8f).RotatedByRandom(0.33f);
				Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center + (hasTarget ? Vector2.Normalize(relativePoint) * 6 : vel), vel, ModContent.ProjectileType<UrchinSpike>(), Projectile.damage, 2f, Projectile.owner);
			}

			for (int i = 0; i < 2; ++i)
				Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, Mod.Find<ModGore>("Gores/Projectiles/UrchinLobber/Urchin" + i).Type);

			SoundEngine.PlaySound(SoundID.Item14 with { PitchVariance = 0.2f, Volume = 0.4f }, Projectile.Center);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.QuickDrawTrail(Main.spriteBatch, 0.33f);
			Projectile.QuickDraw(Main.spriteBatch);

			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			const float Cutoff = 120;

			Texture2D tex = ModContent.Request<Texture2D>(Texture + "Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			float flashTimer = (Math.Max(Cutoff - Projectile.timeLeft, 0) / Cutoff);
			int numFlashes = 6;
			float alpha = 1 - (float)Math.Pow(Math.Sin(Utilities.EaseFunction.EaseQuadIn.Ease(flashTimer) * numFlashes * MathHelper.Pi), 4);

			Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * (1 - alpha), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 3;
			height /= 3;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		private bool hasTarget = false;
		private Vector2 relativePoint = Vector2.Zero;

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.aiStyle = 0;
			Projectile.extraUpdates = 1;
			Projectile.timeLeft = 40;
			Projectile.scale = Main.rand.NextFloat(0.9f, 1.1f);
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => !hasTarget;
		public override bool? CanCutTiles() => !hasTarget;

		public override void AI()
		{
			Projectile.alpha = 255 - (int)((Projectile.timeLeft / 50f) * 255);
			Projectile.velocity *= 0.96f;
			if (!hasTarget)
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			else
			{
				NPC npc = Main.npc[(int)Projectile.ai[1]];

				if (!npc.active)
					Projectile.Kill();
				else
					Projectile.Center = npc.Center + relativePoint;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.ai[1] = target.whoAmI;
			Projectile.tileCollide = false;
			Projectile.netUpdate = true;
			Projectile.timeLeft = 60;
			Projectile.velocity = Vector2.Zero;
			Projectile.penetrate++;

			hasTarget = true;
			relativePoint = Projectile.Center - target.Center;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.QuickDrawTrail(Main.spriteBatch, 0.5f);
			Projectile.QuickDraw(Main.spriteBatch);
			return false;
		}
	}
}
