using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ReachBoss
{
    [AutoloadEquip(EquipType.Head)]
    public class ReachBossHead : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thornspeaker's Helmet");
            Tooltip.SetDefault("Increases throwing damage by 7% and throwing velocity by 5%");

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
            player.thrownDamage += 0.07f;
			player.thrownVelocity += 0.05f;
        }
			public override void UpdateArmorSet(Player player)
        {

            player.setBonus = "'Prey on the weak'\nThrowing crits do 75% more damage to enemies under half health\nIncreases throwing velocity by 15%";
            player.GetSpiritPlayer().reachSet = true;
			player.thrownVelocity += .15f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("ReachBossBody") && legs.type == mod.ItemType("ReachBossLegs");
        }
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ReachFlowers", 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
