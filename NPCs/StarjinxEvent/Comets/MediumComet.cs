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
		public override string Size => "Medium";

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
			int numEnemies = 0;
			switch (choice)
			{
				case 0: //4 random starachnid or starweaver, 50% chance for empowerment
					numEnemies = 4;
					for (int i = 0; i < 4; ++i)
					{
						Vector2 spawn = SpawnSpawnerProjectile(Main.rand.NextBool(2) ? ModContent.NPCType<Starachnid>() : ModContent.NPCType<StarWeaverNPC>());
						if (Main.rand.NextBool())
						{
							numEnemies++;
							SpawnSpawnerProjectile(ModContent.NPCType<Pathfinder>(), spawn);
						}
					}
					break;
				case 1: //1 empowered meteor magus
					numEnemies = 2;
					var offset = SpawnSpawnerProjectile(ModContent.NPCType<MeteorMagus_NPC>());
					SpawnSpawnerProjectile(ModContent.NPCType<Pathfinder>(), offset);
					break;
				default: //6 starachnid, 67% chance for empowerment
					numEnemies = 6;
					for (int i = 0; i < 6; ++i)
					{
						Vector2 spawn = SpawnSpawnerProjectile(ModContent.NPCType<Starachnid>());
						if (Main.rand.Next(3) > 0)
						{
							numEnemies++;
							SpawnSpawnerProjectile(ModContent.NPCType<Pathfinder>(), spawn);
						}
					}
					break;
			}

			StarjinxEventWorld.SetMaxEnemies(numEnemies);
		}
	}
}