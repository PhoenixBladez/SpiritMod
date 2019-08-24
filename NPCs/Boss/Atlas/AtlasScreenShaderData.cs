using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Atlas
{
	public class AtlasScreenShaderData : ScreenShaderData
	{
		private int AtlasIndex;

		public AtlasScreenShaderData(string passName)
			: base(passName)
		{
		}

		private void UpdateAtlasIndex()
		{
			int AtlasType = ModLoader.GetMod("SpiritMod").NPCType("Atlas");
			if (AtlasIndex >= 0 && Main.npc[AtlasIndex].active && Main.npc[AtlasIndex].type == AtlasType)
				return;

			AtlasIndex = -1;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == AtlasType)
				{
					AtlasIndex = i;
					break;
				}
			}
		}

		public override void Apply()
		{
			UpdateAtlasIndex();
			if (AtlasIndex != -1)
				UseTargetPosition(Main.npc[AtlasIndex].Center);

			base.Apply();
		}
	}
}