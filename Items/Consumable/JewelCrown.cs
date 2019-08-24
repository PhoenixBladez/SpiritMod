using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class JewelCrown : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jewel Crown");
			Tooltip.SetDefault("'Summons the ruler of the skies'");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.rare = 2;
            item.maxStack = 99;

            item.useStyle = 4;
            item.useTime = item.useAnimation = 20;

            item.noMelee = true;
            item.consumable = true;
            item.autoReuse = false;

            item.UseSound = SoundID.Item43;
        }

             public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(mod.NPCType("AncientFlyer"));
        }

    public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("AncientFlyer"));
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BeeWax, 1);
            recipe.AddIngredient(null, "Talon", 4);
            recipe.AddIngredient(ItemID.Feather, 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
