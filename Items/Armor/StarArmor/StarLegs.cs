using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.StarArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class StarLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starplate Leggings");
            Tooltip.SetDefault("Increases ranged damage by 6% and movement speed by 8%");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/StarArmor/StarLegs_Glow");
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
            item.value = Terraria.Item.sellPrice(0, 0, 35, 0);
            item.rare = 3;
            item.defense = 5;
        }
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.08f;
            player.maxRunSpeed += 0.05f;
            player.rangedDamage += .06f;
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SteamParts", 8);
            recipe.AddIngredient(null, "CosmiliteShard", 11);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
