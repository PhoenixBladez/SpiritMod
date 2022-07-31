using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	public class SwarmScarab : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarab");
			Main.npcFrameCount[NPC.type] = 2;
			NPCID.Sets.TrailCacheLength[NPC.type] = 10;
			NPCID.Sets.TrailingMode[NPC.type] = 1;

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { Hide = true };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults()
		{
			NPC.width = 12;
			NPC.height = 12;
			NPC.lifeMax = 5;
			NPC.damage = 28;
			NPC.value = 0;
			NPC.friendly = false;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.aiStyle = -1;
			NPC.alpha = 0;
			NPC.DeathSound = SoundID.NPCDeath16;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => NPC.lifeMax = 5;
		public override bool PreKill()
		{
			for (int i = 0; i < 10; i++) {
				int randomDustType = Main.rand.Next(3);
				if (randomDustType == 0)
					randomDustType = 5;
				else if (randomDustType == 1)
					randomDustType = 36;
				else
					randomDustType = 32;
				int d = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, randomDustType, NPC.oldVelocity.X * 0.2f, NPC.oldVelocity.Y * 0.2f);
				Main.dust[d].noGravity = true;
				Main.dust[d].scale = 1.2f;
			}
			return false;
		}
		int frame = 0;
		public override void AI()
		{
			NPC.rotation = NPC.velocity.ToRotation();
			NPC.localAI[1]++;
			NPC.TargetClosest(false);
			NPC.spriteDirection = Math.Sign(NPC.velocity.X);
			NPC.position += NPC.velocity;
			if (!NPC.AnyNPCs(ModContent.NPCType<Scarabeus>()) || NPC.localAI[1] > 360) {
				NPC.life = 0;
				NPC.active = false;
				NPC.NPCLoot();
			}
			if (NPC.ai[1] == 0) {
				if (NPC.ai[2] == 0)
					NPC.velocity = Vector2.UnitX.RotatedBy(NPC.ai[0]) * 4;
				NPC.ai[2]++;
				if(++NPC.ai[2] > 220) {
					NPC.alpha += 9;
					if(NPC.alpha > 255) {
						NPC.life = 0;
						NPC.active = false;
					}
				}
				else
					NPC.alpha = Math.Max(NPC.alpha - 9, 0); 
				if (NPC.velocity.Length() < 18 && NPC.ai[2] > 60)
					NPC.velocity *= 1.02f;
			}
			else {
				if(NPC.ai[2] == 0) 
					NPC.velocity = Vector2.UnitX.RotatedBy(NPC.ai[0]) * Main.rand.NextFloat(8, 12);

				if(++NPC.ai[2] > 110) {
					NPC.alpha += 7;
					if (NPC.alpha > 255) {
						NPC.life = 0;
						NPC.active = false;
					}
				}
				else 
					NPC.alpha = Math.Max(NPC.alpha - 7, 0);
				
				if (NPC.velocity.Length() < 18)
					NPC.velocity *= 1.02f;
			}
			UpdateFrame(10, 0, 2);
		}
		private void UpdateFrame(int framespersecond, int minframe, int maxframe)
		{
			NPC.frameCounter++;
			if (NPC.frameCounter >= (60 / framespersecond)) {
				frame++;
				NPC.frameCounter = 0;
			}
			if (frame >= maxframe || frame < minframe) {
				frame = minframe;
			}
		}
		public override void FindFrame(int frameHeight) => NPC.frame.Y = frameHeight * frame;

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture2D = TextureAssets.Npc[NPC.type].Value;
			SpriteEffects spriteeffects = (NPC.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			float rotation = (NPC.spriteDirection < 0) ? NPC.rotation - MathHelper.Pi : NPC.rotation;
			Vector2 drawOrigin = NPC.frame.Size() / 2;
			if (NPC.ai[1] == 1) {
				for (int k = 0; k < NPC.oldPos.Length; k++) {
					Vector2 drawPos = Vector2.Lerp(NPC.oldPos[k], NPC.position, 0.5f) - Main.screenPosition + new Vector2(NPC.width / 2, NPC.height / 2);
					Color color = NPC.GetAlpha(drawColor) * (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
					spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, spriteeffects, 0f);
				}
			}
			spriteBatch.Draw(texture2D, NPC.Center - Main.screenPosition, NPC.frame, NPC.GetAlpha(drawColor), rotation, NPC.frame.Size() / 2f, NPC.scale, spriteeffects, 0f);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture2D = Mod.Assets.Request<Texture2D>("NPCs/Boss/Scarabeus/SwarmScarab_glow").Value;
			SpriteEffects spriteeffects = (NPC.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			float rotation = (NPC.spriteDirection < 0) ? NPC.rotation - MathHelper.Pi : NPC.rotation;
			spriteBatch.Draw(texture2D, NPC.Center - Main.screenPosition, NPC.frame, NPC.GetAlpha(Color.White), rotation, NPC.frame.Size() / 2f, NPC.scale, spriteeffects, 0f);
		}
	}
}