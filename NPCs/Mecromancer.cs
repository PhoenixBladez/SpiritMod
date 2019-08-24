using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class Mecromancer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mechromancer");
			Main.npcFrameCount[npc.type] = 17;
		}

		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 56;
			npc.damage = 16;
			npc.defense = 8;
			npc.lifeMax = 75;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 6760f;
			npc.knockBackResist = 0.1f;
			npc.aiStyle = 3;
			aiType = NPCID.AngryBones;
			animationType = 471;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (!NPC.downedBoss2)
			{
				return 0f;
			}
			return SpawnCondition.GoblinArmy.Chance * 0.09f;
		}

		public override void NPCLoot()
		{
			int Techs = Main.rand.Next(2, 5);
			for (int J = 0; J <= Techs; J++)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TechDrive"));
			}
		}

		public override void AI()
		{
			if (Main.rand.Next(250) == 2)
			{
				npc.TargetClosest();
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				float ai = Main.rand.Next(100);
				direction.Normalize();
				int MechBat = Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -6, mod.ProjectileType("MechBat"), 11, 0);
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mech1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mech2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mech2"), 1f);
			}
		}
	}
}
