using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using SpiritMod.Players;
using SpiritMod.Buffs;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Sets.VinewrathDrops
{
	public class DeathRose : ModItem
	{
		public override bool Autoload(ref string name)
		{
			DoubleTapPlayer.OnDoubleTap += DoubleTapPlayer_OnDoubleTap;
			return base.Autoload(ref name);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Briar Blossom");
			Tooltip.SetDefault("Double tap {0} to ensnare an enemy at the cursor position\n4 second cooldown");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string down = !Main.ReversedUpDownArmorSetBonuses ? "UP" : "DOWN";

			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "Tooltip0")
					line.text = line.text.Replace("{0}", down);
			}
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 36;
			item.value = Item.buyPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
			item.expert = true;
		}

		private void DoubleTapPlayer_OnDoubleTap(Player player, int keyDir)
		{
			if (keyDir == 1 && player.GetSpiritPlayer().deathRose && !player.HasBuff(ModContent.BuffType<DeathRoseCooldown>()))
			{
				player.AddBuff(ModContent.BuffType<DeathRoseCooldown>(), 240);
				Vector2 mouse = Main.MouseScreen + Main.screenPosition;
				Projectile.NewProjectile(mouse, Vector2.Zero, ModContent.ProjectileType<Projectiles.BrambleTrap>(), 30, 0, Main.myPlayer, mouse.X, mouse.Y);
			}
		}

		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetSpiritPlayer().deathRose = true;
	}
}
