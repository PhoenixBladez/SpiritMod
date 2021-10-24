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
			int choice = Main.rand.Next(3);

			switch (choice)
			{
				case 0: //4 random starachnid or starweaver, 50% chance for empowerment
					for (int i = 0; i < 4; ++i)
					{
						Vector2 spawn = SpawnValidNPC(Main.rand.NextBool(2) ? ModContent.NPCType<Starachnid>() : ModContent.NPCType<StarWeaverNPC>());
						if (Main.rand.NextBool())
							SpawnValidNPC(ModContent.NPCType<Pathfinder>(), spawn);
					}
					break;
				case 1: //1 empowered meteor magus
					var offset = SpawnValidNPC(ModContent.NPCType<MeteorMagus>());
					SpawnValidNPC(ModContent.NPCType<Pathfinder>(), offset);
					break;
				default: //6 starachnid, 67% chance for empowerment
					for (int i = 0; i < 6; ++i)
					{
						Vector2 spawn = SpawnValidNPC(ModContent.NPCType<Starachnid>());
						if (Main.rand.Next(3) > 0)
							SpawnValidNPC(ModContent.NPCType<Pathfinder>(), spawn);
					}
					break;
			}
		}
	}
}