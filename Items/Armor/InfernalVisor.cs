using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class InfernalVisor : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pain Monger's Mask");
			Tooltip.SetDefault("Increases magic damage by 15% and magic critical strike chance by 9%");
		}


        int timer;

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 20;
            item.rare = 5;
            item.value = 72000;

            item.defense = 9;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicCrit += 9;
            player.magicDamage += 0.15f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("InfernalBreastplate") && legs.type == mod.ItemType("InfernalGreaves");
        }

        public override void UpdateArmorSet(Player player)
        {
            timer++;

            if (timer == 20)
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, 6);
                timer = 0;
            }
            {
                player.setBonus = "When under 25%, defense is decreased by 4, but Infernal Guardians surround you \n Infernal Guardians vastly increase magic damage and reduce mana cost \n Getting hurt may spawn multiple exploding Infernal Embers";
                player.GetModPlayer<MyPlayer>(mod).infernalSet = true;

            }
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "InfernalAppendage", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
