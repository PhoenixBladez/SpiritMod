using Microsoft.Xna.Framework;
using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.NPCs.Critters
{
	public class GoldenCarp : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golden Carp");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 26;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ItemID.GoldenCarp;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = .35f;
			npc.aiStyle = 16;
			npc.rarity = 3;
			npc.noGravity = true;
			npc.dontCountMe = true;
			npc.npcSlots = 0;
			aiType = NPCID.Goldfish;
			npc.dontTakeDamageFromHostiles = false;
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
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.GoldCoin, 0f, 0f, 100, default, 1.4f);
					Main.dust[num622].velocity *= 1f;
					Main.dust[num622].noGravity = true;
					{
						Main.dust[num622].scale = 0.23f;
					}
				}
			}
		}
		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), .1f, .1f, .1f);
			npc.spriteDirection = npc.direction;
			int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldCoin);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity *= 0f;
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
		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RawFish>(), 1);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneRockLayerHeight && spawnInfo.water ? 0.007666f : 0f;
		}

	}
}
