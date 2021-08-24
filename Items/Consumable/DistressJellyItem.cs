using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class DistressJellyItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Distress Jelly");
            Tooltip.SetDefault("'It needs help!'\nUse at nighttime to summon the Jelly Deluge");
        }

        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.rare = ItemRarityID.Green;
            item.maxStack = 99;

            item.useStyle = ItemUseStyleID.HoldingUp;
            item.useTime = item.useAnimation = 20;

            item.noMelee = true;
            item.consumable = true;
            item.autoReuse = false;

            item.UseSound = SoundID.Item43;
        }

        public override bool CanUseItem(Player player) => !MyWorld.jellySky && !Main.dayTime && (player.ZoneSkyHeight || player.ZoneOverworldHeight);

        public override bool UseItem(Player player)
        {
			Main.NewText("Strange jellyfish are pouring out of the sky!", 61, 255, 142);
			MyWorld.jellySky = true;
    		if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);
            return true;
        }
    }
}