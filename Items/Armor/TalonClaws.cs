using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class TalonClaws : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talon Claws");
            Tooltip.SetDefault("8% Increased magic damage and reduced mana cost\n5% increased movement speed");

        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 12;
            item.value = 10000;
            item.rare = 3;
            item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.08f;
            player.manaCost -= 0.08f;
            player.moveSpeed += 0.05f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Talon", 12);
            recipe.AddIngredient(null, "FossilFeather", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}