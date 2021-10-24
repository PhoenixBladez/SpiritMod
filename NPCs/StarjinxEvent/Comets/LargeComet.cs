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
    public class LargeComet : SmallComet
    {
		protected override string Size => "Large";
		protected override float BeamScale => 1.25f;
		public override void SetStaticDefaults() => DisplayName.SetDefault("Large Starjinx Comet");

		public override void SetDefaults()
        {
            base.SetDefaults();
            npc.lifeMax = 800;
            npc.width = 62;
            npc.height = 58;
        }

		public override void SpawnWave()
		{
			int choice = Main.rand.Next(4);

			switch (choice)
			{
				case 0: //circle of 5 empowered starweavers
					npc.TargetClosest(false);
					Player target = Main.player[npc.target];

					for (int i = 0; i < 5; ++i)
					{
						var offset = Vector2.One.RotatedBy(i * (MathHelper.TwoPi / 5)) * 250;

						SpawnValidNPC(ModContent.NPCType<StarWeaverNPC>(), target.Center + offset, true);
						SpawnValidNPC(ModContent.NPCType<Pathfinder>(), target.Center + offset, true);
					}
					break;
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