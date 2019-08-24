using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.SnowArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class SnowRangerHead : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hunter's Cowl");
            Tooltip.SetDefault("Increases ranged damage by 4%\nIncreases ranged critical strike chance by 3%");

        }


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 3000;
            item.rare = 2;
            item.defense = 4;
        }
         public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.04f;
            player.rangedCrit += 3;
        }
			public override void UpdateArmorSet(Player player)
        {

            player.setBonus = "Provides immunity to the Chilled debuff\nIncreases ranged damage by 4%";
            player.rangedDamage += 0.04f;
            player.buffImmune[BuffID.Chilled] = true;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("SnowRangerBody") && legs.type == mod.ItemType("SnowRangerLegs");
        }
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SturdyFur", 6);
            recipe.AddIngredient(null, "FrigidFragment", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
