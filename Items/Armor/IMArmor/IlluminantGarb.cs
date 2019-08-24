using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.IMArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class IlluminantGarb : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Platemail");
            Tooltip.SetDefault("Increases max life by 25 and damage by 10%");

        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 24;
            item.value = 120000;
            item.rare = 7;
            item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.10f;
            player.meleeDamage += 0.10f;
            player.thrownDamage += 0.10f;
            player.rangedDamage += 0.10f;
            player.minionDamage += 0.10f;
            player.statLifeMax2 += 25;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "IlluminatedCrystal", 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
