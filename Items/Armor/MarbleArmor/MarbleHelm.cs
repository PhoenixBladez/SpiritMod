using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.MarbleArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class MarbleHelm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marble Helm");
            Tooltip.SetDefault("Increases damage dealt by 5%\nIncreases movement speed by 6%");

        }
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = 1100;
            item.rare = 2;
            item.defense = 5;
        }
        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.05f;
            player.rangedDamage += 0.05f;
            player.magicDamage += 0.05f;
            player.minionDamage += 0.05f;
            player.thrownDamage += 0.05f;
            player.maxRunSpeed += 0.06f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("MarbleChest") && legs.type == mod.ItemType("MarbleLegs");
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Increases melee speed by 5% \n Increases life regen when moving";

            player.meleeSpeed += 0.05f;
            if (player.velocity.X != 0)
            {
                player.lifeRegen += 3;
            }
            if (player.velocity.Y != 0)
            {
                player.lifeRegen += 3;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MarbleChunk", 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
