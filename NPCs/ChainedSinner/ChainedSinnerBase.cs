using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.ChainedSinner
{
	internal class ChainedSinnerBase : ModNPC
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Chained Sinner");

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 24;
			npc.knockBackResist = 0;
			npc.aiStyle = -1;
			npc.lifeMax = 50;
			npc.damage = 10;
			npc.defense = 4;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.dontTakeDamage = true;
		}

		private bool spawnedHead = false;
		public override void AI()
		{
			npc.velocity = Vector2.Zero;
			if (!spawnedHead && Main.netMode != NetmodeID.MultiplayerClient)
			{
				spawnedHead = true;
				int child = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<ChainedSinner>());
				if (Main.npc[child].modNPC is ChainedSinner head)
				{
					head.parentid = npc.whoAmI;
					head.InitializeChain(npc.Center);
				}
				npc.netUpdate = true;
			}
		}

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(spawnedHead);

		public override void ReceiveExtraAI(BinaryReader reader) => spawnedHead = reader.ReadBoolean();
	}
}