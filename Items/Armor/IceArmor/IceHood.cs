using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.IceArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class  IceHood : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blizzard Hood");
			Tooltip.SetDefault("Increases magic damage by 15% and increases maximum mana by 40");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 18;
            item.value = 46000;
            item.rare = 6;
            item.defense = 8;
        }
        public override void UpdateEquip(Player player)
        {
            player.magicDamage+= 0.15f;
            player.statManaMax2 += 40;

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("IceArmor") && legs.type == mod.ItemType("IceRobe");  
        }
        public override void UpdateArmorSet(Player player)
        {
  
            player.setBonus = "Magic hits occasionally grant you the Blizzard's Wrath";
            player.GetModPlayer<MyPlayer>(mod).icySet = true;
        }
		        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "IcyEssence", 16);
            recipe.AddTile(null,"EssenceDistorter");
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}