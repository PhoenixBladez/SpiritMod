using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
    [AutoloadEquip(EquipType.Shield)]
    public class ClatterboneShield : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fervent Protector");
			Tooltip.SetDefault("Increases defense by 2 and reduces movement speed for every nearby enemy\nThis effect stacks up to five times\n'There is something special between us'");
		}

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.rare = 2;
            item.defense = 2;
            item.melee = true;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
           player.GetModPlayer<MyPlayer>(mod).clatterboneShield = true;

           player.statDefense += 2 * player.GetModPlayer<MyPlayer>(mod).clatterStacks;
           player.moveSpeed -= .04f * player.GetModPlayer<MyPlayer>(mod).clatterStacks;
           player.maxRunSpeed -= .04f * player.GetModPlayer<MyPlayer>(mod).clatterStacks;
            player.runAcceleration -= .005f * player.GetModPlayer<MyPlayer>(mod).clatterStacks;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("LeatherShield"), 1);
            recipe.AddIngredient(null, "Carapace", 6);
            recipe.AddRecipeGroup("EvilMaterial1", 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
