
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class RawMeat : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Raw Meat");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 24;
			item.rare = ItemRarityID.White;
			item.maxStack = 1;

			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useTime = item.useAnimation = 20;

			item.noMelee = true;
			item.consumable = true;
			item.autoReuse = false;

			item.UseSound = SoundID.Item43;
		}

		public override bool OnPickup(Player player)
		{
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 2));
			{
				player.statLife += 10;
				player.HealEffect(10, true);
				player.AddBuff(BuffID.WellFed, 540);
			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(189, 191, 174, 100);
		}
		public override bool ItemSpace(Player player)
		{
			return true;
		}
	}
}
