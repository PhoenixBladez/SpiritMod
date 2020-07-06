using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Reach
{
	public class BlossomHound : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blossom Hound");
			Main.npcFrameCount[npc.type] = 7;
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 86;
			npc.height = 54;
			npc.damage = 28;
			npc.defense = 7;
			npc.lifeMax = 85;
			npc.HitSound = SoundID.NPCHit6;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.value = 180f;
			npc.knockBackResist = .2f;
			npc.aiStyle = 3;
			aiType = NPCID.WalkingAntlion;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if(!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0)) {
				return spawnInfo.player.GetSpiritPlayer().ZoneReach ? 0.4f : 0f;
			}
			return 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for(int k = 0; k < 40; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection * 2.5f, -1f, 0, default(Color), Main.rand.NextFloat(.45f, 1.15f));
			}
			if(npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlossomHound1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlossomHound2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlossomHound3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlossomHound4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlossomHound5"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlossomHound6"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlossomHound7"), 1f);
				for(int k = 0; k < 40; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection * 2.5f, -1f, 0, default(Color), Main.rand.NextFloat(.45f, 1.15f));
				}
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			if(trailbehind) {
				Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
				for(int k = 0; k < npc.oldPos.Length; k++) {
					Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
					Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += num34616;
			npc.frameCounter %= 6;
			int frame = (int)npc.frameCounter;
			npc.frame.Y = (frame + 1) * frameHeight;
		}
		int timer;
		bool trailbehind = false;
		float num34616;
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			timer++;
			if(timer == 400 && Main.netMode != NetmodeID.MultiplayerClient) {
				Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 7);
				npc.netUpdate = true;
			}
			if(timer == 400 && Main.netMode != NetmodeID.MultiplayerClient) {
				num34616 = .35f;
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction.X = direction.X * Main.rand.Next(5, 7);
				direction.Y = direction.Y * Main.rand.Next(11, 15);
				npc.velocity.X = direction.X;
				npc.velocity.Y = direction.Y;
				npc.velocity.X *= 0.995f;
				npc.netUpdate = true;
				trailbehind = true;
				npc.knockBackResist = 0f;
			} else {
				num34616 = .15f;
			}
			if(timer >= 551) {
				timer = 0;
				npc.netUpdate = true;
				trailbehind = false;
				npc.knockBackResist = .2f;
			}
		}
	}
}
