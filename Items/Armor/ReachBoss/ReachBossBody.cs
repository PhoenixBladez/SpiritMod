using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ReachBoss
{
    [AutoloadEquip(EquipType.Body)]
    public class ReachBossBody : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thornspeaker's Garb");
            Tooltip.SetDefault("Increases max minions by 1");

        }


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 30200;
            item.rare = 2;
            item.defense = 5;
        }
        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 1;
        }
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ReachFlowers", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
