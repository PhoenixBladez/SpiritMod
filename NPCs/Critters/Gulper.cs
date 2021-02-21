using SpiritMod.Items.Consumable;
using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace SpiritMod.NPCs.Critters
{
	public class Gulper : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gulper");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 16;
			npc.damage = 0;
			npc.defense = 0;
			npc.dontCountMe = true;
			npc.lifeMax = 5;
			npc.HitSound = SoundID.NPCHit1;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ModContent.ItemType<GulperItem>();
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = .35f;
			npc.aiStyle = 16;
			npc.noGravity = true;
			npc.npcSlots = 0;
			aiType = NPCID.Goldfish;
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gulper/Gulper1"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gulper/Gulper2"));
			}
		}
		private int Counter;
		public override void AI()
		{
			npc.spriteDirection = -npc.direction;
			Counter++;
			if (Counter == 100) {
				npc.velocity.Y *= 10.0f;
				npc.velocity.X *= 4.0f;
			}
			if (Counter >= 200) {
				Counter = 0;
			}
            Player player = Main.player[npc.target];
            {
                Player target = Main.player[npc.target];
                int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
                if (distance < 65 && target.wet && npc.wet)
                {
                    Vector2 vel = npc.DirectionFrom(target.Center);
                    vel.Normalize();
                    vel *= 4.5f;
                    npc.velocity = vel;
                    npc.rotation = npc.velocity.X * .06f;
                    if (target.position.X > npc.position.X)
                    {
                        npc.spriteDirection = -1;
                        npc.direction = -1;
                        npc.netUpdate = true;
                    }
                    else if (target.position.X < npc.position.X)
                    {
                        npc.spriteDirection = 1;
                        npc.direction = 1;
                        npc.netUpdate = true;
                    }
                }
            }
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe) {
				return 0f;
			}
			return SpawnCondition.OceanMonster.Chance * 0.31f;
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RawFish>(), 1);
			}
		}
	}
}
