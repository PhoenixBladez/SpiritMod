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
            Item.width = Item.height = 16;
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 99;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = Item.useAnimation = 20;

            Item.noMelee = true;
            Item.consumable = true;
            Item.autoReuse = false;

            Item.UseSound = SoundID.Item43;
        }

        public override bool CanUseItem(Player player) => !MyWorld.jellySky && !Main.dayTime && (player.ZoneSkyHeight || player.ZoneOverworldHeight);

        public override bool? UseItem(Player player)
        {
			Main.NewText("Strange jellyfish are raining from the sky!", 61, 255, 142);
			MyWorld.jellySky = true;
    		if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);
            return true;
        }
    }
}