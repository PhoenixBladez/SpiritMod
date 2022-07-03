using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.BaseProj;
using System;
using Terraria;
using Terraria.GameContent;
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

		public override void AbstractSetDefaults()
		{
			Projectile.minionSlots = 0f;
			Projectile.localNPCHitCooldown = 30;
		}

		public override bool PreAI()
		{
			MyPlayer mp = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
			if (mp.Player.dead || !mp.spellswordCrest || !mp.Player.active)
				Projectile.active = false;

			if (mp.spellswordCrest)
				Projectile.timeLeft = 2;

			return true;
		}

        public void AdditiveCall(SpriteBatch spriteBatch)
		{
			float scale = Projectile.scale;
			Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/Projectiles/Summon/HolyKnifeMinion_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Color color = new Color(255, 255, 200) * 0.75f;

			if (Projectile.ai[1] == 1)
			{
				for (int k = 0; k < Projectile.oldPos.Length; k++)
				{
					color = new Color(255, 255, 200) * 0.75f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

					spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, color, Projectile.rotation, tex.Size() / 2, scale, default, default);
					spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, color * .56f, Projectile.rotation, tex.Size() / 2, scale * 1.762f, default, default);
				}
			}

			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, tex.Size() / 2, scale * 1.5f, default, default);
			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, tex.Size() / 2, scale * 1.33f, default, default);
		}

		private const int Returning = 0;
		private const int Attacking = 1;
		private const int LockedToPlayer = 2;

		private ref float AiTimer => ref Projectile.ai[0];
		private ref float AiState => ref Projectile.ai[1];
		public override void IdleMovement(Player player)
		{
			Vector2 desiredPos = player.Center + new Vector2(0, -50 + (float)Math.Sin(Main.GameUpdateCount / 10f) * 10);
			Projectile.ai[0] = 0;
			if(AiState == Attacking)
				AiState = Returning;

			Projectile.rotation = Utils.AngleLerp(Projectile.rotation, 0, 0.07f);

			if (AiState != LockedToPlayer && Projectile.Distance(desiredPos) > 25)
			{
				Projectile.extraUpdates = (int)MathHelper.Clamp(Projectile.Distance(desiredPos) / 100, 1, 4);
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(desiredPos) * 6, 0.1f);
			}
			else
			{
				AiState = LockedToPlayer;
				Projectile.extraUpdates = 0;
				Projectile.velocity = Vector2.Zero;
				Projectile.Center = desiredPos;
			}
		}

		public override void TargettingBehavior(Player player, NPC target)
		{
			Projectile.extraUpdates = 1 + (int)MathHelper.Clamp(Projectile.Distance(target.Center) / 150, 1, 5);
			AiState = Attacking;
			if (Projectile.Distance(target.Center) > 300)
			{
				AiTimer = 0;
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target.Center) * 8, 0.1f);

				Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Projectile.velocity.ToRotation() + MathHelper.PiOver2, 0.04f);
			}

			else
			{
				CanRetarget = AiTimer <= 1;
				--AiTimer;
				if (AiTimer <= 0)
				{
					AiTimer = Main.rand.Next(2, 6) * 20;
					Projectile.velocity = Projectile.DirectionTo(target.Center).RotatedByRandom(MathHelper.Pi / 16) * Main.rand.NextFloat(10, 12) * MathHelper.Clamp(Projectile.Distance(target.Center) / 100, 1.33f, 1.75f);
					Projectile.netUpdate = true;
				}

				Projectile.velocity = Projectile.velocity.Length() * 0.97f * Vector2.Normalize(Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target.Center) * Projectile.velocity.Length(), 0.2f));

				Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Projectile.velocity.ToRotation() + MathHelper.PiOver2, 0.04f);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = TextureAssets.Projectile[Projectile.type].Value.Size()/2;
			if (Projectile.ai[1] == 1)
			{
				for (int k = 0; k < Projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size/2 - Main.screenPosition;
					Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color * .6f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
				}
			}
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);

			return false;
        }
	}
}
