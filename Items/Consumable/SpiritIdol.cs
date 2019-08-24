using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class SpiritIdol : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Idol");
			Tooltip.SetDefault("'Awaken the Being, asleep for aeons'");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.rare = 9;
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
            if (!NPC.AnyNPCs(mod.NPCType("Overseer")) && player.GetModPlayer<MyPlayer>(mod).ZoneSpirit && !Main.dayTime)
                return true;
            return false;
        }
public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Overseer"));
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(520, 4);
            recipe.AddIngredient(521, 4);
            recipe.AddIngredient(null, "SpiritBar", 4);
            recipe.AddIngredient(3467, 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
