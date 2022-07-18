using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Tiles.Block;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Spirit
{
	public class SpiritGhoul : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Ghoul");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.DesertGhoul];
		}

		public override void SetDefaults()
		{
			NPC.width = 34;
			NPC.height = 48;
			NPC.damage = 55;
			NPC.defense = 38;
			NPC.lifeMax = 220;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 360f;
			NPC.knockBackResist = .16f;
			NPC.aiStyle = 3;
			AIType = NPCID.DesertGhoul;
			AnimationType = NPCID.DesertGhoul;
			SpawnModBiomes = new int[1] { ModContent.GetInstance<Biomes.SpiritUndergroundBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				new FlavorTextBestiaryInfoElement("Ghouls reanimated by the Spirit now leak ectoplasm from every crevice. Their bites have a chance of confusing their foes by briefly showing them glimpses of their former lives."),
			});
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Spirit/SpiritGhoul_Glow").Value, screenPos);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.Player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.SpawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.SpawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0)) {
				int[] TileArray2 = { ModContent.TileType<Spiritsand>(), };
				return TileArray2.Contains(Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType) && NPC.downedMechBossAny && spawnInfo.SpawnTileY > Main.rockLayer ? 4f : 0f;
			}
			return 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient && NPC.life <= 0) {
				NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.UnusedWhiteBluePurple, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}

			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(12) == 1) {
				target.AddBuff(BuffID.Cursed, 150);
			}
		}

		public override void AI()
		{
			Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), 0f, 0.675f, 2.50f);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon<Items.Sets.RunicSet.Rune>();
	}
}