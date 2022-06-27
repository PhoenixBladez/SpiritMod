using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BriarChestLoot
{
	[AutoloadEquip(EquipType.Neck)]
	public class ReachBrooch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forsworn Pendant");	
			Tooltip.SetDefault("Generates a bright glow around the player\nGives increased vision in the Briar at night or underground");	
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/BriarChestLoot/ReachBrooch");
		}

		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor) => glowMaskColor = Color.White;

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.buyPrice(silver: 2);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().reachBrooch = true;
			if (player.GetSpiritPlayer().ZoneReach && (!Main.dayTime || (!player.ZoneSkyHeight && !player.ZoneOverworldHeight)))
				player.AddBuff(BuffID.NightOwl, 2);

			Lighting.AddLight(player.Center, Color.Yellow.ToVector3() / 1.75f);
		}
	}
}
