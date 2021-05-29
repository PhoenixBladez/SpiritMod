using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.DeadeyeMarksman
{
	public class DeadArcher : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deadeye Marksman");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.GoblinArcher];
		}

		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 46;
			npc.damage = 0;
			npc.defense = 9;
			npc.lifeMax = 47;
			if (Main.expertMode)
                npc.lifeMax = 94;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 120f;
			npc.knockBackResist = .30f;
			npc.aiStyle = 3;
			aiType = NPCID.GoblinArcher;
			animationType = NPCID.GoblinArcher;
            banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.DeadeyeMarksmanBanner>();
        }

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<OldLeather>());
			if (Main.rand.NextBool(100))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.BlackLens);
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.WoodenArrow, Main.rand.Next(5, 10));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			bool conditions = spawnInfo.spawnTileY < Main.rockLayer && !Main.dayTime && !spawnInfo.playerSafe && spawnInfo.player.ZoneOverworldHeight && !spawnInfo.player.ZoneDesert && !spawnInfo.player.ZoneCorrupt && !spawnInfo.player.ZoneCrimson && !spawnInfo.player.ZoneBeach && !spawnInfo.player.ZoneJungle && !Main.pumpkinMoon && !Main.snowMoon;
			if (!NPC.downedBoss1)
				return conditions ? 0.025f : 0f;
			if (Main.hardMode)
				return conditions ? 0.01f : 0f;
			return conditions ? 0.04f : 0f;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => npc.lifeMax = (int)(npc.lifeMax * bossLifeScale);

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 40; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, 0, default(Color), .45f);
			}
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Archer2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Archer1"), 1f);
				for (int k = 0; k < 80; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, 0, default(Color), .85f);
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/DeadeyeMarksman/DeadArcher_Glow"));

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.Darkness, 180);
		}
	}
}
