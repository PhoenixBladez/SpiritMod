using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GraniteArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class GraniteLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Greaves");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/GraniteArmor/GraniteLegs_Glow");

        }

        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            glowMaskColor = Color.White;
        }
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = 1100;
            item.rare = 2;
            item.defense = 7;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "GraniteChunk", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
