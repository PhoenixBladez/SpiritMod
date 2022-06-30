using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.SpiritBiomeDrops;
using SpiritMod.Tiles.Block;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Spirit
{
	public class HauntedBook : ModNPC
	{
		int timer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Tome");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 48;
			NPC.height = 40;
			NPC.damage = 45;
			NPC.defense = 12;
			NPC.lifeMax = 410;
			NPC.HitSound = SoundID.NPCHit3;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 3060f;
			NPC.knockBackResist = .45f;
			NPC.aiStyle = -1;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
		}
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.Player;
            if (!player.GetSpiritPlayer().ZoneSpirit)
            {
                return 0f;
            }
            if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.SpawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.SpawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0)) {
				int[] TileArray2 = { ModContent.TileType<SpiritDirt>(), ModContent.TileType<SpiritStone>(), ModContent.TileType<Spiritsand>(), ModContent.TileType<SpiritGrass>(), ModContent.TileType<SpiritIce>(), };
				return TileArray2.Contains(Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType) && NPC.downedMechBossAny && player.ZoneOverworldHeight ? 2.09f : 0f;
			}
			return 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 13);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 12);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 11);
			}
		}

		public override void AI()
		{
			if (Main.rand.Next(150) == 8) {
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				direction.X *= 2f;
				direction.Y *= 2f;

				int amountOfProjectiles = Main.rand.Next(1, 2);
				for (int i = 0; i < amountOfProjectiles; ++i) {
					float A = (float)Main.rand.Next(-1, 1) * 0.03f;
					float B = (float)Main.rand.Next(-1, 1) * 0.03f;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<RuneHostile>(), 38, 1, Main.myPlayer, 0, 0);
				}
			}
			NPC.spriteDirection = NPC.direction;
			Lighting.AddLight((int)((NPC.position.X + (NPC.width / 2)) / 16f), (int)((NPC.position.Y + (NPC.height / 2)) / 16f), 0f, 0.675f, 2.50f);
			timer++;
			NPC.TargetClosest(true);
			if (timer == 25) {
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				NPC.velocity.Y = direction.Y * 3f;
				NPC.velocity.X = direction.X * 3f;
				timer = 0;
			}
			if (timer == 32) {
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				NPC.velocity.Y = direction.Y * 3f;
				NPC.velocity.X = direction.X * 3f;
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon(ModContent.ItemType<Items.Sets.RunicSet.Rune>(), 2, 1, 2);
			npcLoot.AddCommon(ModContent.ItemType<PossessedBook>(), 20);
		}
	}
}
