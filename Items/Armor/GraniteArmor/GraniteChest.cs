using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GraniteArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class GraniteChest : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Breastplate");
            Tooltip.SetDefault("Reduces movement speed by 15%\nIncreases damage dealt by 3%");

        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = 1100;
            item.rare = 2;
            item.defense = 8;
        }
        public override void UpdateEquip(Player player)
        {
            player.maxRunSpeed -= 0.15f;
            player.meleeDamage += 0.03f;
            player.rangedDamage += 0.03f;
            player.magicDamage += 0.03f;
            player.thrownDamage += 0.03f;
            player.minionDamage += 0.03f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "GraniteChunk", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
