
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Dungeon
{
	public class PossessedPrefix : GlobalNPC
	{
		public override bool InstancePerEntity {
			get {
				return true;
			}
		}
		public string elementalType = "";

		public bool nameConfirmed = false;
		public bool MPSynced = false;
		public bool readyForChecks = false;

		public override void SetDefaults(NPC npc)
		{
			npc.GivenName = npc.FullName;
			if(Main.netMode == NetmodeID.Server || npc == null || npc.FullName == null)//if multiplayer, but not server. 1 is client in MP, 2 is server. Prefixes are sent to client by server in MP.
			{
				return;
			}
			Player player = Main.LocalPlayer;
			if(player.ZoneDungeon && !NPC.downedPlantBoss) {
				if(npc.aiStyle != 7 && !(npc.catchItem > 0) && ((npc.aiStyle != 6 && npc.aiStyle != 37)) && npc.type != 401 && npc.type != 488 && npc.type != 371 && npc.lifeMax > 1 && !(npc.aiStyle == 0 && npc.value == 0 && npc.npcSlots == 1)) {
					if(Main.rand.Next(0, Main.expertMode ? 44 : 50) == 0 && (npc.value != 0 || (npc.type >= 402 && npc.type <= 429)) && npc.type != 239 && npc.type != 240 && npc.type != 469 && npc.type != 238 && npc.type != 237 && npc.type != 236 && npc.type != 164 && npc.type != 165 && npc.type != 163) {
						npc.GivenName = "Possessed " + npc.GivenName;
						elementalType += "Possessed #";
					}
				}
			}
		}
		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if(elementalType.Contains("Possessed ")) {
				if(Main.rand.Next(2) == 0) {
					for(int k = 0; k < 4; k++) {
						Dust dust;
						Vector2 position = npc.position;
						dust = Main.dust[Terraria.Dust.NewDust(position, npc.width, npc.height, 156, 0f, -3.421053f, 117, new Color(0, 255, 142), .6f)];
						dust.noGravity = true;

					}
				}
			}
		}
		public override void HitEffect(NPC npc, int hitDirection, double damage)
		{
			if(elementalType.Contains("Possessed ")) {
				for(int i = 0; i < 10; i++) {
					int d1 = 156;
					Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 117, new Color(0, 255, 142), .6f);
				}
			}
		}
		public override bool PreAI(NPC npc)
		{
			if(!MPSynced) {
				if(Main.netMode == NetmodeID.Server) {
					//send packet containing npc.whoAmI and all prefixes/suffixes.
					var netMessage = mod.GetPacket();
					netMessage.Write("elementalType");
					netMessage.Write(npc.whoAmI);
					bool haselementalType = elementalType.Length > 0;
					netMessage.Write(haselementalType);
					if(haselementalType) {
						netMessage.Write(elementalType);
					}
					netMessage.Send();
				}
				MPSynced = true;
				return true;
			}
			if(!nameConfirmed && (Main.netMode != 1 || readyForChecks))//block to ensure all enemies retain prefixes in their displayNames
			{
				if(elementalType.Length > 1)//has prefixes
				   {
					npc.GivenName = npc.FullName;
					string[] elementalTypeArr = elementalType.Split('#');
					for(int i = 0; i < elementalTypeArr.Length; i++) {
						if(!npc.GivenName.Contains(elementalTypeArr[i])) {
							npc.GivenName = elementalType[i] + npc.GivenName;
						}
					}
				}
				npc.GivenName = npc.GivenName.Replace("  the", "");
				nameConfirmed = true;
			}
			return base.PreAI(npc);
		}
		public override bool CheckDead(NPC npc)
		{
			if(elementalType.Contains("Possessed ")) {
				Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 53);
				int newNPC = NPC.NewNPC((int)npc.position.X + Main.rand.Next(-10, 10), (int)npc.position.Y + Main.rand.Next(-10, 10), ModContent.NPCType<SpectralSkull>());
				int newNPC1 = NPC.NewNPC((int)npc.position.X + Main.rand.Next(-10, 10), (int)npc.position.Y + Main.rand.Next(-10, 10), ModContent.NPCType<SpectralSkull>());
				int newNPC2 = NPC.NewNPC((int)npc.position.X + Main.rand.Next(-10, 10), (int)npc.position.Y + Main.rand.Next(-10, 10), ModContent.NPCType<SpectralSkull>());
			}
			return base.CheckDead(npc);
		}
	}
}