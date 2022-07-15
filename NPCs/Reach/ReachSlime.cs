using Microsoft.Xna.Framework;
using SpiritMod.Biomes;
using SpiritMod.Items.Sets.HuskstalkSet;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Reach
{
	public class ReachSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Briarthorn Slime");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BlueSlime];
		}

		public override void SetDefaults()
		{
			NPC.width = 64;
			NPC.height = 24;
			NPC.damage = 12;
			NPC.defense = 4;
			NPC.lifeMax = 46;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 30f;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.alpha = 60;
			NPC.knockBackResist = .25f;
			NPC.aiStyle = 1;
			AIType = NPCID.BlueSlime;
			AnimationType = NPCID.BlueSlime;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.BriarthornSlimeBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<BriarSurfaceBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				new FlavorTextBestiaryInfoElement("Due to the perpetual rainfall in the Briar, these slimes are composed of 90% water. As a result, they often leak and lose volume when struck."),
			});
		}

		public override void AI()
		{
			NPC.spriteDirection = -NPC.direction;
			NPC.scale = (.2f * (float)(NPC.life / NPC.lifeMax)) + .8f;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.Player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.SpawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.SpawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0))
				return spawnInfo.Player.GetSpiritPlayer().ZoneReach && Main.dayTime ? 2.1f : 0f;
			return 0f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon(ItemID.Gel, 1, 1, 3);
			npcLoot.AddCommon<AncientBark>(2, 1, 3);
			npcLoot.AddCommon(ItemID.SlimeStaff, 10000);
			npcLoot.AddCommon(ItemID.Bezoar, 90);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.Poisoned, 180);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 12; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SlimeBunny, 2.5f * hitDirection, -2.5f, 0, Color.Green, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SlimeBunny, 2.5f * hitDirection, -2.5f, 0, Color.Green, 0.7f);
			}
		}
	}
}