using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.StarArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class StarPlate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starplate Chestguard");
            Tooltip.SetDefault("Increases ranged damage by 6%");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/StarArmor/StarPlate_Glow");
        }
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            glowMaskColor = Color.White;
        }
        int timer = 0;

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = Terraria.Item.sellPrice(0, 0, 38, 0);
            item.rare = 3;
            item.defense = 7;
        }
        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += .06f;
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SteamParts", 9);
            recipe.AddIngredient(null, "CosmiliteShard", 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
