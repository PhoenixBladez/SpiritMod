using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class CoiledMask : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coiled Mask");
			Tooltip.SetDefault("Increases throwing velocity by 10%");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = Terraria.Item.sellPrice(0, 0, 15, 0);
            item.rare = 2;
            item.defense = 5;
        }
        public override void UpdateEquip(Player player)
        {
            player.thrownVelocity += 0.1f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("CoiledChestplate") && legs.type == mod.ItemType("CoiledLeggings");
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "TechDrive", 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        public override void UpdateArmorSet(Player player)
        {

            player.setBonus = "Go into Overdrive when your health reaches a critical level...";

            if (player.statLife < player.statLifeMax2 / 4)
            {
                player.AddBuff(mod.BuffType("OverDrive"), 420);
                int dust = Dust.NewDust(player.position, player.width, player.height, 172); 
            }
        }
    }
}
