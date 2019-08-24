using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class CursedCloth : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pain Caller");
			Tooltip.SetDefault("Summons Infernon");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.rare = 4;
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
            return !NPC.AnyNPCs(mod.NPCType("Infernon")) && !(player.position.Y / 16f < Main.maxTilesY - 200);
        }

    public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Infernon"));
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofNight, 4);
            recipe.AddIngredient(null, "CarvedRock", 3);
            recipe.AddIngredient(ItemID.HellstoneBar, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
