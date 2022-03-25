using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	public class SwarmScarab : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarab");
			Main.npcFrameCount[npc.type] = 2;
			NPCID.Sets.TrailCacheLength[npc.type] = 10;
			NPCID.Sets.TrailingMode[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.width = 12;
			npc.height = 12;
			npc.lifeMax = 5;
			npc.damage = 28;
			npc.value = 0;
			npc.friendly = false;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.aiStyle = -1;
			npc.alpha = 0;
			npc.DeathSound = SoundID.NPCDeath16;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => npc.lifeMax = 5;
		public override bool PreNPCLoot()
		{
			for (int i = 0; i < 10; i++) {
				int randomDustType = Main.rand.Next(3);
				if (randomDustType == 0)
					randomDustType = 5;
				else if (randomDustType == 1)
					randomDustType = 36;
				else
					randomDustType = 32;
				int d = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, randomDustType, npc.oldVelocity.X * 0.2f, npc.oldVelocity.Y * 0.2f);
				Main.dust[d].noGravity = true;
				Main.dust[d].scale = 1.2f;
			}
			return false;
		}
		int frame = 0;
		public override void AI()
		{
			npc.rotation = npc.velocity.ToRotation();
			npc.localAI[1]++;
			npc.TargetClosest(false);
			npc.spriteDirection = Math.Sign(npc.velocity.X);
			npc.position += npc.velocity;
			if (!NPC.AnyNPCs(ModContent.NPCType<Scarabeus>()) || npc.localAI[1] > 360) {
				npc.life = 0;
				npc.active = false;
				NPCLoot();
			}
			if (npc.ai[1] == 0) {
				if (npc.ai[2] == 0)
					npc.velocity = Vector2.UnitX.RotatedBy(npc.ai[0]) * 4;
				npc.ai[2]++;
				if(++npc.ai[2] > 220) {
					npc.alpha += 9;
					if(npc.alpha > 255) {
						npc.life = 0;
						npc.active = false;
					}
				}
				else
					npc.alpha = Math.Max(npc.alpha - 9, 0); 
				if (npc.velocity.Length() < 18 && npc.ai[2] > 60)
					npc.velocity *= 1.02f;
			}
			else {
				if(npc.ai[2] == 0) 
					npc.velocity = Vector2.UnitX.RotatedBy(npc.ai[0]) * Main.rand.NextFloat(8, 12);

				if(++npc.ai[2] > 110) {
					npc.alpha += 7;
					if (npc.alpha > 255) {
						npc.life = 0;
						npc.active = false;
					}
				}
				else 
					npc.alpha = Math.Max(npc.alpha - 7, 0);
				
				if (npc.velocity.Length() < 18)
					npc.velocity *= 1.02f;
			}
			UpdateFrame(10, 0, 2);
		}
		private void UpdateFrame(int framespersecond, int minframe, int maxframe)
		{
			npc.frameCounter++;
			if (npc.frameCounter >= (60 / framespersecond)) {
				frame++;
				npc.frameCounter = 0;
			}
			if (frame >= maxframe || frame < minframe) {
				frame = minframe;
			}
		}
		public override void FindFrame(int frameHeight) => npc.frame.Y = frameHeight * frame;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture2D = Main.npcTexture[npc.type];
			SpriteEffects spriteeffects = (npc.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			float rotation = (npc.spriteDirection < 0) ? npc.rotation - MathHelper.Pi : npc.rotation;
			Vector2 drawOrigin = npc.frame.Size() / 2;
			if (npc.ai[1] == 1) {
				for (int k = 0; k < npc.oldPos.Length; k++) {
					Vector2 drawPos = Vector2.Lerp(npc.oldPos[k], npc.position, 0.5f) - Main.screenPosition + new Vector2(npc.width / 2, npc.height / 2);
					Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, spriteeffects, 0f);
				}
			}
			spriteBatch.Draw(texture2D, npc.Center - Main.screenPosition, npc.frame, npc.GetAlpha(lightColor), rotation, npc.frame.Size() / 2f, npc.scale, spriteeffects, 0f);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture2D = mod.GetTexture("NPCs/Boss/Scarabeus/SwarmScarab_glow");
			SpriteEffects spriteeffects = (npc.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			float rotation = (npc.spriteDirection < 0) ? npc.rotation - MathHelper.Pi : npc.rotation;
			spriteBatch.Draw(texture2D, npc.Center - Main.screenPosition, npc.frame, npc.GetAlpha(Color.White), rotation, npc.frame.Size() / 2f, npc.scale, spriteeffects, 0f);
		}
	}
}