using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CryoliteArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class CryoliteHead : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Helmet");
			Tooltip.SetDefault("Increases ranged damage by 6% and ranged critical strike chance by 4%");
		}

        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 26;
            item.value = 10000;
            item.rare = 3;
            item.defense = 6;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("CryoliteBody") && legs.type == mod.ItemType("CryoliteLegs");  
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Ranged attacks may freeze foes\nRanged attacks may deal extra ticks of damage to frozen foes\nThis effect does not apply to bosses";
            player.GetModPlayer<MyPlayer>(mod).cryoSet = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.06f;
            player.rangedCrit += 4;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CryoliteBar", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
