using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GeodeArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class GeodeHelmet : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geode Helmet");
			Tooltip.SetDefault("Increases movement speed by 15%");
		}
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 30;
            item.value = Terraria.Item.sellPrice(0, 0, 75, 0);
            item.rare = 4;

            item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.15f;
            player.maxRunSpeed += 1;

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("GeodeChestplate") && legs.type == mod.ItemType("GeodeLeggings");
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Critical hits crystallize foes, slowing them in place";
            player.GetModPlayer<MyPlayer>(mod).geodeSet = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Geode", 14);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
