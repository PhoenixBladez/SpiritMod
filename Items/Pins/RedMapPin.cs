using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Pins
{
    public class RedMapPin : ModItem
    {
        //TODO: Minimap integration, saving upon world exit, make them not draw in corner when not in use
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Wayfinder's Pin- Red");
            Tooltip.SetDefault("Places a map pin at your location\nRight Click to delete pin\nOnly one red pin can be placed at a time");
        }

        public override void SetDefaults() {

            item.width = 32;
            item.height = 32;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 5;
            item.value = 20000;
            item.rare = 2;
            item.autoReuse = false;
            item.shootSpeed = 0f;
        }
        public override bool AltFunctionUse(Player player) {
            return true;
        }
        public override bool UseItem(Player player) {
            Main.PlaySound(0, (int)player.position.X, (int)player.position.Y);
            if(player.altFunctionUse != 2) {
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 10, player.width, player.height), new Color(255, 255, 255, 100),
                "Location Pinned");
                ((MyPlayer)player.GetModPlayer(mod, "MyPlayer")).RedPinX = (int)(player.position.X / 16);
                ((MyPlayer)player.GetModPlayer(mod, "MyPlayer")).RedPinY = (int)(player.position.Y / 16);
            } else {
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 10, player.width, player.height), new Color(255, 255, 255, 100),
                "Pin Removed");
                ((MyPlayer)player.GetModPlayer(mod, "MyPlayer")).RedPinX = 0;
                ((MyPlayer)player.GetModPlayer(mod, "MyPlayer")).RedPinY = 0;
            }
            return true;
        }
    }
}