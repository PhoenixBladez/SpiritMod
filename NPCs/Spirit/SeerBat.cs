using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
namespace SpiritMod.NPCs.Spirit
{
	public class SeerBat : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Bat");
			Main.npcFrameCount[npc.type] = 3;
		}

		public override void SetDefaults()
		{
			npc.width = 51;
			npc.height = 28;
			npc.damage = 44;
			npc.defense = 19;
			npc.lifeMax = 240;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath4;
			npc.value = 60f;
			npc.knockBackResist = .45f;
			npc.aiStyle = 14;
			aiType = NPCID.CaveBat;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0f, 0.135f, 0.75f);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0))
			{
				int[] TileArray2 = { ModContent.TileType<SpiritDirt>(), ModContent.TileType<SpiritStone>(), ModContent.TileType<Spiritsand>(), ModContent.TileType<SpiritIce>(), };
				return TileArray2.Contains(Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].type) && NPC.downedMechBossAny && spawnInfo.spawnTileY > (Main.rockLayer) ? 6.09f : 0f;
			}
			return 0f;
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
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Spirit/SeerBat_Glow"));
        }
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, 13);
				Gore.NewGore(npc.position, npc.velocity, 12);
				Gore.NewGore(npc.position, npc.velocity, 11);
			}
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<SpiritOre>(), Main.rand.Next(2) + 1);

			if (Main.rand.Next(3) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<SoulShred>(), Main.rand.Next(1) + 1);

			if (Main.rand.Next(2) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<SpiritCrystal>(), Main.rand.Next(1) + 1);
		}

	}
}
