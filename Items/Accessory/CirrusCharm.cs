using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class CirrusCharm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cirrus Crystal");
            Tooltip.SetDefault("Leave a trail of chilly embers as you walk\nThrowing weapons may inflict 'Soul Burn'\nMagic attacks may inflict 'Frostburn'");
            ItemID.Sets.ItemIconPulse[item.type] = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;

        }


        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 28;
            item.value = Item.sellPrice(0, 2, 33, 0);
            item.rare = 5;
            item.defense = 3;
            item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.jumpSpeedBoost += 6f;
            player.GetModPlayer<MyPlayer>(mod).icytrail = true;
            player.GetModPlayer<MyPlayer>(mod).icySoul = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FrigidWind", 1);
            recipe.AddIngredient(null, "FrostSoul", 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
