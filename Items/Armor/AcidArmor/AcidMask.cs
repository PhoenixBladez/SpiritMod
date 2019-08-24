using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.AcidArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class AcidMask : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Mask");
			Tooltip.SetDefault("Increases throwing velocity by 7% and throwing damage by 10%");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 18;
            item.value = 46000;
            item.rare = 5;
            item.defense = 7;
        }
        public override void UpdateEquip(Player player)
        {
            player.thrownDamage+= 0.1f;
            player.thrownVelocity += 0.07f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("AcidBody") && legs.type == mod.ItemType("AcidLegs");  
        }
        public override void UpdateArmorSet(Player player)
        {
  
            player.setBonus = "Getting hurt may release an acid explosion, causing enemies to suffer Acid Burn \nThrowing hits may instantly kill non boss enemies extremely rarely.";
            player.GetModPlayer<MyPlayer>(mod).acidSet = true;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Acid", 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}