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
			DisplayName.SetDefault("Vinecaller's Garb");
            Tooltip.SetDefault("Increases throwing damage by 8% and movement speed by 6%");

        }


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 30200;
            item.rare = 2;
            item.defense = 4;
        }
        public override void UpdateEquip(Player player)
        {
            player.thrownDamage += 0.08f;
			player.moveSpeed += .06f;
			player.maxRunSpeed += 0.03f;
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
