using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Alien
{
	public class Alien : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alien");
			Main.npcFrameCount[NPC.type] = 8;
		}

		public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 44;
			NPC.damage = 70;
			NPC.defense = 30;
			NPC.lifeMax = 600;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.WeaponImbueVenom] = true;
			NPC.HitSound = SoundID.NPCHit6;
			NPC.DeathSound = SoundID.NPCDeath8;
			NPC.value = 10000f;
			NPC.knockBackResist = .25f;
			NPC.aiStyle = 26;
			AIType = NPCID.Unicorn;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => NPC.downedMechBossAny && Main.eclipse && spawnInfo.Player.ZoneOverworldHeight ? 0.07f : 0;

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Alien1").Type, 1f);
				for (int i = 0; i < 4; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Alien2").Type, 1f);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.40f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void AI() => NPC.spriteDirection = NPC.direction;

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(4) == 1) {
				target.AddBuff(BuffID.Venom, 260);
			}
		}
	}
}
