using SpiritMod.Items.Consumable;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SpiritMod.NPCs.Critters
{
	public class Luvdisc : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ardorfish");
			Main.npcFrameCount[NPC.type] = 1;
		}

		public override void SetDefaults()
		{
			NPC.width = 30;
			NPC.height = 24;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			Main.npcCatchable[NPC.type] = true;
			NPC.catchItem = (short)ModContent.ItemType<LuvdiscItem>();

			NPC.knockBackResist = .35f;
			NPC.aiStyle = 16;
			NPC.noGravity = true;
			NPC.npcSlots = 0;
			NPC.dontCountMe = true;
			AIType = NPCID.Goldfish;
			NPC.dontTakeDamageFromHostiles = false;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Luvdisc").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Luvdisc1").Type, 1f);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon<HeartScale>(2);

		public override void AI()
		{
			NPC.spriteDirection = -NPC.direction;
            Player player = Main.player[NPC.target];
            {
                Player target = Main.player[NPC.target];
                int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));
                if (distance < 65 && target.wet && NPC.wet)
                {
                    Vector2 vel = NPC.DirectionFrom(target.Center);
                    vel.Normalize();
                    vel *= 4.5f;
                    NPC.velocity = vel;
                    NPC.rotation = NPC.velocity.X * .06f;
                    if (target.position.X > NPC.position.X)
                    {
                        NPC.spriteDirection = -1;
                        NPC.direction = -1;
                        NPC.netUpdate = true;
                    }
                    else if (target.position.X < NPC.position.X)
                    {
                        NPC.spriteDirection = 1;
                        NPC.direction = 1;
                        NPC.netUpdate = true;
                    }
                }
            }
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.Player.ZoneRockLayerHeight && spawnInfo.Water ? 0.2f : 0f;
		}

	}
}
