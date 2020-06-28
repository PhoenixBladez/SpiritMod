
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.LeatherArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class LeatherHood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marksman's Hood");
			Tooltip.SetDefault("Increases ranged weapon damage by 1");
		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 12;
			item.value = 100;
			item.rare = 1;

			item.defense = 1;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<LeatherPlate>() && legs.type == ModContent.ItemType<LeatherLegs>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Wearing Marksman's Armor builds up concentration\nWhile concentrated, your next strike is a critical strike and deals more damage\nConcentration is disrupted when hurt, but charges faster while standing still";
			player.GetSpiritPlayer().leatherSet = true;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetSpiritPlayer().leatherHood = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
