using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Boss;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.ReachBoss
{
	[AutoloadBossHead]
	public class ReachBoss : ModNPC
	{
		int moveSpeed = 0;
		bool text = false;
		int moveSpeedY = 0;
		float HomeY = 150f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vinewrath Bane");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.width = 132;
			npc.height = 222;
			npc.damage = 28;
            npc.boss = true;
			npc.lifeMax = 2500;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.npcSlots = 20;
			npc.defense = 15;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/ReachBoss");
			npc.buffImmune[20] = true;
			npc.buffImmune[31] = true;
			npc.buffImmune[70] = true;

			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
		}

		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.301f, 0.110f, 0.126f);
			Player player = Main.player[npc.target];
			bool expertMode = Main.expertMode;
			if (!player.active || player.dead) {
				npc.TargetClosest(false);
				npc.velocity.Y = -2000;
			}
			if (player.GetSpiritPlayer().ZoneReach) {
				npc.defense = 25;
				npc.damage = 45;
			}
			else {
				npc.defense = 14;
				npc.damage = 28;
			}
			npc.ai[0]++;
			if (npc.ai[0] < 470) {
				if (npc.Center.X >= player.Center.X && moveSpeed >= -30) // flies to players x position
					moveSpeed--;
				else if (npc.Center.X <= player.Center.X && moveSpeed <= 30)
					moveSpeed++;

				npc.velocity.X = moveSpeed * 0.1f;

				if (npc.Center.Y >= player.Center.Y - 30f && moveSpeedY >= -20) //Flies to players Y position
					moveSpeedY--;
				else if (npc.Center.Y <= player.Center.Y - 30f && moveSpeedY <= 20)
					moveSpeedY++;
				npc.velocity.Y = moveSpeedY * 0.1f;
			}
			if (npc.ai[0] == 470 || npc.ai[0] == 540 || npc.ai[0] == 590 || npc.ai[0] == 670)
			{
				npc.velocity = Vector2.Zero;
				npc.netUpdate = true;
			}
			if (npc.ai[0] > 480  && npc.ai[0] < 730)
			{
				sideFloat();
				pulseTrail = true;
				num1 = 14f;
				movement = .5f;
			}			
			if (npc.ai[0] > 730)
			{
				pulseTrail = false;
				npc.ai[0] = 0;
				npc.netUpdate = true;
			}
			
			npc.spriteDirection = npc.direction;
		}
		public void sideFloat()
		{
			bool expertMode = Main.expertMode;
			if (npc.ai[0] >= 480 && npc.ai[0] < 540) {

				npc.TargetClosest(true);
				vector2_1 = Main.player[npc.target].Center - npc.Center + new Vector2(150, -150f);
				num2 = vector2_1.Length();

				if ((double)num2 < 20.0)
					desiredVelocity = npc.velocity;
				else if ((double)num2 < 40.0) {
					vector2_1.Normalize();
					desiredVelocity = vector2_1 * (num1 * 0.45f);
				}
				else if ((double)num2 < 80.0) {
					vector2_1.Normalize();
					desiredVelocity = vector2_1 * (num1 * 0.75f);
				}
				else {
					vector2_1.Normalize();
					desiredVelocity = vector2_1 * num1;
				}
				npc.SimpleFlyMovement(desiredVelocity, movement);			
			}
			if (npc.ai[0] >= 600 && npc.ai[0] < 670)
			{
				vector2_1 = Main.player[npc.target].Center - npc.Center + new Vector2(-150f, -150f);
				num2 = vector2_1.Length();

				if ((double)num2 < 20.0)
					desiredVelocity = npc.velocity;
				else if ((double)num2 < 40.0) {
					vector2_1.Normalize();
					desiredVelocity = vector2_1 * (num1 * 0.45f);
				}
				else if ((double)num2 < 80.0) {
					vector2_1.Normalize();
					desiredVelocity = vector2_1 * (num1 * 0.75f);
				}
				else {
					vector2_1.Normalize();
					desiredVelocity = vector2_1 * num1;
				}
				
				npc.SimpleFlyMovement(desiredVelocity, movement);
			}
			if (npc.ai[0] == 560 || npc.ai[0] == 690)
			{
				Main.PlaySound(SoundID.Grass, (int)npc.position.X, (int)npc.position.Y);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    direction.Normalize();
                    direction.X *= 14f;
                    direction.Y *= 14f;

                    int amountOfProjectiles = Main.rand.Next(3, 5);
                    for (int i = 0; i < amountOfProjectiles; ++i)
                    {
                        float A = (float)Main.rand.Next(-200, 200) * 0.05f;
                        float B = (float)Main.rand.Next(-200, 200) * 0.05f;
                        int damage = expertMode ? 11 : 16;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<BossRedSpike>(), damage, 1, Main.myPlayer, 0, 0);
                    }
                }
			}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += .15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		float num1;
		float num2;
		bool pulseTrail;
		float movement;
		Vector2 vector2_1;
		Vector2 desiredVelocity;
		
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.66f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			float num108 = 4;
            float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
            float num106 = 0f;
			Color color1 = Color.White * num107 * .8f;
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(
				mod.GetTexture("NPCs/Boss/ReachBoss/ReachBoss_Glow"),
				npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY),
				npc.frame,
				color1,
				npc.rotation,
				npc.frame.Size() / 2,
				npc.scale,
				effects,
				0
			);
			if (pulseTrail)
			{
				SpriteEffects spriteEffects3 = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				Vector2 vector33 = new Vector2(npc.Center.X, npc.Center.Y - 18) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity;
				Color color29 = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.Tomato);
				for (int num103 = 0; num103 < 4; num103++)
				{
					Color color28 = color29;
					color28 = npc.GetAlpha(color28);
					color28 *= 1f - num107;
					Vector2 vector29 = new Vector2(npc.Center.X, npc.Center.Y -18) + ((float)num103 / (float)num108 * 6.28318548f + npc.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)num103;
					Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/ReachBoss/ReachBoss_Glow"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
				}
			}
		}

	
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient && npc.life <= 0) {
				if (!text) {
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), new Color(0, 200, 80, 100),
					"You cannot stop the wrath of nature!");
					text = true;
				}
				Vector2 spawnAt = npc.Center + new Vector2(0f, (float)npc.height / 2f);
				NPC.NewNPC((int)spawnAt.X, (int)spawnAt.Y, mod.NPCType("ReachBoss1"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);

				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss1"), 1f);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 2, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}
	}
}
