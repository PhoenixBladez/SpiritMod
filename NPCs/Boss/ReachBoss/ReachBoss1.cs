using SpiritMod.Items.Sets.VinewrathDrops;
using Terraria;
using Terraria.Audio;
using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.ReachBoss
{
	[AutoloadBossHead]
	public class ReachBoss1 : ModNPC
	{
		//int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;
		//float HomeY = 150f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vinewrath Husk");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.Size = new Vector2(80, 120);
			npc.damage = 36;
			npc.lifeMax = 2050;
			npc.knockBackResist = 0;
			npc.boss = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.npcSlots = 20;
			npc.defense = 10;
			bossBag = ModContent.ItemType<ReachBossBag>();
			music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/ReachBoss");
			npc.buffImmune[20] = true;
			npc.buffImmune[31] = true;
			npc.buffImmune[70] = true;

			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.3f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.075f, 0.184f, 0.062f);			
			npc.spriteDirection = npc.direction;
			Player player = Main.player[npc.target];
			bool expertMode = Main.expertMode;			
			if (!player.active || player.dead) {
				npc.TargetClosest(false);
				npc.velocity.Y = -2000;
			}
			if (npc.life <= npc.lifeMax/2)
			{
				npc.ai[0]+= 1.5f;
			}
			else
			{
				npc.ai[0]++;
			}
			if (npc.ai[0] < 120 || npc.ai[0] > 180 && npc.ai[0] < 300)
			{
				generalMovement(player);
			}
			if (npc.ai[0] == 150)
			{
				Main.PlaySound(SoundID.Grass, (int)npc.position.X, (int)npc.position.Y);
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(SoundID.Item, 104).WithPitchVariance(0.2f), npc.Center);
			    DustHelper.DrawStar(npc.Center, 163, pointAmount: 163, mainSize: 2.7425f, dustDensity: 4, dustSize: .65f, pointDepthMult: 3.6f, noGravity: true);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    direction.Normalize();
                    direction.X *= 8f;
                    direction.Y *= 8f;

                    int amountOfProjectiles = 2;
					if (npc.life <= npc.lifeMax / 2)
					{
						amountOfProjectiles = 3;
					}
                    for (int i = 0; i < amountOfProjectiles; ++i)
                    {
                        float A = (float)Main.rand.Next(-200, 200) * 0.01f;
                        float B = (float)Main.rand.Next(-200, 200) * 0.01f;
                        int damage = expertMode ? 15 : 17;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<BouncingSpore>(), damage, 1, Main.myPlayer, 0, 0);
                    }
                }
			}
			float[] stoptimes = new float[] { 120, 300};
			if (stoptimes.Contains(npc.ai[0]))
			{
				npc.velocity = Vector2.Zero;
				npc.netUpdate = true;
			}
			if (npc.ai[0] >= 300 && npc.ai[0] < 900)
			{
				DashAttack(player);
			}
			if (npc.life <= (npc.lifeMax / 2)) {
				if (npc.ai[3] == 0) {
					npc.ai[0] = 0;
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), new Color(0, 200, 80, 100),
					"The Bramble shall consume you...");
					Main.PlaySound(SoundID.Grass, (int)npc.position.X, (int)npc.position.Y);
					Main.PlaySound(SoundID.Trackable, (int)npc.position.X, (int)npc.position.Y, 39);
                    npc.ai[3]++;
					npc.netUpdate = true;
                }
				if (npc.ai[0] > 460 && npc.ai[0] < 501 || npc.ai[0] > 661 && npc.ai[0] < 699)
				{
					CircleSpikeAttack(player);
				}
			}
			if (npc.ai[0] >= 900)
			{
				npc.ai[0] = 0;
				npc.ai[1] = 0;
				npc.ai[2] = 0;
				npc.netUpdate = true;				
			}			
		}
		public void generalMovement(Player player)
		{
			float value12 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) * 60f;
			if (npc.Center.X >= player.Center.X && moveSpeed >= -37) // flies to players x position
				moveSpeed--;
			else if (npc.Center.X <= player.Center.X && moveSpeed <= 37)
				moveSpeed++;

			npc.velocity.X = moveSpeed * 0.16f;
			npc.rotation = npc.velocity.X * 0.04f;

			if (npc.Center.Y >= player.Center.Y - 140f + value12 && moveSpeedY >= -20) //Flies to players Y position
				moveSpeedY--;
			else if (npc.Center.Y <= player.Center.Y - 140f + value12 && moveSpeedY <= 20)
				moveSpeedY++;
			npc.velocity.Y = moveSpeedY * 0.12f;			
		}
		public void CircleSpikeAttack(Player player)
		{
			bool expertMode = Main.expertMode;	
			if (npc.ai[0] == 489 || npc.ai[0] == 690)
			{
			    DustHelper.DrawStar(npc.Center, 163, pointAmount: 163, mainSize: 2.7425f, dustDensity: 4, dustSize: .65f, pointDepthMult: 3.6f, noGravity: true);
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(4, 55).WithPitchVariance(0.2f), npc.Center);				
				float spread = 10f * 0.0174f;
				double startAngle = Math.Atan2(6, 6) - spread / 2;
				double deltaAngle = spread / 8f;
				for (int i = 0; i < 6; i++) {
				double offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
                int damage = expertMode ? 15 : 17;
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Sin(offsetAngle) * 3f), (float)(Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<Yikes>(), damage, 0, player.whoAmI);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(-Math.Sin(offsetAngle) * 3f), (float)(-Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<Yikes>(), damage, 0, player.whoAmI);
				}				
			}
		}
		void DashAttack(Player player) //basically just copy pasted from scarabeus mostly
		{
			npc.direction = Math.Sign(player.Center.X - npc.Center.X);		
			if (npc.ai[0] < 400 || npc.ai[0] > 500 && npc.ai[0] < 600 || npc.ai[0] > 700 && npc.ai[0] < 800) {
				npc.ai[1] = 0;
				npc.ai[2] = 0;
				Vector2 homeCenter = player.Center;
				npc.spriteDirection = npc.direction;
				homeCenter.X += (npc.Center.X < player.Center.X) ? -280 : 280;
				homeCenter.Y -= 30;

				float vel = MathHelper.Clamp(npc.Distance(homeCenter) / 12, 8, 30);
				npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(homeCenter) * vel, 0.08f);
			}
			else {
				npc.rotation = npc.velocity.X * 0.04f;
				if (npc.ai[0] < 420 || npc.ai[0] > 600 && npc.ai[0] < 620 || npc.ai[0] > 800 && npc.ai[0] < 820) {
					npc.velocity.X = -npc.spriteDirection;
					npc.velocity.Y = 0;
				}

				else if (npc.ai[0] == 420 || npc.ai[0] == 621 || npc.ai[0] == 822) {
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(SoundID.Roar, 0), npc.Center);
					npc.velocity.X = MathHelper.Clamp(Math.Abs((player.Center.X - npc.Center.X) / 10), 27, 40) * npc.spriteDirection;
					npc.netUpdate = true;
				}

				else if (npc.direction != npc.spriteDirection || npc.ai[1] > 0) {
					npc.ai[1]++; //ai 1 is used here to store this being triggered at least once, so if direction is equal to sprite direction again after this it will continue this part of the ai
					npc.velocity.X = MathHelper.Lerp(npc.velocity.X, 0, 0.06f);

					if (npc.collideX && npc.ai[2] == 0) {
						npc.ai[2]++; //ai 2 is used here as a flag to make sure the tile collide effects only trigger once
						Collision.HitTiles(npc.position, npc.velocity, npc.width, npc.height);
						Main.PlaySound(SoundID.Dig, npc.Center);
						npc.velocity.X *= -0.5f;
						//put other things here for on tile collision effects
					}
				}
			}
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.85f * bossLifeScale);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Poisoned, 200);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient && npc.life <= 0) {
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Grass, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
						Main.dust[num622].scale = 0.5f;

					int num623 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Grass, 0f, 0f, 100, default, 2f);
					Main.dust[num623].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
						Main.dust[num623].scale = 0.5f;
				}
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LeafGreen"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LeafGreen"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LeafGreen"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LeafGreen"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LeafGreen"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LeafGreen"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LeafGreen"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LeafGreen"), 1f);
			}
			for (int j = 0; j < 2; j++)
			{
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(npc.Center + new Vector2(Main.rand.Next(-50, 50), Main.rand.Next(-50, 50)), npc.velocity, 386, goreScale);
				Main.gore[a].timeLeft = 15;
				Main.gore[a].rotation = 10f;
				Main.gore[a].velocity = new Vector2(hitDirection * 2.5f, Main.rand.NextFloat(1f, 2f));
				
				int a1 = Gore.NewGore(npc.Center + new Vector2(Main.rand.Next(-50, 50), Main.rand.Next(-50, 50)), npc.velocity, 911, goreScale);
				Main.gore[a1].timeLeft = 15;
				Main.gore[a1].rotation = 1f;
				Main.gore[a1].velocity = new Vector2(hitDirection * 2.5f, Main.rand.NextFloat(10f, 20f));
			}
			for (int k = 0; k < 12; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Plantera_Green, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
		}


		public override bool PreNPCLoot()
		{
            if (!MyWorld.downedReachBoss)
            {
                Main.NewText("The torrential downpour in the Briar has lifted!", 61, 255, 142, false);
            }
            MyWorld.downedReachBoss = true;
			if(Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);

			npc.PlayDeathSound("VinewrathDeathSound");
			return true;
		}
		Vector2 Drawoffset => new Vector2(0, npc.gfxOffY) + Vector2.UnitX * npc.spriteDirection * 12;
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
            float num395 = Main.mouseTextColor / 200f - 0.35f;
            num395 *= 0.2f;
            float num366 = num395 + .85f;
			if (npc.ai[0] > 300 || npc.life <= (npc.lifeMax/2))
			{
				DrawAfterImage(Main.spriteBatch, new Vector2(0f, 0f), 0.75f, Color.Chartreuse * .7f, Color.PaleGreen * .05f, 0.75f, num366, .65f);
			}
			var effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + Drawoffset, npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color color, float opacity, float startScale, float endScale) => DrawAfterImage(spriteBatch, offset, trailLengthModifier, color, color, opacity, startScale, endScale);
        public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color startColor, Color endColor, float opacity, float startScale, float endScale)
        {
            SpriteEffects spriteEffects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int i = 1; i < 10; i++)
            {
                Color color = Color.Lerp(startColor, endColor, i / 10f) * opacity;
                spriteBatch.Draw(mod.GetTexture("NPCs/Boss/ReachBoss/ReachBoss1_Afterimage"), new Vector2(npc.Center.X, npc.Center.Y) + offset - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)i * trailLengthModifier, npc.frame, color, npc.rotation, npc.frame.Size() * 0.5f, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
            }
        }
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			float num108 = 4;
            float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
			float num1076 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 26.28318548f)) / 2f + 0.5f;
            float num106 = 0f;
			Color color1 = Color.White * num107 * .8f;
			Color color2 = Color.White * num1076;
			var effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(
				mod.GetTexture("NPCs/Boss/ReachBoss/ReachBoss1_Glow"),
				npc.Center - Main.screenPosition + Drawoffset,
				npc.frame,
				color1,
				npc.rotation,
				npc.frame.Size() / 2,
				npc.scale,
				effects,
				0
			);
			SpriteEffects spriteEffects3 = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 vector33 = new Vector2(npc.Center.X, npc.Center.Y) - Main.screenPosition + Drawoffset - npc.velocity;
			Color color29 = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.LawnGreen);
			for (int num103 = 0; num103 < 4; num103++)
			{
				Color color28 = color29;
				color28 = npc.GetAlpha(color28);
				color28 *= 1f - num107;
				Vector2 vector29 = npc.Center + ((float)num103 / (float)num108 * 6.28318548f + npc.rotation + num106).ToRotationVector2() * (7f * num107 + 2f) - Main.screenPosition + Drawoffset - npc.velocity * (float)num103;
				Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/ReachBoss/ReachBoss1_Glow"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
			}
			if (npc.life <= npc.lifeMax/2)
			{	
				if (npc.ai[0] > 400 && npc.ai[0] < 500 || npc.ai[0] > 600 && npc.ai[0] < 700)
				{
					spriteBatch.Draw(
						mod.GetTexture("NPCs/Boss/ReachBoss/ReachBoss1_Flash"),
						npc.Center - Main.screenPosition + Drawoffset,
						npc.frame,
						color2,
						npc.rotation,
						npc.frame.Size() / 2,
						npc.scale,
						effects,
						0
					);	
				}
			}	
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.HealingPotion;
		}

		public override void NPCLoot()
		{
			if (Main.expertMode) {
				npc.DropBossBags();
				return;
			}

			int[] lootTable = {
				ModContent.ItemType<ThornBow>(),
				ModContent.ItemType<SunbeamStaff>(),
				ModContent.ItemType<ReachVineStaff>(),
				ModContent.ItemType<ReachBossSword>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			npc.DropItem(lootTable[loot]);

			npc.DropItem(ModContent.ItemType<ReachMask>(), 1f / 7);
			npc.DropItem(ModContent.ItemType<Trophy5>(), 1f / 10);
		}
	}
}
