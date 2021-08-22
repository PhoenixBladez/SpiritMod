using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent
{
	class StarjinxGlobalNPC : GlobalNPC
	{
		public bool spawnedByComet = false;

		public override bool CloneNewInstances => true;
		public override bool InstancePerEntity => true;

		public override void SetDefaults(NPC npc) => spawnedByComet = false;

		public override bool CheckDead(NPC npc)
		{
			spawnedByComet = false;
			return true;
		}
	}
}
