using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.MarbleArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class MarbleLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marble Treads");
            Tooltip.SetDefault("Increases movement speed by 6%\nLeave a bright trail of light as you walk");

        }
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = 15100;
            item.rare = 2;
            item.defense = 4;
        }
        public override void UpdateEquip(Player player)
        {

            player.maxRunSpeed += 0.06f;
            int dust1 = Dust.NewDust(player.position, player.width, player.height, DustID.GoldCoin);
            Main.dust[dust1].scale += 0.5f;
            Main.dust[dust1].velocity *= 0f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MarbleChunk", 13);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
