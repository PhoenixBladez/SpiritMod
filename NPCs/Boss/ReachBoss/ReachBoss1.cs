using SpiritMod.Items.Sets.VinewrathDrops;
using Terraria;
using Terraria.Audio;
using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using SpiritMod.Items.Placeable.Relics;

namespace SpiritMod.NPCs.Boss.ReachBoss
{
	[AutoloadBossHead]
	public class ReachBoss1 : ModNPC
	{
		int moveSpeed = 0;
		int moveSpeedY = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vinewrath Husk");
			Main.npcFrameCount[NPC.type] = 5;
		}

		public override void SetDefaults()
		{
			NPC.Size = new Vector2(80, 120);
			NPC.damage = 36;
			NPC.lifeMax = 2050;
			NPC.knockBackResist = 0;
			NPC.boss = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.npcSlots = 20;
			NPC.defense = 10;
			Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ReachBoss");
			NPC.buffImmune[20] = true;
			NPC.buffImmune[31] = true;
			NPC.buffImmune[70] = true;

			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.3f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void AI()
		{
			Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), 0.075f, 0.184f, 0.062f);			
			NPC.spriteDirection = NPC.direction;
			Player player = Main.player[NPC.target];
			bool expertMode = Main.expertMode;			
			if (!player.active || player.dead) {
				NPC.TargetClosest(false);
				NPC.velocity.Y = -2000;
			}
			if (NPC.life <= NPC.lifeMax/2)
			{
				NPC.ai[0]+= 1.5f;
			}
			else
			{
				NPC.ai[0]++;
			}
			if (NPC.ai[0] < 120 || NPC.ai[0] > 180 && NPC.ai[0] < 300)
			{
				generalMovement(player);
			}
			if (NPC.ai[0] == 150)
			{
				SoundEngine.PlaySound(SoundID.Grass, NPC.Center);
				SoundEngine.PlaySound(SoundID.Item104 with { PitchVariance = 0.2f }, NPC.Center);
			    DustHelper.DrawStar(NPC.Center, 163, pointAmount: 163, mainSize: 2.7425f, dustDensity: 4, dustSize: .65f, pointDepthMult: 3.6f, noGravity: true);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
                    direction.Normalize();
                    direction.X *= 8f;
                    direction.Y *= 8f;

                    int amountOfProjectiles = 2;
					if (NPC.life <= NPC.lifeMax / 2)
					{
						amountOfProjectiles = 3;
					}
                    for (int i = 0; i < amountOfProjectiles; ++i)
                    {
                        float A = (float)Main.rand.Next(-200, 200) * 0.01f;
                        float B = (float)Main.rand.Next(-200, 200) * 0.01f;
                        int damage = expertMode ? 15 : 17;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<BouncingSpore>(), damage, 1, Main.myPlayer, 0, 0);
                    }
                }
			}
			float[] stoptimes = new float[] { 120, 300};
			if (stoptimes.Contains(NPC.ai[0]))
			{
				NPC.velocity = Vector2.Zero;
				NPC.netUpdate = true;
			}
			if (NPC.ai[0] >= 300 && NPC.ai[0] < 900)
			{
				DashAttack(player);
			}
			if (NPC.life <= (NPC.lifeMax / 2)) {
				if (NPC.ai[3] == 0) {
					NPC.ai[0] = 0;
					CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), new Color(0, 200, 80, 100), "The Bramble shall consume you...");
					SoundEngine.PlaySound(SoundID.Grass, NPC.Center);
					SoundEngine.PlaySound(SoundID.DD2_WyvernScream, NPC.Center);
                    NPC.ai[3]++;
					NPC.netUpdate = true;
                }
			}
			if (NPC.ai[0] > 460 && NPC.ai[0] < 501 || NPC.ai[0] > 661 && NPC.ai[0] < 699)
			{
				CircleSpikeAttack(player);
			}
			if (NPC.ai[0] >= 900)
			{
				NPC.ai[0] = 0;
				NPC.ai[1] = 0;
				NPC.ai[2] = 0;
				NPC.netUpdate = true;				
			}			
		}
		public void generalMovement(Player player)
		{
			float value12 = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * 6.28318548f)) * 60f;
			if (NPC.Center.X >= player.Center.X && moveSpeed >= -37) // flies to players x position
				moveSpeed--;
			else if (NPC.Center.X <= player.Center.X && moveSpeed <= 37)
				moveSpeed++;

			NPC.velocity.X = moveSpeed * 0.16f;
			NPC.rotation = NPC.velocity.X * 0.04f;

			if (NPC.Center.Y >= player.Center.Y - 140f + value12 && moveSpeedY >= -20) //Flies to players Y position
				moveSpeedY--;
			else if (NPC.Center.Y <= player.Center.Y - 140f + value12 && moveSpeedY <= 20)
				moveSpeedY++;
			NPC.velocity.Y = moveSpeedY * 0.12f;			
		}
		public void CircleSpikeAttack(Player player)
		{
			bool expertMode = Main.expertMode;
			if (NPC.ai[0] == 489 || NPC.ai[0] == 690)
			{
				DustHelper.DrawStar(NPC.Center, 163, pointAmount: 163, mainSize: 2.7425f, dustDensity: 4, dustSize: .65f, pointDepthMult: 3.6f, noGravity: true);
				SoundEngine.PlaySound(SoundID.NPCDeath55 with { PitchVariance = 0.2f }, NPC.Center);
				float spread = 10f * 0.0174f;
				double startAngle = Math.Atan2(6, 6) - spread / 2;
				double deltaAngle = spread / 8f;
				for (int i = 0; i < 6; i++)
				{
					double offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
					int damage = expertMode ? 15 : 17;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(Math.Sin(offsetAngle) * 3f), (float)(Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<Yikes>(), damage, 0, player.whoAmI);
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(-Math.Sin(offsetAngle) * 3f), (float)(-Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<Yikes>(), damage, 0, player.whoAmI);
				}
			}
		}
		void DashAttack(Player player) //basically just copy pasted from scarabeus mostly
		{
			NPC.direction = Math.Sign(player.Center.X - NPC.Center.X);		
			if (NPC.ai[0] < 400 || NPC.ai[0] > 500 && NPC.ai[0] < 600 || NPC.ai[0] > 700 && NPC.ai[0] < 800) {
				NPC.ai[1] = 0;
				NPC.ai[2] = 0;
				Vector2 homeCenter = player.Center;
				NPC.spriteDirection = NPC.direction;
				homeCenter.X += (NPC.Center.X < player.Center.X) ? -280 : 280;
				homeCenter.Y -= 30;

				float vel = MathHelper.Clamp(NPC.Distance(homeCenter) / 12, 8, 30);
				NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(homeCenter) * vel, 0.08f);
			}
			else {
				NPC.rotation = NPC.velocity.X * 0.04f;
				if (NPC.ai[0] < 420 || NPC.ai[0] > 600 && NPC.ai[0] < 620 || NPC.ai[0] > 800 && NPC.ai[0] < 820) {
					NPC.velocity.X = -NPC.spriteDirection;
					NPC.velocity.Y = 0;
				}

				else if (NPC.ai[0] == 420 || NPC.ai[0] == 621 || NPC.ai[0] == 822) {
					SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
					NPC.velocity.X = MathHelper.Clamp(Math.Abs((player.Center.X - NPC.Center.X) / 10), 27, 40) * NPC.spriteDirection;
					NPC.netUpdate = true;
				}

				else if (NPC.direction != NPC.spriteDirection || NPC.ai[1] > 0) {
					NPC.ai[1]++; //ai 1 is used here to store this being triggered at least once, so if direction is equal to sprite direction again after this it will continue this part of the ai
					NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, 0, 0.06f);

					if (NPC.collideX && NPC.ai[2] == 0) {
						NPC.ai[2]++; //ai 2 is used here as a flag to make sure the tile collide effects only trigger once
						Collision.HitTiles(NPC.position, NPC.velocity, NPC.width, NPC.height);
						SoundEngine.PlaySound(SoundID.Dig, NPC.Center);
						NPC.velocity.X *= -0.5f;
					}
				}
			}
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => NPC.lifeMax = (int)(NPC.lifeMax * 0.85f * bossLifeScale);
		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.Poisoned, 200);

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient && NPC.life <= 0) {
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Grass, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.NextBool(2))
						Main.dust[num622].scale = 0.5f;

					int num623 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Grass, 0f, 0f, 100, default, 2f);
					Main.dust[num623].velocity *= 3f;
					if (Main.rand.NextBool(2))
						Main.dust[num623].scale = 0.5f;
				}

				for (int i = 0; i < 8; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("LeafGreen").Type, 1f);
			}
			for (int j = 0; j < 2; j++)
			{
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(Main.rand.Next(-50, 50), Main.rand.Next(-50, 50)), NPC.velocity, 386, goreScale);
				Main.gore[a].timeLeft = 15;
				Main.gore[a].rotation = 10f;
				Main.gore[a].velocity = new Vector2(hitDirection * 2.5f, Main.rand.NextFloat(1f, 2f));
				
				int a1 = Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(Main.rand.Next(-50, 50), Main.rand.Next(-50, 50)), NPC.velocity, 911, goreScale);
				Main.gore[a1].timeLeft = 15;
				Main.gore[a1].rotation = 1f;
				Main.gore[a1].velocity = new Vector2(hitDirection * 2.5f, Main.rand.NextFloat(10f, 20f));
			}
			for (int k = 0; k < 12; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Plantera_Green, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
		}


		public override bool PreKill()
		{
            if (!MyWorld.downedReachBoss)
                Main.NewText("The torrential downpour in the Briar has lifted!", 61, 255, 142);

            MyWorld.downedReachBoss = true;
			if(Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);

			NPC.PlayDeathSound("VinewrathDeathSound");
			return true;
		}

		Vector2 Drawoffset => new Vector2(0, NPC.gfxOffY) + Vector2.UnitX * NPC.spriteDirection * 12;

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
            float num395 = Main.mouseTextColor / 200f - 0.35f;
            num395 *= 0.2f;
            float num366 = num395 + .85f;
			if (NPC.ai[0] > 300 || NPC.life <= (NPC.lifeMax/2))
			{
				DrawAfterImage(Main.spriteBatch, new Vector2(0f, 0f), 0.75f, Color.Chartreuse * .7f, Color.PaleGreen * .05f, 0.75f, num366, .65f, screenPos);
			}
			var effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos + Drawoffset, NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color color, float opacity, float startScale, float endScale, Vector2 screenPos) => DrawAfterImage(spriteBatch, offset, trailLengthModifier, color, color, opacity, startScale, endScale, screenPos);
        
		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color startColor, Color endColor, float opacity, float startScale, float endScale, Vector2 screenPos)
        {
            SpriteEffects spriteEffects = (NPC.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int i = 1; i < 10; i++)
            {
                Color color = Color.Lerp(startColor, endColor, i / 10f) * opacity;
                spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/ReachBoss/ReachBoss1_Afterimage").Value, new Vector2(NPC.Center.X, NPC.Center.Y) + offset - screenPos + new Vector2(0, NPC.gfxOffY) - NPC.velocity * (float)i * trailLengthModifier, NPC.frame, color, NPC.rotation, NPC.frame.Size() * 0.5f, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
            }
        }

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
            float num107 = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
			float num1076 = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * 26.28318548f)) / 2f + 0.5f;
            float num106 = 0f;
			Color color1 = Color.White * num107 * .8f;
			Color color2 = Color.White * num1076;
			var effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/ReachBoss/ReachBoss1_Glow").Value, NPC.Center - screenPos + Drawoffset, NPC.frame, color1, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);

			SpriteEffects spriteEffects3 = (NPC.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Color color29 = new Color(127 - NPC.alpha, 127 - NPC.alpha, 127 - NPC.alpha, 0).MultiplyRGBA(Color.LawnGreen);

			const int Repeats = 4;
			for (int i = 0; i < Repeats; i++)
			{
				Color color28 = color29;
				color28 = NPC.GetAlpha(color28);
				color28 *= 1f - num107;
				Vector2 vector29 = NPC.Center + (i / (float)Repeats * 6.28318548f + NPC.rotation + num106).ToRotationVector2() * (7f * num107 + 2f) - screenPos + Drawoffset - NPC.velocity * (float)i;
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/ReachBoss/ReachBoss1_Glow").Value, vector29, NPC.frame, color28, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects3, 0f);
			}

			if (NPC.ai[0] > 400 && NPC.ai[0] < 500 || NPC.ai[0] > 600 && NPC.ai[0] < 700)
				spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/ReachBoss/ReachBoss1_Flash").Value, NPC.Center - screenPos + Drawoffset, NPC.frame, color2, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
		}

		public override void BossLoot(ref string name, ref int potionType) => potionType = ItemID.HealingPotion;

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddMasterModeCommonDrop<VinewrathRelicItem>();
			npcLoot.AddBossBag<ReachBossBag>();
			npcLoot.AddCommon<ReachMask>(7);
			npcLoot.AddCommon<Trophy5>(10);
			npcLoot.AddOneFromOptions<ThornBow, SunbeamStaff, ReachVineStaff, ReachBossSword>();
		}
	}
}
