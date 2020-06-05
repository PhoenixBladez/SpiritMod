using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class TalonGarb : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Apostle's Garb");
            Tooltip.SetDefault("Increases magic and ranged damage by 8%\nIncreases movement speed by 9%");

        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 22;
            item.value = 10000;
            item.rare = 3;
            item.defense = 5;
        }
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			// The equipSlot is added in ExampleMod.cs --> Load hook
			equipSlot = mod.GetEquipSlot("TalonGarb_Legs", EquipType.Legs);
		}
		
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
		}
        public override void UpdateEquip(Player player)
        {
            player.magicDamage += .08f;
            player.rangedDamage += .08f;
            player.moveSpeed += 0.09f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Talon", 16);
            recipe.AddIngredient(null, "FossilFeather", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
