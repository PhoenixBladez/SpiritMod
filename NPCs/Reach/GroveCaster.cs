using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Reach
{
	public class GroveCaster : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wildwood Shaman");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 54;

			npc.lifeMax = 54;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.defense = 6;
			npc.damage = 19;

			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath2;

			npc.value = 300f;
			npc.knockBackResist = 0.75f;
			npc.noGravity = true;
			npc.netAlways = true;
			npc.chaseable = false;
			npc.lavaImmune = true;
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<EnchantedLeaf>());
		}

		public override bool PreAI()
		{
			if(npc.localAI[0] == 0f) {
				npc.localAI[0] = npc.Center.Y;
				npc.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
			}
			if(npc.Center.Y >= npc.localAI[0]) {
				npc.localAI[1] = -1f;
				npc.netUpdate = true;
			}
			if(npc.Center.Y <= npc.localAI[0] - 2f) {
				npc.localAI[1] = 1f;
				npc.netUpdate = true;
			}
			npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y + 0.009f * npc.localAI[1], -.5f, .5f);
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.46f, 0.32f, .1f);

			npc.TargetClosest(true);
			npc.velocity.X = npc.velocity.X * 0.93f;
			if(npc.velocity.X > -0.1F && npc.velocity.X < 0.1F)
				npc.velocity.X = 0;
			if(npc.ai[0] == 0)
				npc.ai[0] = 500f;

			if(npc.ai[2] != 0 && npc.ai[3] != 0) {
				// Teleport effects: away.
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);
				for(int index1 = 0; index1 < 50; ++index1) {
					int newDust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 228, 0.0f, 0.0f, 100, new Color(), 1.5f);
					Main.dust[newDust].velocity *= 3f;
					Main.dust[newDust].noGravity = true;
				}
				npc.position.X = (npc.ai[2] * 16 - (npc.width / 2) + 8);
				npc.position.Y = npc.ai[3] * 16f - npc.height;
				npc.velocity.X = 0.0f;
				npc.velocity.Y = 0.0f;
				npc.ai[2] = 0.0f;
				npc.ai[3] = 0.0f;
				// Teleport effects: arrived.
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);
				for(int index1 = 0; index1 < 50; ++index1) {
					int newDust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 228, 0.0f, 0.0f, 100, new Color(), 1.5f);
					Main.dust[newDust].velocity *= 3f;
					Main.dust[newDust].noGravity = true;
				}
			}

			++npc.ai[0];

			if(npc.ai[0] == 100 || npc.ai[0] == 200 || npc.ai[0] == 300) {
				npc.ai[1] = 30f;
				npc.netUpdate = true;
			}

			bool teleport = false;

			// Teleport
			if(npc.ai[0] >= 500 && Main.netMode != NetmodeID.MultiplayerClient) {
				teleport = true;
			}

			if(teleport) {
				Teleport();
				npc.ai[1] = 200f;
			}


			if(npc.ai[1] > 0) {
				--npc.ai[1];
				if(npc.ai[1] == 15) {
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);
					if(Main.netMode != NetmodeID.MultiplayerClient) {
						NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.Center.Y - 16, ModContent.NPCType<GrassBall>(), 0, 0, 0, 0, 0, 255);
					}
				}
			}

			if(Main.rand.Next(3) == 0)
				return false;
			Dust dust = Main.dust[Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 228, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, new Color(), 0.9f)];
			dust.noGravity = true;
			dust.velocity.X = dust.velocity.X * 0.3f;
			dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;

			return false;
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
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Reach/GroveCaster_Glow"));
		}
		public void Teleport()
		{
			npc.ai[0] = 1f;
			int num1 = (int)Main.player[npc.target].position.X / 16;
			int num2 = (int)Main.player[npc.target].position.Y / 16;
			int num3 = (int)npc.position.X / 16;
			int num4 = (int)npc.position.Y / 16;
			int num5 = 20;
			int num6 = 0;
			bool flag1 = false;
			if(Math.Abs(npc.position.X - Main.player[npc.target].position.X) + Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000.0) {
				num6 = 100;
				flag1 = true;
			}
			while(!flag1 && num6 < 100) {
				++num6;
				int index1 = Main.rand.Next(num1 - num5, num1 + num5);
				for(int index2 = Main.rand.Next(num2 - num5, num2 + num5); index2 < num2 + num5; ++index2) {
					if((index2 < num2 - 4 || index2 > num2 + 4 || (index1 < num1 - 4 || index1 > num1 + 4)) && (index2 < num4 - 1 || index2 > num4 + 1 || (index1 < num3 - 1 || index1 > num3 + 1)) && Main.tile[index1, index2].nactive()) {
						bool flag2 = true;
						if(Main.tile[index1, index2 - 1].lava())
							flag2 = false;
						if(flag2 && Main.tileSolid[(int)Main.tile[index1, index2].type] && !Collision.SolidTiles(index1 - 1, index1 + 1, index2 - 4, index2 - 1)) {
							npc.ai[1] = 20f;
							npc.ai[2] = (float)index1;
							npc.ai[3] = (float)index2;
							flag1 = true;
							break;
						}
					}
				}
			}
			npc.netUpdate = true;
		}

		public override void FindFrame(int frameHeight)
		{
			int currShootFrame = (int)npc.ai[1];
			if(currShootFrame >= 25)
				npc.frame.Y = frameHeight;
			else if(currShootFrame >= 20)
				npc.frame.Y = frameHeight * 2;
			else if(currShootFrame >= 15)
				npc.frame.Y = frameHeight * 3;
			else if(currShootFrame >= 10)
				npc.frame.Y = frameHeight * 2;
			else if(currShootFrame >= 5)
				npc.frame.Y = frameHeight;
			else
				npc.frame.Y = 0;

			npc.spriteDirection = npc.direction;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if(!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0)) {
				return spawnInfo.player.GetSpiritPlayer().ZoneReach && !Main.dayTime ? 0.3f : 0f;
			}
			if(!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0)) {
				return spawnInfo.player.GetSpiritPlayer().ZoneReach && spawnInfo.spawnTileY > Main.rockLayer ? 0.3f : 0f;
			}
			return 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 3;
			int d1 = 6;
			for(int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
			}
			if(npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Reach1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Reach2"), 1f);
			}
		}
	}
}