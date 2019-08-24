using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class OceanHelmet : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diver's Helmet");
			Tooltip.SetDefault("Increases minion damage by 7%, and magic damage by 5% \nIncreases maximum number of minions by 1");
		}
        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.value = Terraria.Item.sellPrice(0, 0, 35, 0);
            item.rare = 3;
            item.defense = 6;
        }
        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.07f;
            player.magicDamage += 0.05f;
            player.maxMinions += 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Coral, 7);
            recipe.AddIngredient(null, "PearlFragment", 9);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("OceanChestplate") && legs.type == mod.ItemType("OceanGreaves");  
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Provides the Ability to double jump \n A mini Clamper fights for you!";
            player.doubleJumpSail = true;
            player.GetModPlayer<MyPlayer>(mod).oceanSet = true;
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
    }
}