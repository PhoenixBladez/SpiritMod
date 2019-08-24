using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Overseer
{
	public class SeerScreenShaderData : ScreenShaderData
	{
		private int SeerIndex;

		public SeerScreenShaderData(string passName)
			: base(passName)
		{
		}

		private void UpdateSeerIndex()
		{
			int SeerType = ModLoader.GetMod("SpiritMod").NPCType("Overseer");
			if (SeerIndex >= 0 && Main.npc[SeerIndex].active && Main.npc[SeerIndex].type == SeerType)
				return;

			SeerIndex = -1;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == SeerType)
				{
					SeerIndex = i;
					break;
				}
			}
		}

		public override void Apply()
		{
			UpdateSeerIndex();
			if (SeerIndex != -1)
				UseTargetPosition(Main.npc[SeerIndex].Center);

			base.Apply();
		}
	}
}