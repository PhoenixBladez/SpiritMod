using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Neck)]
    public class BoneCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bone Charm");
			Tooltip.SetDefault("Increases maximum mana by 40 when below 50% health");
		}

        public override void SetDefaults()
		{
			item.width = 26;
			item.height = 24;
            item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = 3;

			item.accessory = true;
			item.defense = 1;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.statLife <= player.statLifeMax2 / 2)
			{
                player.statManaMax2 += 60;
            }
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Bone, 20);
            recipe.AddIngredient(ItemID.ManaCrystal);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
