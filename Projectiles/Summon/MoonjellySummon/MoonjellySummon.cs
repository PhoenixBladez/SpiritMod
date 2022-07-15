using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.MoonjellySummon
{
	public class MoonjellySummon : Minion
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonlight Preserver");
			Main.projFrames[Projectile.type] = 10;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 50;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 99999;
            Projectile.scale = .8f;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;
		}

		public override void CheckActive() { }

		public override void SelectFrame()
		{
			Projectile.frameCounter++;

			if (Projectile.frameCounter >= 10f)
			{
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
				Projectile.frameCounter = 0;

				if (Projectile.frame >= 10)
					Projectile.frame = 0;
			}
		}

		int counter;
        float alphaCounter;

		public override void Behavior()
		{
			alphaCounter += .04f;
			Player player = Main.player[Projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();

			if (player.dead)
				modPlayer.lunazoa = false;

			if (modPlayer.lunazoa)
				Projectile.timeLeft = 2;

			int summonTime = (int)(34 / (.33f * Projectile.minionSlots));
			if (summonTime >= 110)
				summonTime = 110;

			counter++;
			if (counter % summonTime == 0)
			{
				Vector2 vel = Vector2.UnitY.RotatedByRandom(MathHelper.PiOver2) * new Vector2(5f, 3f);

				int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X + Main.rand.Next(-50, 50), Projectile.Center.Y + Main.rand.Next(-50, 50), vel.X, vel.Y, ModContent.ProjectileType<LunazoaOrbiter>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0.0f, (float)Projectile.whoAmI);
				Main.projectile[p].scale = Main.rand.NextFloat(.4f, 1f);
				Main.projectile[p].timeLeft = (int)(62 / (.33f * Projectile.minionSlots));
				counter = 0;
			}

			Lighting.AddLight(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);

			Projectile.rotation = Projectile.velocity.X * 0.025f;
			float num = Projectile.width * 1.1f;
			for (int i = 0; i < 1000; i++)
			{
				Projectile current = Main.projectile[i];
				if (i != Projectile.whoAmI && current.active && Projectile.owner == current.owner && Projectile.type == current.type && Math.Abs(Projectile.position.X - current.position.X) + Math.Abs(Projectile.position.Y - current.position.Y) < num)
				{
					if (Projectile.position.X < Main.projectile[i].position.X)
						Projectile.velocity.X -= 0.08f;
					else
						Projectile.velocity.X += 0.08f;

					if (Projectile.position.Y < Main.projectile[i].position.Y)
						Projectile.velocity.Y -= 0.08f;
					else
						Projectile.velocity.Y += 0.08f;
				}
			}

			float num21 = 920f;
			bool flag = false;

			for (int j = 0; j < 200; j++)
			{
				NPC nPC = Main.npc[j];
				if (nPC.CanBeChasedBy(this, false))
				{
					float num3 = Vector2.Distance(nPC.Center, Projectile.Center);
					if ((num3 < num21 || !flag) && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, nPC.position, nPC.width, nPC.height))
					{
						num21 = num3;
						flag = true;
					}
				}
			}

			if (Vector2.Distance(player.Center, Projectile.Center) > (flag ? 1000f : 500f))
			{
				Projectile.ai[0] = 1f;
				Projectile.netUpdate = true;
			}

			if (!Collision.CanHitLine(Projectile.Center, 1, 1, player.Center, 1, 1))
				Projectile.ai[0] = 1f;

			float num4 = 6f;
			if (Projectile.ai[0] == 1f)
				num4 = 15f;

			Vector2 center = Projectile.Center;
			Vector2 vector = player.Center - center;
			Projectile.ai[1] = 3600f;
			Projectile.netUpdate = true;
			int num5 = 1;
			for (int k = 0; k < Projectile.whoAmI; k++)
			{
				if (Main.projectile[k].active && Main.projectile[k].owner == Projectile.owner && Main.projectile[k].type == Projectile.type)
					num5++;
			}
			vector.X -= 0;
			vector.Y -= 70f;
			float num6 = vector.Length();

			if (num6 > 200f && num4 < 9f)
				num4 = 9f;

			if (num6 < 100f && Projectile.ai[0] == 1f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
			{
				Projectile.ai[0] = 0f;
				Projectile.netUpdate = true;
			}

			if (num6 > 2000f)
				Projectile.Center = player.Center;

			if (num6 > 48f)
			{
				vector.Normalize();
				vector *= num4;
				float num7 = 10f;
				Projectile.velocity = (Projectile.velocity * num7 + vector) / (num7 + 1f);
			}
			else
			{
				Projectile.direction = Main.player[Projectile.owner].direction;
				Projectile.velocity *= (float)Math.Pow(0.9, 2.0);
			}

			if (Projectile.velocity.X > 0f)
				Projectile.spriteDirection = Projectile.direction = -1;
			else if (Projectile.velocity.X < 0f)
				Projectile.spriteDirection = Projectile.direction = 1;

			if (Projectile.ai[1] > 0f)
				Projectile.ai[1] += 1f;

			if (Projectile.ai[1] > 140f)
			{
				Projectile.ai[1] = 0f;
				Projectile.netUpdate = true;
			}
		}


		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = new Color(44, 168, 67) * 0.75f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

				float scale = Projectile.scale;
				Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/Projectiles/Summon/Zones/StaminaZone", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, color, Projectile.rotation, tex.Size() / 2, scale, default, default);
			}
		}

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, (Projectile.height / Main.projFrames[Projectile.type]) * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                var effects = Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * (float)(((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) / 2);
                Color color1 = Color.White * (float)(((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) / 2);

                Microsoft.Xna.Framework.Color color29 = new Color(127 - Projectile.alpha, 127 - Projectile.alpha, 127 - Projectile.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.LightBlue);
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Projectiles/Summon/MoonjellySummon/MoonjellySummon_Glow").Value, drawPos, TextureAssets.Projectile[Projectile.type].Value.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame), color1, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);

				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, TextureAssets.Projectile[Projectile.type].Value.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, TextureAssets.Projectile[Projectile.type].Value.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);

                float sineAdd = (float)Math.Sin(alphaCounter) + 3;
                SpriteEffects spriteEffects = SpriteEffects.None;
                if (Projectile.spriteDirection == 1)
                    spriteEffects = SpriteEffects.FlipHorizontally;
                int xpos = (int)((Projectile.Center.X + 38) - Main.screenPosition.X) - (int)(TextureAssets.Projectile[Projectile.type].Value.Width / 2);
                int ypos = (int)((Projectile.Center.Y + 30) - Main.screenPosition.Y) - (int)(TextureAssets.Projectile[Projectile.type].Value.Width / 2);
                Texture2D ripple = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value;
                Main.spriteBatch.Draw(ripple, new Vector2(xpos, ypos), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), Projectile.rotation, ripple.Size() / 2f, Projectile.scale * .8f, spriteEffects, 0);
            }
            return false;
        }
    }
}
