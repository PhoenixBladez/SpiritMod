using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;

namespace SpiritMod.Projectiles.Summon.MoonjellySummon
{
	public class LunazoaOrbiter : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunazoa");
			Main.projFrames[Projectile.type] = 3;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 16;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.minion = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			Lighting.AddLight(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
			Projectile.frameCounter++;
			Projectile.spriteDirection = -Projectile.direction;
			if (Projectile.frameCounter >= 10)
			{
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
				Projectile.frameCounter = 0;
			}

			float x = 0.2f;
			float y = 0.2f;
			bool flag2 = false;

			if ((double)Projectile.ai[0] < 60)
			{
				bool flag4 = true;
				int index1 = (int)Projectile.ai[1];
				if (Main.projectile[index1].active && Main.projectile[index1].type == ModContent.ProjectileType<MoonjellySummon>())
				{
					if (!flag2 && Main.projectile[index1].oldPos[1] != Vector2.Zero)
						Projectile.position = Projectile.position + Main.projectile[index1].position - Main.projectile[index1].oldPos[1];
				}
				else
				{
					Projectile.ai[0] = 60;
					flag4 = false;
					Projectile.Kill();
				}
				if (flag4 && !flag2)
				{
					Projectile.velocity += new Vector2((float)Math.Sign(Main.projectile[index1].Center.X - Projectile.Center.X), (float)Math.Sign(Main.projectile[index1].Center.Y - Projectile.Center.Y)) * new Vector2(x, y);
					if (Projectile.velocity.Length() > 4f)
						Projectile.velocity *= 4f / Projectile.velocity.Length();
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			NPC mainTarget = Projectile.OwnerMinionAttackTargetNPC;
			if (mainTarget != null && mainTarget.CanBeChasedBy(Projectile))
			{
				float dist = Projectile.Distance(mainTarget.Center);
				if (dist / 16 < 30)
				{
					SoundEngine.PlaySound(SoundID.Item110, Projectile.position);
					Vector2 direction = Vector2.Normalize(mainTarget.Center - Projectile.Center) * 15f;
					Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, direction, ModContent.ProjectileType<JellyfishOrbiter_Friendly>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
					p.friendly = true;
					p.hostile = false;
					p.minion = true;
					p.netUpdate = true;
					p.scale = Projectile.scale;
				}
			}
			else
			{
				for (int npcFinder = 0; npcFinder < 200; ++npcFinder)
				{
					NPC npc = Main.npc[npcFinder];
					if (npc.active && npc.CanBeChasedBy(Projectile) && !npc.friendly)
					{
						//if npc is within 50 blocks
						float dist = Projectile.Distance(npc.Center);
						if (dist / 16 < 30)
						{
							if (!Main.npc[npcFinder].friendly && !Main.npc[npcFinder].townNPC && Main.npc[npcFinder].active)
							{
								SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 110);
								Vector2 direction = Vector2.Normalize(Main.npc[npcFinder].Center - Projectile.Center) * 15f;
								Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, direction, ModContent.ProjectileType<JellyfishOrbiter_Friendly>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
								p.friendly = true;
								p.hostile = false;
								p.minion = true;
								p.netUpdate = true;
								p.scale = Projectile.scale;
								break;
							}
						}
					}
				}
			}

			for (int k = 0; k < 10; k++)
			{
				Dust d = Dust.NewDustPerfect(Projectile.Center, 226, Vector2.One.RotatedByRandom(3.28f) * Main.rand.NextFloat(5), 0, default, Main.rand.NextFloat(.4f, .8f));
				d.noGravity = true;
			}
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => behindProjectiles.Add(index);

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, (Projectile.height / Main.projFrames[Projectile.type]) * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				var effects = Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * (float)(((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) / 2);
				Color color1 = Color.White * (float)(((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) / 2);
				Texture2D glow = ModContent.Request<Texture2D>("Projectiles/Summon/MoonjellySummon/LunazoaOrbiter_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				Main.spriteBatch.Draw(glow, drawPos, new Microsoft.Xna.Framework.Rectangle?(TextureAssets.Projectile[Projectile.type].Value.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame)), color1, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
				Main.spriteBatch.Draw(glow, drawPos, new Microsoft.Xna.Framework.Rectangle?(TextureAssets.Projectile[Projectile.type].Value.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame)), color1, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);


				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(TextureAssets.Projectile[Projectile.type].Value.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame)), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(TextureAssets.Projectile[Projectile.type].Value.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame)), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
			}
			return false;
		}
	}
}