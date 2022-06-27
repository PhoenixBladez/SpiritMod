using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.DeadeyeMarksman
{
	public class DeadArcher : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deadeye Marksman");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.GoblinArcher];
		}

		public override void SetDefaults()
		{
			NPC.width = 36;
			NPC.height = 46;
			NPC.damage = 0;
			NPC.defense = 9;
			NPC.lifeMax = 47;
			if (Main.expertMode)
                NPC.lifeMax = 94;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 120f;
			NPC.knockBackResist = .30f;
			NPC.aiStyle = 3;
			AIType = NPCID.GoblinArcher;
			AnimationType = NPCID.GoblinArcher;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.DeadeyeMarksmanBanner>();
        }

		public override void OnKill()
		{
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<OldLeather>());
			if (Main.rand.NextBool(100))
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.BlackLens);
            Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.WoodenArrow, Main.rand.Next(5, 10));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			bool conditions = spawnInfo.SpawnTileY < Main.rockLayer && !Main.dayTime && !spawnInfo.PlayerSafe && spawnInfo.Player.ZoneOverworldHeight && !spawnInfo.Player.ZoneDesert && !spawnInfo.Player.ZoneCorrupt && !spawnInfo.Player.ZoneCrimson && !spawnInfo.Player.ZoneBeach && !spawnInfo.Player.ZoneJungle && !Main.pumpkinMoon && !Main.snowMoon;
			if (!NPC.downedBoss1)
				return conditions ? 0.025f : 0f;
			if (Main.hardMode)
				return conditions ? 0.01f : 0f;
			return conditions ? 0.04f : 0f;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => NPC.lifeMax = (int)(NPC.lifeMax * bossLifeScale);

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 40; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hitDirection, -1f, 0, default, .45f);
			}
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Archer2").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Archer1").Type, 1f);
				for (int k = 0; k < 80; k++) {
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hitDirection, -1f, 0, default, .85f);
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.GetTexture("NPCs/DeadeyeMarksman/DeadArcher_Glow"));

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.Darkness, 180);
		}
	}
}
