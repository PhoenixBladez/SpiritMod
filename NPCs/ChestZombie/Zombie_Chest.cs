using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.ChestZombie
{
	public class Zombie_Chest : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 32;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.White;
		}

		public override void SetStaticDefaults() => DisplayName.SetDefault("Chest");
		public override void GrabRange(Player player, ref int grabRange) => grabRange *= 0;
		public override bool ItemSpace(Player player) => true;

		public override bool OnPickup(Player player)
		{
			for (int i = 0; i < Item.stack; i++)
			{
				int bab = Main.rand.Next(11);
				if (bab == 0)
					Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 280);
				if (bab == 1)
					Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 281);
				if (bab == 2)
					Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 284);
				if (bab == 3)
					Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 282);
				if (bab == 4)
					Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 279);
				if (bab == 5)
					Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 285);
				if (bab == 6)
					Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 953);
				if (bab == 7)
					Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 946);
				if (bab == 8)
					Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 3068);
				if (bab == 9)
					Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 3069);
				if (bab == 10)
					Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 3084);

				if (Main.rand.Next(6) == 0)
				{
					int p = Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 3093);
					if (Main.rand.Next(5) == 0)
						Main.item[p].stack += Main.rand.Next(2);
					if (Main.rand.Next(10) == 0)
						Main.item[p].stack += Main.rand.Next(2);
				}

				if (Main.rand.Next(3) == 0)
				{
					int p = Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 168);
					Main.item[p].stack = Main.rand.Next(3, 6);
				}

				if (Main.rand.Next(2) == 0)
				{
					int num3 = Main.rand.Next(2);
					int num4 = Main.rand.Next(8) + 3;
					if (num3 == 0)
						Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, WorldGen.copperBar, num4);
					if (num3 == 1)
						Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, WorldGen.ironBar, num4);
				}

				if (Main.rand.Next(2) == 0)
				{
					int num3 = Main.rand.Next(50, 101);
					Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 965, num3);
				}

				if (Main.rand.Next(3) != 0)
				{
					int num3 = Main.rand.Next(2);
					int num4 = Main.rand.Next(26) + 25;
					if (num3 == 0)
						Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 40, num4);
					if (num3 == 1)
						Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 42, num4);
				}

				if (Main.rand.Next(2) == 0)
				{
					int num3 = Main.rand.Next(1);
					int num4 = Main.rand.Next(3) + 3;
					if (num3 == 0)
						Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 28, num4);
				}

				if (Main.rand.Next(3) != 0)
				{
					int p = Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 2350);
					Main.item[p].stack = Main.rand.Next(2, 5);
				}

				if (Main.rand.Next(3) > 0)
				{
					int num3 = Main.rand.Next(6);
					int num4 = Main.rand.Next(1, 3);
					if (num3 == 0)
						Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 292, num4);
					if (num3 == 1)
						Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 298, num4);
					if (num3 == 2)
						Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 299, num4);
					if (num3 == 3)
						Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 290, num4);
					if (num3 == 4)
						Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 2322, num4);
					if (num3 == 5)
						Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 2325, num4);
				}

				if (Main.rand.Next(2) == 0)
				{
					int num3 = Main.rand.Next(2);
					int num4 = Main.rand.Next(11) + 10;
					if (num3 == 0)
						Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 8, num4);
					if (num3 == 1)
						Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 31, num4);
				}

				if (Main.rand.Next(2) == 0)
					Item.NewItem(player.GetSource_Loot("Pickup"), (int)player.position.X, (int)player.position.Y, player.width, player.height, 72, Main.rand.Next(10, 30));
			}
			return false;
		}
	}
}
