using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class FHelmet : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Helmet");
			Tooltip.SetDefault("Increases melee speed by 4% and movement speed by 6%");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 22;
            item.value = Terraria.Item.sellPrice(0, 0, 12, 0);
            item.rare = 2;
            item.defense = 5;
        }
        public override void UpdateEquip(Player player)
        {
            player.meleeSpeed += .04f;
            player.moveSpeed += .06f;
            player.maxRunSpeed += .03f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("FPlate") && legs.type == mod.ItemType("FLegs");  
        }
        public override void UpdateArmorSet(Player player)
        {
            timer++;

            if (timer == 20)
            {
                int d = Dust.NewDust(player.position, player.width, player.height, 39);
                Main.dust[d].velocity *= 0f;
                timer = 0;
            }

            player.setBonus = "Killing enemies may drop raw meat, restoring health and granting 'Well Fed'";
            player.GetSpiritPlayer().floranSet = true;
        }
        public override void AddRecipes()  
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FloranBar", 12);
            recipe.AddTile(TileID.Anvils);   
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
