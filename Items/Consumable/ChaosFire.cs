using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class ChaosFire : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Fire");
			Tooltip.SetDefault("'Summons the Master of the Hallow'");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.rare = 5;
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
            return !NPC.AnyNPCs(mod.NPCType("IlluminantMaster")) && !Main.dayTime;
        }

    public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("IlluminantMaster"));
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("EvilMaterial", 3);
            recipe.AddIngredient(547, 1);
			recipe.AddIngredient(548, 1);
			recipe.AddIngredient(549, 1);
			recipe.AddIngredient(502, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
