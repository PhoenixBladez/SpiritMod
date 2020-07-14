using Terraria;
using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ElderbarkArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class ElderbarkHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderbark Helmet");
		}


		int timer = 0;

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 24;
			item.value = Terraria.Item.sellPrice(0, 0, 0, 0);
			item.rare = ItemRarityID.White;
			item.defense = 1;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<ElderbarkChest>() && legs.type == ModContent.ItemType<ElderbarkLegs>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"Increases all item damage by 1";
			player.GetSpiritPlayer().elderbarkWoodSet = true;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
