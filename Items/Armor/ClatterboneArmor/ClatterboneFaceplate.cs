
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ClatterboneArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class ClatterboneFaceplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clatterbone Faceplate");
		}

		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 30;
			item.value = Item.buyPrice(gold: 1, silver: 10);
			item.rare = ItemRarityID.Green;

			item.defense = 3;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<ClatterboneBreastplate>() && legs.type == ModContent.ItemType<ClatterboneLeggings>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Taking fatal damage will instead return you to the amount of health you had before death\n6 minute cooldown";
			player.GetSpiritPlayer().clatterboneSet = true;
		}


		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Carapace>(), 7);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
