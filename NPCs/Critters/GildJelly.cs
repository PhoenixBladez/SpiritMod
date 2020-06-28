using Microsoft.Xna.Framework;
using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Critters
{
	public class GildJelly : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Jelly");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 14;
			npc.height = 18;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.HitSound = SoundID.NPCHit25;
			npc.DeathSound = SoundID.NPCDeath28;
			npc.knockBackResist = .35f;
			npc.dontCountMe = true;
			npc.aiStyle = 18;
			npc.noGravity = true;
			npc.npcSlots = 0;
			aiType = NPCID.PinkJellyfish;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if(spawnInfo.playerSafe) {
				return 0f;
			}
			return SpawnCondition.OceanMonster.Chance * 0.008f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if(npc.life <= 0) {
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for(int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.GoldCoin, 0f, 0f, 100, default(Color), 1.4f);
					Main.dust[num622].velocity *= 1f;
					Main.dust[num622].noGravity = true;
					if(Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.23f;
					}
				}
			}
		}
		public override bool PreAI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), .2f, .2f, .2f);
			return true;
		}


		public override void NPCLoot()
		{
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RawFish>(), 1);
			}
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldCoin, 5);
			}
		}
	}
}
