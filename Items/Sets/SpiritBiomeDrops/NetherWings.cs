
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritBiomeDrops
{
	[AutoloadEquip(EquipType.Wings)]
	public class NetherWings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nether Wings");
			Tooltip.SetDefault("Allows for flight and slow fall.");

			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new Terraria.DataStructures.WingStats(90, 7.6f, 1.3f);
		}

		public override void SetDefaults()
		{
			Item.width = 47;
			Item.height = 37;
			Item.value = 60000;
			Item.accessory = true;
			Item.rare = ItemRarityID.Pink;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.wingTimeMax = 190;
			if (Main.rand.NextBool(4))
				Dust.NewDust(player.position, player.width, player.height, DustID.UnusedWhiteBluePurple);
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.65f;
			ascentWhenRising = 0.07f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 2.2f;
			constantAscend = 0.095f;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ModContent.ItemType<NetherCrystal>(), 1);
			modRecipe.AddIngredient(ItemID.SoulofFlight, 20);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.Register();
		}
	}
}