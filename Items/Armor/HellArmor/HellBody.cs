using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.HellArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class  HellBody : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Malevolent Platemail");
			Tooltip.SetDefault("Increases movement speed by 15% and increases ranged critical strike chance by 8%");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 18;
            item.value = 46000;
            item.rare = 6;
            item.defense = 20;
        }
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.15f;
            player.rangedCrit += 8;
			player.maxRunSpeed += 1;

        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FieryEssence", 20);
            recipe.AddTile(null,"EssenceDistorter");
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}