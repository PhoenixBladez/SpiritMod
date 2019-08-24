using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class ReachBossSummon : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bramble Tooth");
			Tooltip.SetDefault("'A malevolent mixture of flora and fauna'\nSummons the Protector of the Reach");
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
            return !NPC.AnyNPCs(mod.NPCType("ReachBoss")) && player.GetModPlayer<MyPlayer>(mod).ZoneReach && Main.dayTime;
        }


    public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("ReachBoss"));
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BismiteCrystal", 2);
            recipe.AddIngredient(null, "EnchantedLeaf", 2);
            recipe.AddRecipeGroup("EvilMaterial1", 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
    }
}
