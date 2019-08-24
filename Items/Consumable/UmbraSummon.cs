using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class UmbraSummon : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eerie Wisp");
			Tooltip.SetDefault("'A Dark Spirit awaits...'");
		}


        public override void SetDefaults()
        {
            item.width = 16;
                item.height = 26;
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
            if (!NPC.AnyNPCs(mod.NPCType("SpiritCore")) && player.GetModPlayer<MyPlayer>(mod).ZoneSpirit && !Main.dayTime)
                return true;
            return false;
        }

         public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SpiritCore"));
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DuskStone", 3);
            recipe.AddIngredient(null, "SpiritBar", 3);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
