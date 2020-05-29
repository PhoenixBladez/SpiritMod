using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Armor.SeraphArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class SeraphLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraph's Greaves");
			 Tooltip.SetDefault("Increases maximum mana by 50");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/SeraphArmor/SeraphLegs_Glow");
        }
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 16;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 4;
            item.defense = 12;
        }
        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 50;

        }
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            glowMaskColor = Color.White;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MoonStone", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
