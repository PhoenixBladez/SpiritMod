using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Items.Placeable.Furniture;

namespace SpiritMod.Items.Sets.Vulture_Matriarch
{
	public class GoldenEgg : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Vulture Matriarch's Golden Egg");

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 42;
			Item.maxStack = 999;
			Item.rare = -11;
		}

		public override void GrabRange(Player player, ref int grabRange) => grabRange = 0;
		public override bool ItemSpace(Player player) => true;

		public override bool OnPickup(Player player)
		{
			for (int i = 0; i < Item.stack; i++)
			{
				if (Main.rand.Next(5) == 0)
					player.QuickSpawnItem(ModContent.ItemType<HourglassItem>(), 1);

				if (Main.rand.Next(7) == 0)
					player.QuickSpawnItem(ModContent.ItemType<Vulture_Matriarch_Mask>(), 1);

				player.QuickSpawnItem(Main.rand.Next(new int[] { ModContent.ItemType<Tome_of_the_Great_Scavenger.Tome_of_the_Great_Scavenger>(), ModContent.ItemType<Sovereign_Talon.Sovereign_Talon>() }), 1);
				player.QuickSpawnItem(ItemID.GoldCoin, 1);
				player.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(20, 40));
			}
			return false;
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			if (Main.rand.Next(20) == 0)
			{
				int index3 = Dust.NewDust(new Vector2(Item.position.X, Item.position.Y), Item.width, Item.height, DustID.GoldFlame, 0.0f, 0.0f, 150, default, 0.3f);
				Dust dust = Main.dust[index3];
				dust.fadeIn = 0.75f;
				dust.velocity *= 0.1f;
				dust.noLight = true;
				dust.noGravity = true;
			}
		}
	}
}