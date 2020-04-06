using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.FieryArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class ObsidiusHelm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slag Tyrant's Helm");
			Tooltip.SetDefault("Increases minion damage by 6%\nIncreases your maximum number of sentries by 1");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/FieryArmor/ObsidiusHelm_Glow");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = Terraria.Item.sellPrice(0, 0, 35, 0);
            item.rare = 3;
            item.defense = 5;
        }
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            glowMaskColor = Color.White;
        }
        public override void UpdateEquip(Player player)
        {
            player.maxTurrets+= 1;
            player.minionDamage += .06f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("ObsidiusPlate") && legs.type == mod.ItemType("ObsidiusGreaves");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CarvedRock", 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Press the 'Armor Bonus' hotkey to cause all sentries to release a burst of fireballs\n8 second cooldown";
            player.GetSpiritPlayer().fierySet = true;
        }
    }
}
