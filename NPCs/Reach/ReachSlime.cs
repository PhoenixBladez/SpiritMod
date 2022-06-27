using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.HuskstalkSet;
using Terraria;
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
		public override void OnKill()
		{
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Gel, Main.rand.Next(1, 4));

			if (Main.rand.Next(2) == 0)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<AncientBark>(), Main.rand.Next(1, 4));

			if (Main.rand.Next(10000) == 0)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.SlimeStaff);

			if (Main.rand.NextBool(90))
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Bezoar);
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