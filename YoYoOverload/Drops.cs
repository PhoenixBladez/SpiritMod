using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.YoyoOverload
{
	internal class Drops : GlobalNPC
	{
		public override void NPCLoot(NPC npc)
		{

			if (npc.type == 126 && Main.rand.Next(2) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("Cursed"), 1, false, 0, false, false);
			}

			if (npc.type == 346 && Main.rand.Next(8) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("Crumbler"), 1, false, 0, false, false);
			}
			if (npc.type == 268 && Main.rand.Next(28) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("Ichor"), 1, false, 0, false, false);
			}
			if (npc.type == 182 && Main.rand.Next(28) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("Ichor"), 1, false, 0, false, false);
			}
			if (npc.type == 113 && Main.rand.Next(2) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("WEye"), 1, false, 0, false, false);
			}

			if (npc.type == 213 && Main.rand.Next(50) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("PBoot"), 1, false, 0, false, false);
			}
			if (npc.type == 215 && Main.rand.Next(50) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("PBoot"), 1, false, 0, false, false);
			}
			if (npc.type == 216 && Main.rand.Next(25) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("PBoot"), 1, false, 0, false, false);
			}
			if (npc.type == 127 && Main.rand.Next(4) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("SkellyV"), 1, false, 0, false, false);
			}
			if (npc.type == 29 && Main.rand.Next(30) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("CBall"), 1, false, 0, false, false);
			}
			if (npc.type == 267 && Main.rand.Next(25) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("Creep"), 1, false, 0, false, false);
			}

			if (npc.type == 489 && Main.rand.Next(25) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("Plague"), 1, false, 0, false, false);
			}
			if (npc.type == 490 && Main.rand.Next(25) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("Plague"), 1, false, 0, false, false);
			}
			if (npc.type == 35)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("Bone"), 1, false, 0, false, false);
			}
			if (npc.type == 172)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("Rune"), 1, false, 0, false, false);
			}
			if (npc.type == 253 && Main.rand.Next(50) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("Reap"), 1, false, 0, false, false);
			}
			if (npc.type == 481 && NPC.downedBoss3 && Main.rand.Next(25) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("Medusa"), 1, false, 0, false, false);
			}
			if (npc.type == 483 && NPC.downedBoss3 && Main.rand.Next(25) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("Granite"), 1, false, 0, false, false);
			}
			if (npc.type == 528 && Main.rand.Next(28) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("Equinox"), 1, false, 0, false, false);
			}
			if (npc.type == 529 && Main.rand.Next(28) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, base.mod.ItemType("Equinox"), 1, false, 0, false, false);
			}
		}
	}
}
