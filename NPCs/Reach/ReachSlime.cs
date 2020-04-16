using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Reach
{
	public class ReachSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Briarthorn Slime");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
		}

		public override void SetDefaults()
		{
			npc.width = 64;
			npc.height = 24;
			npc.damage = 12;
			npc.defense = 4;
			npc.lifeMax = 46;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 30f;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.alpha = 60;
            npc.knockBackResist = .25f;
            npc.aiStyle = 1;
            aiType = NPCID.BlueSlime;
            animationType = NPCID.BlueSlime;    
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0))
			{
				return spawnInfo.player.GetSpiritPlayer().ZoneReach && Main.dayTime? 2.1f : 0f;
			}
			return 0f;
		}
        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Gel, Main.rand.Next(1, 4));
            if (Main.rand.Next(2) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AncientBark"), Main.rand.Next(1, 4));
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(3) == 0)
            {
                target.AddBuff(BuffID.Poisoned, 180);
            }
        }
        public override void AI()
        {
            npc.spriteDirection = -npc.direction;
        }
        public override void HitEffect(int hitDirection, double damage)
		{
            int d = 193;
            for (int k = 0; k < 12; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.Green, 0.7f);
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.Green, 0.7f);
            }
            npc.scale -= .03f;
            npc.velocity.X += .05f;
		}
	}
}
