using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Tiles.Block;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Spirit
{
	public class WanderingSoul : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wandering Soul");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Wraith];
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 48;
			npc.damage = 37;
			npc.defense = 40;
			npc.lifeMax = 540;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 60f;
			npc.knockBackResist = .60f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.aiStyle = 22;
			aiType = NPCID.Wraith;
			aiType = NPCID.Wraith;
			animationType = NPCID.Wraith;
			npc.stepSpeed = .5f;
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
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Spirit/WanderingSoul_Glow"));
		}
		private static int[] SpawnTiles = { };
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if(SpawnTiles.Length == 0) {
				int[] Tiles = { ModContent.TileType<SpiritDirt>(), ModContent.TileType<SpiritStone>(), ModContent.TileType<SpiritGrass>(), ModContent.TileType<SpiritIce>() };
				SpawnTiles = Tiles;
			}
			return SpawnTiles.Contains(Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].type) && !spawnInfo.playerSafe && !spawnInfo.invasion && NPC.downedMechBossAny ? 5f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if(npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, 13);
				Gore.NewGore(npc.position, npc.velocity, 12);
				Gore.NewGore(npc.position, npc.velocity, 11);
			}
		}

		public override void NPCLoot()
		{
			if(Main.rand.Next(3) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Material.Rune>());
		}

	}
}