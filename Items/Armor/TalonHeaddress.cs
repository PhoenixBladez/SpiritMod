using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class TalonHeaddress : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talon Headdress");
			Tooltip.SetDefault("8% increased magic damage and critical strike chance");
		}

        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 26;
            item.value = 10000;
            item.rare = 3;
            item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("TalonGarb") && legs.type == mod.ItemType("TalonClaws");  
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Wind Spirits guide you, granting you double jumps\nMagic and ranged attacks ocassionally spawn feathers to attack foes.";
            player.doubleJumpCloud = true;
            player.GetModPlayer<MyPlayer>(mod).talonSet = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.08f;
            player.magicCrit += 8;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Talon", 10);
            recipe.AddIngredient(null, "FossilFeather", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
