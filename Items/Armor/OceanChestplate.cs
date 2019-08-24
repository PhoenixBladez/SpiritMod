using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class OceanChestplate : ModItem
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diver's Chestplate");
            Tooltip.SetDefault("Increases minion damage by 7%, and maximum mana by 20 \n Increases maximum number of minions by 1 \n Allows the player to move freely underwater");

        }
        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 18;
            item.value = Terraria.Item.sellPrice(0, 0, 30, 0);
            item.rare = 3;
            item.defense = 7;
        }
        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.07f;
            player.maxMinions += 1;
            player.statManaMax2 += 20;
            player.ignoreWater = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Coral, 8);
            recipe.AddIngredient(null, "PearlFragment", 13);
            recipe.AddIngredient(ItemID.BottledWater, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}