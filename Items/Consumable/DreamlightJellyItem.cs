using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Consumable
{
    public class DreamlightJellyItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dreamlight Jelly");
            Tooltip.SetDefault("'It exudes arcane energy'\nUse at nighttime to summon the Moon Jelly Wizard");
        }


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.rare = ItemRarityID.Green;
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
			if (Main.netMode == NetmodeID.SinglePlayer)
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("MoonWizard"));

			else if (Main.netMode == NetmodeID.MultiplayerClient && player == Main.LocalPlayer) {
				Vector2 spawnPos = player.Center;
				int tries = 0;
				int maxtries = 300;
				while ((Vector2.Distance(spawnPos, player.Center) <= 200 || WorldGen.SolidTile((int)spawnPos.X / 16, (int)spawnPos.Y / 16) || WorldGen.SolidTile2((int)spawnPos.X / 16, (int)spawnPos.Y / 16) || WorldGen.SolidTile3((int)spawnPos.X / 16, (int)spawnPos.Y / 16)) && tries <= maxtries) {
					spawnPos = player.Center + Main.rand.NextVector2Circular(800, 800);
					tries++;
				}

				if (tries >= maxtries)
					return false;

				SpiritMod.WriteToPacket(SpiritMod.instance.GetPacket(), (byte)MessageType.BossSpawnFromClient, (byte)player.whoAmI, mod.NPCType("MoonWizard"), (int)spawnPos.X, (int)spawnPos.Y).Send(-1);
			}
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;

        }
    }
}