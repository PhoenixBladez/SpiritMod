using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent
{
	/// <summary>Used exclusively to check if an NPC has been spawned by a Starjinx Comet. May be phased out once the waves proper are added.</summary>
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
	
	class StarjinxNonInstancedGlobalNPC : GlobalNPC
	{
		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			if (StarjinxEventWorld.StarjinxActive && player.HasBuff(ModContent.BuffType<Buffs.HighGravityBuff>()))
				maxSpawns = 0;
		}
	}
}
