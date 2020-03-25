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
			Tooltip.SetDefault("Increases melee speed by 10% and throwing velocity by 12%");
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
            player.setBonus = "Generates an icy aura that slows nearby enemies\nThis aura expands gradually, but resets if the player is hurt";
            player.GetSpiritPlayer().cryoSet = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeSpeed += 0.1f;
            player.thrownVelocity += .12f;
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
