using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.ChainedSinner
{
	internal class ChainedSinnerBase : ModNPC
	{
		public override bool IsLoadingEnabled(Mod mod) => false;
		public override void SetStaticDefaults() => DisplayName.SetDefault("Chained Sinner");

		public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 24;
			NPC.knockBackResist = 0;
			NPC.aiStyle = -1;
			NPC.lifeMax = 50;
			NPC.damage = 10;
			NPC.defense = 4;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.dontTakeDamage = true;
		}

		private bool spawnedHead = false;
		public override void AI()
		{
			NPC.velocity = Vector2.Zero;
			if (!spawnedHead && Main.netMode != NetmodeID.MultiplayerClient)
			{
				spawnedHead = true;
				int child = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<ChainedSinner>());
				if (Main.npc[child].ModNPC is ChainedSinner head)
				{
					head.parentid = NPC.whoAmI;
					head.InitializeChain(NPC.Center);
				}
				NPC.netUpdate = true;
			}
		}

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(spawnedHead);
		public override void ReceiveExtraAI(BinaryReader reader) => spawnedHead = reader.ReadBoolean();
	}
}