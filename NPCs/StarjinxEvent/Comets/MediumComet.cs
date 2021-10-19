using Microsoft.Xna.Framework;
using SpiritMod.NPCs.StarjinxEvent.Enemies.MeteorMagus;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Pathfinder;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Starachnid;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent.Comets
{
    public class MediumComet : SmallComet
    {
		protected override string Size => "Medium";
		protected override float BeamScale => 1f;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Medium Starjinx Comet");

		public override void SetDefaults()
        {
            base.SetDefaults();
            npc.lifeMax = 15;
            npc.width = 42;
            npc.height = 40;
        }

		public override void SpawnWave()
		{
			int choice = Main.rand.Next(4);

			switch (choice)
			{

				default: //2 magus & 2 pathfinder connected, 1 starachnid
					for (int i = 0; i < 5; ++i)
					{
						var spawn = SpawnValidNPC(ModContent.NPCType<Starachnid>());
						if (Main.rand.NextBool())
							SpawnValidNPC(ModContent.NPCType<Pathfinder>(), spawn);
					}
					break;
			}
		}
	}
}