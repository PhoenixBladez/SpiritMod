using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.SpiritWood
{
    [AutoloadEquip(EquipType.Head)]
    public class SpiritWoodHead : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duskwood Helmet");

        }


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 0;
            item.rare = 0;
            item.defense = 4;
        }
			public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Increases defense by 2";
            player.statDefense += 2;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("SpiritWoodBody") && legs.type == mod.ItemType("SpiritWoodLegs");
        }
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritWoodItem", 25);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
