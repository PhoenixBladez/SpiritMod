using Microsoft.Xna.Framework;
using SpiritMod.NPCs.StarjinxEvent.Enemies.MeteorMagus;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Pathfinder;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Starachnid;
using SpiritMod.NPCs.StarjinxEvent.Enemies.StarWeaver;
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
            npc.lifeMax = 500;
            npc.width = 42;
            npc.height = 40;
        }

		public override void SpawnWave()
		{
			int choice = Main.rand.Next(4);

			switch (choice)
			{
				case 0:
					SpawnValidNPC(ModContent.NPCType<StarWeaverNPC>());
					break;
				default: //2 empowered starachnids
					for (int i = 0; i < 5; ++i)
					{
						Vector2 spawn = SpawnValidNPC(ModContent.NPCType<Starachnid>());
						if (Main.rand.NextBool())
							SpawnValidNPC(ModContent.NPCType<Pathfinder>(), spawn);
					}
					break;
			}
		}
	}
}