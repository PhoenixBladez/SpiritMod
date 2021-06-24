using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.BaseProj;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class HolyKnifeMinion : BaseMinion, IDrawAdditive
    {
		public HolyKnifeMinion() : base(600, 1200, new Vector2(12, 12)) { }

		public override void AbstractSetStaticDefaults()
		{
			DisplayName.SetDefault("Spellsword");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void AbstractSetDefaults()
		{
			projectile.minionSlots = 0f;
			projectile.localNPCHitCooldown = 30;
		}

		public override bool PreAI()
		{
			MyPlayer mp = Main.player[projectile.owner].GetModPlayer<MyPlayer>();
			if (mp.player.dead || !mp.spellswordCrest || !mp.player.active)
				projectile.active = false;

			if (mp.spellswordCrest)
				projectile.timeLeft = 2;

			return true;
		}

        public void AdditiveCall(SpriteBatch spriteBatch)
		{
			float scale = projectile.scale;
			Texture2D tex = ModContent.GetTexture("SpiritMod/Projectiles/Summon/HolyKnifeMinion_Glow");
			Color color = new Color(255, 255, 200) * 0.75f;

			if (projectile.ai[1] == 1)
			{
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{
					color = new Color(255, 255, 200) * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

					spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
					spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color * .56f, projectile.rotation, tex.Size() / 2, scale * 1.762f, default, default);
				}
			}

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale * 1.5f, default, default);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale * 1.33f, default, default);
		}

		private const int Returning = 0;
		private const int Attacking = 1;
		private const int LockedToPlayer = 2;

		private ref float AiTimer => ref projectile.ai[0];
		private ref float AiState => ref projectile.ai[1];
		public override void IdleMovement(Player player)
		{
			Vector2 desiredPos = player.Center + new Vector2(0, -50 + (float)Math.Sin(Main.GameUpdateCount / 10f) * 10);
			projectile.ai[0] = 0;
			if(AiState == Attacking)
				AiState = Returning;

			projectile.rotation = Utils.AngleLerp(projectile.rotation, 0, 0.07f);

			if (AiState != LockedToPlayer && projectile.Distance(desiredPos) > 25)
			{
				projectile.extraUpdates = (int)MathHelper.Clamp(projectile.Distance(desiredPos) / 100, 1, 4);
				projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(desiredPos) * 6, 0.1f);
			}
			else
			{
				AiState = LockedToPlayer;
				projectile.extraUpdates = 0;
				projectile.velocity = Vector2.Zero;
				projectile.Center = desiredPos;
			}
		}

		public override void TargettingBehavior(Player player, NPC target)
		{
			projectile.extraUpdates = 1 + (int)MathHelper.Clamp(projectile.Distance(target.Center) / 150, 1, 5);
			AiState = Attacking;
			if (projectile.Distance(target.Center) > 300)
			{
				AiTimer = 0;
				projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(target.Center) * 8, 0.1f);

				projectile.rotation = Utils.AngleLerp(projectile.rotation, projectile.velocity.ToRotation() + MathHelper.PiOver2, 0.04f);
			}

			else
			{
				--AiTimer;
				if (AiTimer <= 0)
				{
					AiTimer = Main.rand.Next(2, 6) * 20;
					projectile.velocity = projectile.DirectionTo(target.Center).RotatedByRandom(MathHelper.Pi / 16) * Main.rand.NextFloat(10, 12) * MathHelper.Clamp(projectile.Distance(target.Center) / 100, 1f, 2f);
					projectile.netUpdate = true;
				}

				projectile.velocity = projectile.velocity.Length() * 0.96f * Vector2.Normalize(Vector2.Lerp(projectile.velocity, projectile.DirectionTo(target.Center) * projectile.velocity.Length(), 0.2f));

				projectile.rotation = Utils.AngleLerp(projectile.rotation, projectile.velocity.ToRotation() + MathHelper.PiOver2, 0.04f);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = Main.projectileTexture[projectile.type].Size()/2;
			if (projectile.ai[1] == 1)
			{
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = projectile.oldPos[k] + projectile.Size/2 - Main.screenPosition;
					Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
					spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color * .6f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
				}
			}
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);

			return false;
        }
	}
}
