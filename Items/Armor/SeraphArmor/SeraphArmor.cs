using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Armor.SeraphArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class SeraphArmor : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraph's Breastplate");
            Tooltip.SetDefault("Increases magic damage by 10%");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/SeraphArmor/SeraphBody_Glow");

        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 24;
            item.value = 60000;
            item.rare = 4;
            item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += .10f;
        }
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            glowMaskColor = Color.White;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MoonStone", 13);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
