using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.IlluminantMaster
{
	public class IMScreenShaderData : ScreenShaderData
	{
		private int IMIndex;

		public IMScreenShaderData(string passName)
			: base(passName)
		{
		}

		private void UpdateIMIndex()
		{
			int IMType = ModLoader.GetMod("SpiritMod").NPCType("IlluminantMaster");
			if (IMIndex >= 0 && Main.npc[IMIndex].active && Main.npc[IMIndex].type == IMType)
				return;

			IMIndex = -1;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == IMType)
				{
					IMIndex = i;
					break;
				}
			}
		}

		public override void Apply()
		{
			UpdateIMIndex();
			if (IMIndex != -1)
				UseTargetPosition(Main.npc[IMIndex].Center);

			base.Apply();
		}
	}
}