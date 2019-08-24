using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.FieryArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class ObsidiusHelm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Hood");
			Tooltip.SetDefault("Increases throwing damage by 10%");
		}

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = Terraria.Item.sellPrice(0, 0, 35, 0);
            item.rare = 3;
            item.defense = 8;
        }
        public override void UpdateEquip(Player player)
        {
            player.thrownDamage += 0.1f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("ObsidiusPlate") && legs.type == mod.ItemType("ObsidiusGreaves");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CarvedRock", 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Thrower attacks can burn foes and call fireballs to protect you";
            player.GetModPlayer<MyPlayer>(mod).fierySet = true;
            if (Main.rand.Next(6) == 0)
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, 6);
            }
        }
    }
}
