using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class FrostFireCharm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frostfire Necklace");
            Tooltip.SetDefault("Increases melee damage by 6%\nMelee attacks may inflict 'On Fire'\nGreatly increases jump height\nLeave a trail of chilly embers as you walk\nThrowing weapons may inflict 'Soul Burn'\nMagic attacks may inflict 'Frostburn'");

        }


        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 52;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 6;
            item.defense = 3;
            item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.jumpSpeedBoost += 6f;
            player.GetModPlayer<MyPlayer>(mod).icytrail = true;
            player.GetModPlayer<MyPlayer>(mod).icySoul = true;
            if (Main.rand.Next(10) > 3)
            {
                player.magmaStone = true;
            }
            player.meleeDamage *= 1.06f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FieryPendant", 1);
            recipe.AddIngredient(null, "CirrusCharm", 1);
            recipe.AddIngredient(null, "FrigidFragment", 5);
            recipe.AddIngredient(null, "InfernalAppendage", 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
