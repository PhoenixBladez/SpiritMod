using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class StellarMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Mask");
			Tooltip.SetDefault("Increases ranged damage by 10% and ranged critical strike chance by 5%\nIncreases your max number of minions");
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 7;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<StellarPlate>() && legs.type == ModContent.ItemType<StellarLeggings>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Ranged critical strikes empower your minions with increased attack and knockback\nKilling enemies with minions empowers your movement speed";
			player.GetSpiritPlayer().stellarSet = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.rangedDamage += 0.1f;
			player.rangedCrit += 5;
			player.maxMinions += 1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<StarPiece>(), 1);
			recipe.AddIngredient(ItemID.TitaniumBar, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe1 = CreateRecipe(1);
			recipe1.AddIngredient(ModContent.ItemType<StarPiece>(), 1);
			recipe1.AddIngredient(ItemID.AdamantiteBar, 10);
			recipe1.AddTile(TileID.MythrilAnvil);
			recipe1.Register();
		}
	}
}
