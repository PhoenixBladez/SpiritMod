using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class ScarabIdol : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarab Idol");
			Tooltip.SetDefault("Summons the Sun's Insect");
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
            if (!NPC.AnyNPCs(mod.NPCType("Scarabeus")))
                return true;
            return false;
        }

 public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Scarabeus"));
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Sapphire, 1);
			recipe.AddIngredient(323, 3);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.Topaz, 1);
            recipe2.AddIngredient(323, 3);
            recipe2.AddTile(TileID.DemonAltar);
            recipe2.SetResult(this);
            recipe2.AddRecipe();

        }
    }
}
