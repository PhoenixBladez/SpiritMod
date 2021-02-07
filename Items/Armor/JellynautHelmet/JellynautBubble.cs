using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Localization;

namespace SpiritMod.Items.Armor.JellynautHelmet
{
	[AutoloadEquip(EquipType.Head)]
	public class JellynautBubble : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jellynaut's Bubble");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/JellynautHelmet/JellynautBubble_Head_Glow");
            Tooltip.SetDefault("Increases maximum mana by 20\nIncreases critical strike chance by 4%\nProvides a special set bonus with any magic robes");
        }
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
			=> glowMaskColor = Color.White;

        public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;
			item.value = 3500;
			item.rare = 1;
			item.defense = 1;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.08f, .4f, .28f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Armor/JellynautHelmet/JellynautBubble_Glow"),
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (body.type >= 1282 && body.type <= 1287 || body.type == 2279);
        }
        public override void UpdateEquip(Player player)
        {
            player.meleeCrit += 4;
            player.rangedCrit += 4;
            player.magicCrit += 4;
            player.statManaMax2 += 20;
        }
        public override void UpdateArmorSet(Player player)
        {
            string tapDir = Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN");
            player.setBonus = $"Hitting or killing enemies with magic weapons generates arcane jellyfish around the player\nDouble tap {tapDir} to cause the jellyfish to attack the cursor position";
            player.GetSpiritPlayer().jellynautHelm = true;
        }
	}
}
