using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class DuneHelm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dune Helm");
			Tooltip.SetDefault("Increases throwing velocity by 10% and thrown damage by 20%");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 18;
            item.value = 46000;
            item.rare = 6;
            item.defense = 12;
        }
        public override void UpdateEquip(Player player)
        {
            player.thrownDamage+= 0.20f;
            player.thrownVelocity += 0.10f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("DunePlate") && legs.type == mod.ItemType("DuneLeggings");  
        }
        public override void UpdateArmorSet(Player player)
        {
  
            player.setBonus = "4 successful hits on enemies with thrower weapons grants you the Desert Winds buff, causing an ancient knife to attack your foes";
            player.GetModPlayer<MyPlayer>(mod).duneSet = true;
        }
		        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DuneEssence", 16);
            recipe.AddTile(null,"EssenceDistorter");
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}