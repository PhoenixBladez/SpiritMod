using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class DreamlightJellyItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dreamlight Jelly");
            Tooltip.SetDefault("'It exudes arcane energy'\nSummons the Moon Jelly Wizard");
        }


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.rare = 2;
            item.maxStack = 99;

            item.useStyle = 4;
            item.useTime = item.useAnimation = 20;

            item.noMelee = true;
            item.consumable = false;
            item.autoReuse = false;

            item.UseSound = SoundID.Item43;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(mod.NPCType("MoonWizard")) && !Main.dayTime && (player.ZoneSkyHeight || player.ZoneOverworldHeight);
        }


        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("MoonWizard"));
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;

        }
    }
}