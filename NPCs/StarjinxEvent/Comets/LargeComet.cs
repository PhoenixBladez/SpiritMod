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
    public class LargeComet : SmallComet
    {
		protected override string Size => "Large";
		protected override float BeamScale => 1.25f;
		public override void SetStaticDefaults() => DisplayName.SetDefault("Large Starjinx Comet");

		public override void SetDefaults()
        {
            base.SetDefaults();
            npc.lifeMax = 20;
            npc.width = 62;
            npc.height = 58;
        }

		public override void SpawnWave()
		{
			int choice = Main.rand.Next(4);

			switch (choice)
			{

				default: //2 magus & 2 pathfinder connected, 1 starachnid
					for (int i = 0; i < 2; ++i)
					{
						var offset = new Vector2(i == 0 ? -300 : 300, -300);

						SpawnValidNPC(ModContent.NPCType<MeteorMagus>(), offset);
						SpawnValidNPC(ModContent.NPCType<Pathfinder>(), offset);
					}
					SpawnValidNPC(ModContent.NPCType<Starachnid>());
					break;
			}
		}
	}
}