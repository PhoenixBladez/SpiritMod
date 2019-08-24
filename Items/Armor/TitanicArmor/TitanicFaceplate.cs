using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.TitanicArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class TitanicFaceplate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanic Faceplate");
			Tooltip.SetDefault("Increases melee damage by 10% and melee critical strike chance by 8%");
		}

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 30;
            item.value = 10000;
            item.rare = 6;

            item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.1F;
            player.meleeCrit += 8;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("TitanicPlatemail") && legs.type == mod.ItemType("TitanicGreaves");
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "'The seven seas flow in your favor' \n Melee hits on enemies causes water to erupt around you and damage enemies";
            player.GetModPlayer<MyPlayer>(mod).titanicSet = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "TidalEssence", 16);
            recipe.AddTile(null, "EssenceDistorter");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
