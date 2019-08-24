using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GeodeArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class GeodeLeggings : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geode Leggings");
			Tooltip.SetDefault("Increases damage by 7% and increases critical strike chance by 6%");
		}

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 22;
            item.value = Terraria.Item.sellPrice(0, 0, 75, 0);
            item.rare = 5;

            item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.thrownDamage += 0.07F;
            player.meleeDamage += 0.07F;
            player.minionDamage += 0.07F;
            player.magicDamage += 0.07F;
            player.rangedDamage += 0.07F;
            player.thrownCrit += 6;
            player.rangedCrit += 6;
            player.magicCrit += 6;
            player.meleeCrit += 6;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Geode", 13);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
