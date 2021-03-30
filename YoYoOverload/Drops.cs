using SpiritMod.YoYoOverload.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoyoOverload
{
	internal class Drops : GlobalNPC
	{
		public override void NPCLoot(NPC npc)
		{

			/*if (npc.type == NPCID.Spazmatism && Main.rand.Next(2) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Cursed>(), 1, false, 0, false, false);
			}*/

			if (npc.type == NPCID.SantaNK1 && Main.rand.Next(8) == 0) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Crumbler>(), 1, false, 0, false, false);
			}
			/*if (npc.type == NPCID.IchorSticker && Main.rand.Next(28) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Ichor>(), 1, false, 0, false, false);
			}
			if (npc.type == NPCID.FloatyGross && Main.rand.Next(28) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Ichor>(), 1, false, 0, false, false);
			}
			if (npc.type == NPCID.PirateCorsair && Main.rand.Next(50) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PBoot>(), 1, false, 0, false, false);
			}
			if (npc.type == NPCID.PirateCrossbower && Main.rand.Next(50) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PBoot>(), 1, false, 0, false, false);
			}
			if (npc.type == NPCID.PirateCaptain && Main.rand.Next(25) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PBoot>(), 1, false, 0, false, false);
			}*/
			
			/*if (npc.type == NPCID.GoblinSorcerer && Main.rand.Next(30) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CBall>(), 1, false, 0, false, false);
			}*/

			/*if (npc.type == NPCID.BloodZombie && Main.rand.Next(25) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Plague>(), 1, false, 0, false, false);
			}
			if (npc.type == NPCID.Drippler && Main.rand.Next(25) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Plague>(), 1, false, 0, false, false);
			}*/
			/*if (npc.type == NPCID.RuneWizard)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Rune>(), 1, false, 0, false, false);
			}
			if (npc.type == NPCID.Reaper && Main.rand.Next(50) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Reap>(), 1, false, 0, false, false);
			}
			if (npc.type == NPCID.GreekSkeleton && NPC.downedBoss3 && Main.rand.Next(25) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Medusa>(), 1, false, 0, false, false);
			}
			if (npc.type == NPCID.GraniteFlyer && NPC.downedBoss3 && Main.rand.Next(25) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Granite>(), 1, false, 0, false, false);
			}
			if (npc.type == NPCID.DesertLamiaLight && Main.rand.Next(28) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Equinox>(), 1, false, 0, false, false);
			}
			if (npc.type == NPCID.DesertLamiaDark && Main.rand.Next(28) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Equinox>(), 1, false, 0, false, false);
			}*/
		}
	}
}
